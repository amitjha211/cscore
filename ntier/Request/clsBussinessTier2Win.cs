using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace NTier.Request
{
    
    internal class clsBussinessTier2Win : clsBussinessTier2Base
    {
        System.Collections.Specialized.NameValueCollection clnCookie = new System.Collections.Specialized.NameValueCollection();
        public clsBussinessTier2Win(clsAppServerBase appServerInfo
            , string sMainApp)
            : base(appServerInfo, sMainApp)
        {
        }

        public clsBussinessTier2Win(clsAppServerConfigFiles configFiles)
            : base(configFiles)
        {

        }

        public override void setCookie(string sKey, string sValue)
        {
            clnCookie.Set(sKey, sValue);
        }

        public override string getCookie(string sKey)
        {
            return clnCookie[sKey];
        }
    }
}
