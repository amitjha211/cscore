using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace NTier.Request
{
    internal class clsBussinessTier2Web : clsBussinessTier2Base
    {

        public clsBussinessTier2Web(clsAppServerBase appServerInfo
            , string sMainApp)
            : base(appServerInfo, sMainApp)
        {


        }

        public clsBussinessTier2Web(clsAppServerConfigFiles configFiles)
            : base(configFiles)
        {
        }

        public override void setCookie(string sKey, string sValue)
        {
            HttpCookie myUserCookie = new HttpCookie(sKey);
            myUserCookie.Value = sValue;
            HttpContext.Current.Response.Cookies.Add(myUserCookie);
        }


        public override string getCookie(string sKey)
        {
            if (HttpContext.Current.Request.Cookies.AllKeys.Contains(sKey))
                return HttpContext.Current.Request.Cookies[sKey].Value;
            else
                return "";
        }


    }
}
