using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace System
{

    public static class ui_old
    {

        internal static string getAssetLink()
        {
            return Configuration.ConfigurationManager.AppSettings["assetLink"];
        }


        internal static string getAppServerRootPath()
        {

            string sFolder = Configuration.ConfigurationManager.AppSettings["appServerRootPath"];

            if (sFolder == "~")
            {
                sFolder = HttpContext.Current.Server.MapPath("~/AppServer");
            }

            return sFolder;
        }

        internal static string getAssetAppFolderPath()
        {
            string sFolder = Configuration.ConfigurationManager.AppSettings["assetAppPath"];

            return sFolder;
        }


        internal static string getAppSettings(string sKey)
        {
            return Configuration.ConfigurationManager.AppSettings[sKey];
        }
        internal static string getAppName()
        {
            return Configuration.ConfigurationManager.AppSettings["appName"];
        }

        public static string assetUrl(string additionalUrl)
        {

            return getAssetLink() + additionalUrl;
        }

        public static string assetAppUrl(string additionalUrl)
        {

            return "../Service/appContent?path=" + additionalUrl;
        }

        public static MvcHtmlString headerInclude()
        {
            string sPath = getAppServerRootPath() + "\\webResource\\headerInclude.html";

            StringBuilder sb1 = new StringBuilder(System.IO.File.ReadAllText(sPath));
            sb1.Replace("[assets]", getAssetLink());

            return new MvcHtmlString(sb1.ToString());

        }

        public static MvcHtmlString appContent(string sAdditionalUrl)
        {
            string sPath = getAssetAppFolderPath() + sAdditionalUrl.Replace("/", "\\");
            return new MvcHtmlString(System.IO.File.ReadAllText(sPath));
        }


        public static MvcHtmlString appView(string sAdditionalUrl)
        {
            string sPath = getAssetAppFolderPath() + "\\apps" + sAdditionalUrl;
            return new MvcHtmlString(System.IO.File.ReadAllText(sPath));
        }


        //private static NTier.Request.iBussinessTier __tier;
        //internal static NTier.Request.iBussinessTier oTier
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

    }
}


