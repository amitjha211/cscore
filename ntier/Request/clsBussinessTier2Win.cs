using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace NTier.Request
{
    class clsBussinessTier2Win : clsBussinessTier2Base
    {
        System.Collections.Specialized.NameValueCollection clnCookie = new System.Collections.Specialized.NameValueCollection();
        public clsBussinessTier2Win(clsAppServerBase appServerInfo
            , string sMainApp)
            : base(appServerInfo, sMainApp)
        {
        }

        public override void setCookie(string sKey, string sValue)
        {
            clnCookie.Set(sKey, sValue);
            HttpCookie myUserCookie = new HttpCookie(sKey);
            myUserCookie.Value = sValue;
            HttpContext.Current.Response.Cookies.Add(myUserCookie);
        }

        public override string getCookie(string sKey)
        {
            return clnCookie[sKey];
        }
    }
}
