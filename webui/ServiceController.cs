using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Data;

namespace webui
{
    public abstract class ServiceControllerBase : myController
    {


        public abstract string AppServerRootPath { get  ; }
        public abstract string AppName { get; }



        Dictionary<string, NTier.Request.iBussinessTier> clnTier = new Dictionary<string, NTier.Request.iBussinessTier>();

        NTier.Request.iBussinessTier __tier;

        protected NTier.Request.iBussinessTier _tier
        {
            get
            {
                if (__tier == null)
                {
                    __tier = NTier.Request.utility.createBussinessTierFromXmlForWeb2(new NTier.clsAppServerInfo(AppServerRootPath, AppName), AppName);
                }

                return __tier;
            }
        }

        protected NTier.Request.iBussinessTier getTier(string sOtherLibApp)
        {
            if (!clnTier.ContainsKey(sOtherLibApp))
            {
                clnTier.Add(sOtherLibApp, NTier.Request.utility.createBussinessTierFromXmlForWeb2(new NTier.clsAppServerInfo(AppServerRootPath, sOtherLibApp), AppName));
            }
            return clnTier[sOtherLibApp];
        }


        public void addParamFromPost(clsCmd cmd, FormCollection frm)
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var f = Request.Files[i];
                string sField = Request.Files.AllKeys[i];
                cmd.Files.Add(new FileData() { FileName = f.FileName, Data = g.ConvertStreamToByteArray(f.InputStream), ContentType = f.ContentType, FieldName = sField });
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




        /*
        public ActionResult ViewPDF()
        {
            string sIndex = Request.QueryString[0];
            //string sPath = string.Format("D:\\current\\airtel_Chennai\\testingPDFs\\pdfs\\ch{0}.pdf",sIndex);
            var cmd = new clsCmd();
            cmd.setValue("ID", sIndex);

            var t = _tier.getData("data", cmd).Obj as DataTable;
            string sPath = "";
            if (t.Rows.Count > 0)
                sPath = t.Rows[0]["filename1"].ToString();

            return File(System.IO.File.ReadAllBytes(sPath), "application/pdf");
        }
         */


        int _start = 0;
        int _length = 10;
        int _draw = 1;

        protected void setPageSize(int iPageSize)
        {
            _length = iPageSize;
        }

        public int start
        {
            get
            {
                //get start to skip data
                if (Request["start"] != null && Request["start"] != "")
                    _start = Convert.ToInt32(Request["start"]);

                return _start;
            }
        }


        public int length
        {
            get
            {
                if (Request["length"] != null && Request["length"] != "")
                    _length = Convert.ToInt32(Request["length"]);

                return _length;
            }
        }

        public int draw
        {
            get
            {
                if (Request["draw"] != null && Request["draw"] != "")
                    _draw = Convert.ToInt32(Request["draw"]);

                return _draw;
            }
        }


        private class clsRequestInfo
        {
            public string appName { get; set; }
            public string Path { get; set; }

            public clsRequestInfo(string DefaultAppName
                , string sPath)
            {
                if (sPath.Contains(":"))
                {
                    string[] sSplittedValues = sPath.Split(':');
                    appName = sSplittedValues[0];
                    Path = sSplittedValues[1];
                }
                else
                {
                    appName = DefaultAppName;
                    Path = sPath;
                }
            }
        }

        private clsRequestInfo getRequestPathInfo(string sPath)
        {
            var oRequestPathInfo = new clsRequestInfo(AppName, sPath);
            return oRequestPathInfo;
        }

        public ContentResult getdataAll(FormCollection frm)
        {

            var cmd = new clsCmd();
            addParamFromPost(cmd, frm);

            string spath = Request.QueryString["path"];
            clsRequestInfo oRequestInfo = getRequestPathInfo(spath);

            DataTable t = null;
            clsMsg result;

            if (oRequestInfo.Path.StartsWith("drp\\"))
            {
                result = getTier(oRequestInfo.appName).getDropDownData(oRequestInfo.Path.Substring(4), cmd);
            }
            else
            {
                result = getTier(oRequestInfo.appName).getData(oRequestInfo.Path, cmd);
            }


            t = result.Obj as DataTable;
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(t), "application/json");
        }


