using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace System.Web
{
    public static class webUtil
    {

        //static NTier.Request.iBussinessTier __tier;

        //public NTier.Request.iBussinessTier oTier
        //{
        //    get
        //    {
        //        if (__tier == null)
        //        {
        //            __tier = NTier.Request.utility.createBussinessTierFromXmlForWeb(new NTier.clsAppServerInfo(ui.getAppServerRootPath(), ui.getAppName()));
        //        }

        //        return __tier;

        //    }
        //}

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
            setCookie("userid", "0");
        }

    }





}
