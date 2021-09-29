using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTier.Request
{

    
    public partial class utility
    {

        public static iBussinessTier createBussinessTierFromXmlForWeb(clsAppServerBase oAppServerInfo)
        {
            clsBussinessTierFromXmlBase obj = new clsBussinessTierFromXmlForWeb(oAppServerInfo);
            return obj;
        }
        
        public static iBussinessTier createBussinessTierFromXmlForWeb2(clsAppServerBase oAppServerInfo,string sMainApp)
        {
            clsBussinessTier2Web obj = new clsBussinessTier2Web(oAppServerInfo,sMainApp);
            return obj;
        }

        public static iBussinessTier createBussinessTierFromXmlForWeb2(clsAppServerConfigFiles configFiles)
        {
            clsBussinessTier2Web obj = new clsBussinessTier2Web(configFiles);
            return obj;
        }


        public static iBussinessTier createBussinessTierFromXmlForWin2(clsAppServerBase oAppServerInfo, string sMainApp)
        {
            iBussinessTier obj = new clsBussinessTier2Win(oAppServerInfo, sMainApp);
            return obj;
        }

        public static iBussinessTier createBussinessTierFromXmlForWin2(clsAppServerConfigFiles configFiles)
        {
            iBussinessTier obj = new clsBussinessTier2Win(configFiles);
            return obj;
        }



        public static iBussinessTier createBussinessTierControllerFromXmlForWin2(clsAppServerBase oAppServerInfo, string sMainApp)
        {
            iBussinessTier obj = new clsBussinessTierRequestControllerWin(oAppServerInfo, sMainApp);
            return obj;
        }
    }
}