        //this is not required later on delete this code.
        /*
        public ContentResult getdataPaging_vue(FormCollection frm)
        {
            
            var cmd = new clsCmd();
            addParamFromPost(cmd, frm);
            string sPath = Request.QueryString["path"];

            clsRequestInfo oRequestInfo = getRequestPathInfo(sPath);

            string sSortType = cmd.getStringValue("$sort");
            if (cmd.ContainFields("$sort"))
            {
                cmd.Remove(cmd["$sort"]);
            }

            var result = getTier(oRequestInfo.appName).getData(oRequestInfo.Path, cmd);
            if (result.Validated)
            {
                //var t = _tier.getData(sPath, cmd).Obj as DataTable;
                DataTable t = result.Obj as DataTable;
                var tPaging = g.getJsonPaging(t, sSortType, start, length);
                return Content(Newtonsoft.Json.JsonConvert.SerializeObject(tPaging), "application/json");
            }
            else
            {
                var res = new { draw = draw, recordsTotal = 0, recordsFiltered = 0, data = "", error = true, error_msg = result.Message };
                return Content(Newtonsoft.Json.JsonConvert.SerializeObject(res), "application/json");
            }

        }
        */

        public ContentResult getdataPaging(FormCollection frm)
        {
            var cmd = new clsCmd();
            addParamFromPost(cmd, frm);
            string sPath = Request.QueryString["path"];

            clsRequestInfo oRequestInfo = getRequestPathInfo(sPath);

            string sSortType = cmd.getStringValue("$sort");
            if (cmd.ContainFields("$sort"))
            {
                cmd.Remove(cmd["$sort"]);
            }

            var result = getTier(oRequestInfo.appName).getData(oRequestInfo.Path, cmd);

            if (result.Validated)
            {
                //var t = _tier.getData(sPath, cmd).Obj as DataTable;
                DataTable t = result.Obj as DataTable;
                var tPaging = g.getJsonPaging(t, sSortType, start, length);
                return Content(Newtonsoft.Json.JsonConvert.SerializeObject(tPaging), "application/json");
            }
            else
            {
                var res = new { draw = draw, recordsTotal = 0, recordsFiltered = 0, data = "", error = true, error_msg = result.Message };
                return Content(Newtonsoft.Json.JsonConvert.SerializeObject(res), "application/json");
            }

        }


