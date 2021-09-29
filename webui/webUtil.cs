using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NTier.Request;
using System.Data;

namespace System
{
    public static class webUtil
    {

        public static string getWebAppSettings(string sKey)
        {

            return Configuration.ConfigurationManager.AppSettings[sKey];
        }

        public static string appServicePath
        {
            get { return Configuration.ConfigurationManager.AppSettings["appServicePath"]; }
        }


        public static NTier.Request.iBussinessTier createTier(string sRoot)
        {
            return createTier(sRoot, getWebAppSettings("env"));
        }

        public static NTier.Request.iBussinessTier createTier(string sRoot
            , string sEnv)
        {


            var lstFiles = System.IO.Directory.GetFiles(sRoot + "\\appServer\\apps").ToList();


            if (lstFiles.Count == 0) throw new Exception("No configuration xml file found in configured appService folder !");

            var configFiles = new NTier.clsAppServerConfigFiles();

            configFiles.Add("", string.Format("{0}\\appServer\\apps\\env.{1}.xml", sRoot, sEnv));

            foreach (string sFile in lstFiles)
            {

                string sFileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(sFile);

                if (sFileNameWithoutExtension.StartsWith("env.")) continue;

                if (sFile.EndsWith("main.xml"))
                {
                    configFiles.Add("", sFile);
                }
                else
                {
                    configFiles.Add(sFileNameWithoutExtension + ":", sFile);
                }
            }


            var _tier = NTier.Request.utility.createBussinessTierFromXmlForWeb2(configFiles);
            return _tier;
        }


        public static NTier.Request.iBussinessTier createTierWin(string sRoot, string sEnv)
        {


            var lstFiles = System.IO.Directory.GetFiles(sRoot + "\\appServer\\apps").ToList();


            if (lstFiles.Count == 0) throw new Exception("No configuration xml file found in configured appService folder !");

            var configFiles = new NTier.clsAppServerConfigFiles();

            configFiles.Add("", string.Format("{0}\\appServer\\apps\\env.{1}.xml", sRoot, sEnv));

            foreach (string sFile in lstFiles)
            {

                string sFileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(sFile);

                if (sFileNameWithoutExtension.StartsWith("env.")) continue;

                if (sFile.EndsWith("main.xml"))
                {
                    configFiles.Add("", sFile);
                }
                else
                {
                    configFiles.Add(sFileNameWithoutExtension + ":", sFile);
                }
            }


            var _tier = NTier.Request.utility.createBussinessTierFromXmlForWin2(configFiles);
            return _tier;
        }

        private static iBussinessTier __tier = null;
        public static iBussinessTier tier
        {
            get
            {

                if (__tier == null)
                {
                    __tier = createTier(appServicePath);
                }

                return __tier;
            }
        }

        public static void setCookie(string sKey
            , string sValue)
        {
            HttpCookie myUserCookie = new HttpCookie(sKey);
            myUserCookie.Value = sValue;
            HttpContext.Current.Response.Cookies.Add(myUserCookie);
        }

        public static string getCookie(string sKey)
        {
            if (HttpContext.Current.Request.Cookies.AllKeys.Contains(sKey))
                return HttpContext.Current.Request.Cookies[sKey].Value;
            else
                return "";
        }



        public static void logOut()
        {

            __tier = null;
            _row = null;
            setCookie("userid", "0");
        }



        public static void addParamFromPost(clsCmd cmd, FormCollection frm)
        {

            for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
            {
                var f = HttpContext.Current.Request.Files[i];
                string sField = HttpContext.Current.Request.Files.AllKeys[i];

                var objFileData = new FileData();
                objFileData.FileName = f.FileName;
                objFileData.Data = g.ConvertStreamToByteArray(f.InputStream);
                objFileData.ContentType = f.ContentType;
                objFileData.FieldName = sField;
                objFileData.FileExtension = System.IO.Path.GetExtension(f.FileName);

                cmd.Files.Add(objFileData);
            }



            foreach (string sKey in frm.AllKeys)
            {
                if (!string.IsNullOrWhiteSpace(sKey) && !sKey.Contains("$$"))
                {
                    var p = new Param();

                    p.Name = sKey;
                    p.Value = frm[sKey];

                    cmd.Add(p);
                }
            }

        }


        public static void addParamFromPost(string sContain, clsCmd cmd, FormCollection frm)
        {

            foreach (string sKey in frm.AllKeys)
            {

                if (sKey.Substring(0, sContain.Length) == sContain)
                {
                    if (!string.IsNullOrWhiteSpace(frm[sKey]))
                    {
                        var p = new Param();
                        string[] sNames = sKey.Substring(sContain.Length, sKey.Length - sContain.Length).Split('~');
                        p.Name = sNames[0];

                        if (sNames.Length > 1) p.Operator = sNames[1];

                        if (p.Operator == "NOT LIKE" || p.Operator == "LIKE")
                            p.Value = "%" + frm[sKey].Replace(' ', '%') + "%";
                        else
                            p.Value = frm[sKey];


                        cmd.Add(p);
                    }
                }
            }


        }

        private static DataRow _row = null;



        private static void init_row()
        {

            if (_row == null)
            {
                var t = tier.getAdapter().getData("select * from vsysUser where id = " + sysUserID());
                if (t.Rows.Count > 0) _row = t.Rows[0];
            }
        }


        public static int sysUserID()
        {
            return g.parseInt(tier.getCookie("userid"));
        }
        public static int sysOrgID()
        {
            init_row();
            return g.parseInt(_row["sysOrgID"]);
        }
        public static int sysRoleID()
        {
            init_row();
            return g.parseInt(_row["sysRoleID"]);
        }


        public static bool isSuperUser()
        {
            init_row();
            return (bool)(g.isNull(_row["isSuperUser"], false));
        }

    }





}
