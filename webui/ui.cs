using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace System
{
    public static class ui
    {

       

        public static string appServicePath
        {
            get { return Configuration.ConfigurationManager.AppSettings["appServicePath"]; }
        }


        public static NTier.Request.iBussinessTier createBussinessTier(string appName)
        {

            string sAppServerRootPath = ui.appServicePath + "\\AppServer";

            var oTier =  NTier.Request.utility.createBussinessTierFromXmlForWeb2(new NTier.clsAppServerInfo(sAppServerRootPath, appName),appName);

            return oTier;
        }

        

        public static string assets_global_link
        {
            get
            {
                if (!Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("assets_global_link"))
                    return "";
                else
                    return Configuration.ConfigurationManager.AppSettings["assets_global_link"];
            }

        }

        public static MvcHtmlString sharedWebContent(string sAdditionalUrl)
        {
            string sPath = appServicePath + "\\web\\" + sAdditionalUrl.Replace("/", "\\");
            return new MvcHtmlString(System.IO.File.ReadAllText(sPath));
        }

        private static string getServiceControllerLink()
        {
            if (Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("Service_Controller_Link"))
            {
                return Configuration.ConfigurationManager.AppSettings["Service_Controller_Link"];
            }
            else
                return "../Service/";
        }

        public static string url_admin(string addUrl)
        {
            return getServiceControllerLink() + "appServiceContent?path=" + "/web" + addUrl;
        }

        public static string url_global(string addUrl)
        {
            if (assets_global_link.isEmpty() == false)
            {
                return assets_global_link + addUrl.Replace("\\", "/");
            }
            else
            {
                return getServiceControllerLink() + "appServiceContent?path=" + "/web/global" + addUrl;
            }
        }


        public static void logOut()
        {
            HttpContext.Current.Response.Cookies.Remove("userid");
        }





    }


}