        [HttpPost, ValidateInput(false)]
        public JsonResult UpdateModule(FormCollection frm)
        {
            string sPath = Request.QueryString["path"];
            clsRequestInfo oRequestInfo = getRequestPathInfo(sPath);

            var cmd = new clsCmd();
            addParamFromPost(cmd, frm);


            if (!sPath.isEmpty())
            {
                try
                {
                    var result = getTier(oRequestInfo.appName).exec(oRequestInfo.Path, cmd);
                    return Json(new { msg = result.Message, data = result.Obj }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { msg = ex.Message, data = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                object n = null;
                return Json(new { msg = "You have not specified [Module Name] and OperationName", data = n }, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost, ValidateInput(false)]
        public JsonResult setReport(FormCollection frm)
        {

            string sPath = Request.QueryString["path"];
            clsRequestInfo oRequestInfo = getRequestPathInfo(sPath);

            var cmd = new clsCmd();
            addParamFromPost(cmd, frm);


            if (!string.IsNullOrWhiteSpace(sPath))
            {
                try
                {
                    var rpt = getTier(oRequestInfo.appName).getSQLReport(oRequestInfo.Path, cmd);
                    Session["rpt"] = rpt;
                    Session["rptName"] = rpt.downloadName.isEmpty() ? sPath : rpt.downloadName;
                    return Json(new { msg = "", data = "" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { msg = ex.Message, data = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                object n = null;
                return Json(new { msg = "You have not specified [Report path]", data = n }, JsonRequestBehavior.AllowGet);
            }
        }



        public FileResult downloadSQLReport()
        {
            var rpt = (NTier.sqlReport.iSQLReport)Session["rpt"];

            string sPath = Server.MapPath("~/output.pdf");

            using (FileStream fs = new FileStream(sPath, FileMode.Create))
            {

                string sFileName = Session["rptName"] == null ? "output" : Session["rptName"].ToString();
                string sFileType = Request.QueryString["filetype"].isEmpty() == null ? "pdf" : Request.QueryString["filetype"];

                //Extension manipulation
                string sFileExtension = "pdf";

                switch (sFileType.ToLower())
                {
                    case "excel":
                        sFileExtension = "xls";
                        break;
                    default:
                        sFileExtension = "pdf";
                        break;
                }

                //end 
                rpt.render(sFileType, fs);
                sFileName += string.Format(".{0}", sFileExtension);
                return File(g.ConvertStreamToByteArray(fs), "application/unknown", sFileName);
            }
        }


        public JsonResult setExcelForDownload(FormCollection frm)
        {
            //string sModuleName = Request.QueryString["ModuleName"];
            //string sSubModuleName = Request.QueryString["SubModuleName"];

            //var cmd = new clsCmd();
            //addParamFromPost(cmd, frm);
            //var t = _oBL.getExcelData(sModuleName, sSubModuleName, cmd);
            //Session["excel_data"] = t;
            //return Json(new { msg = "" });

            return null;
        }

        public FileResult DownloadExcel()
        {
            //var t = Session["excel_data"] as System.Data.DataTable;
            //var f = _oBL.ConvertToExcelFile(t);
            //return File(f.Data, "application/unknown", "data.xls");

            return null;
        }



        public clsMsg getFileFromQueryString(System.Collections.Specialized.NameValueCollection qs)
        {



            string sPath = qs["path"];
            clsRequestInfo oRequestInfo = getRequestPathInfo(sPath);

            string[] sKeys = qs.AllKeys;
            var cmd = new clsCmd();

            foreach (string sKey in sKeys)
            {
                if (sKey.isEmpty() == false && sKey.ToUpper() != "PATH")
                {
                    cmd.setValue(sKey, qs[sKey]);
                }
            }

            var msg = getTier(oRequestInfo.appName).getFileContent(oRequestInfo.Path, cmd);
            return msg;


        }

        public ActionResult getFileContent()
        {
            var oFile = getFileFromQueryString(Request.QueryString).Obj as FileData;
            return File(oFile.Data, oFile.ContentType);
        }


        [HttpPost]
        public ActionResult setFileForDownload(FormCollection frm)
        {


            string sPath = Request.QueryString["path"];
            clsRequestInfo oRequestInfo = getRequestPathInfo(sPath);

            var cmd = new clsCmd();
            addParamFromPost(cmd, frm);


            if (!string.IsNullOrWhiteSpace(sPath))
            {
                try
                {
                    var msg = getTier(oRequestInfo.appName).getFileContent(oRequestInfo.Path, cmd);

                    if (msg.Validated)
                    {
                        Session["File"] = msg.Obj as FileData;
                        return Json(new { msg = "", data = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { msg = msg.Message, data = "" }, JsonRequestBehavior.AllowGet);
                    }
                    
                }
                catch (Exception ex)
                {
                    return Json(new { msg = ex.Message, data = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                object n = null;
                return Json(new { msg = "You have not specified [Report path]", data = n }, JsonRequestBehavior.AllowGet);
            }









            /////////////////



            /*

            var msg = getFileFromQueryString(Request.QueryString);

            if (msg.Validated)
            {

                Session["File"] = msg.Obj as FileData;
                return Json(new { msg = "", data = "" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { msg = "", data = "" }, JsonRequestBehavior.AllowGet);
        
             */
        }


        public ActionResult DownloadFile()
        {
            FileData oFile = Session["File"] as FileData;
            return File(oFile.Data, "application/unknown", oFile.FileName);
        }


        [NonAction]
        private ActionResult getContentType(string sFilePath)
        {
            string sExtension = System.IO.Path.GetExtension(sFilePath);

            string sContentType = "";
            switch (sExtension.ToLower())
            {
                case ".js":
                    sContentType = "text/javascript";
                    break;
                case ".html":
                case ".htm":
                    sContentType = "text/html";
                    break;
                case ".pdf":
                    sContentType = "application/pdf";
                    break;
                case ".css":
                    sContentType = "text/css";
                    break;
                case ".xml":
                    sContentType = "text/xml";
                    break;
                case ".jpg":
                case ".gif":
                    sContentType = "image/*";
                    break;

                default:
                    sContentType = "application/unknown";
                    break;
            }


            switch (sExtension.ToLower())
            {
                case ".pdf":
                    return File(System.IO.File.ReadAllBytes(sFilePath), sContentType);
                case ".gif":
                case ".jpg":
                    return File(System.IO.File.ReadAllBytes(sFilePath), sContentType,"test.gif");
                default:
                    return Content(System.IO.File.ReadAllText(sFilePath), sContentType);
            }

        }

        public ActionResult appServiceContent()
        {
            string qPath = Request.QueryString["path"];
            qPath = qPath.Replace("/", "\\");
            string sPath = ui.appServicePath + qPath;
            return getContentType(sPath);
        }


    }
}