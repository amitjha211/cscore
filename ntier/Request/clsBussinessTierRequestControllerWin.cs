using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace NTier.Request
{
    internal class clsBussinessTierRequestControllerWin : iBussinessTier
    {


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

        clsAppServerBase _oAppServerInfo;
        string _sMainApp;
        Dictionary<string, NTier.Request.iBussinessTier> clnTier = new Dictionary<string, NTier.Request.iBussinessTier>();

        private clsRequestInfo getRequestPathInfo(string sPath)
        {
            var oRequestPathInfo = new clsRequestInfo(_sMainApp, sPath);
            return oRequestPathInfo;
        }

        protected NTier.Request.iBussinessTier getTier(string sOtherLibApp)
        {
            if (!clnTier.ContainsKey(sOtherLibApp))
            {
                clnTier.Add(sOtherLibApp, NTier.Request.utility.createBussinessTierFromXmlForWin2(new NTier.clsAppServerInfo(_oAppServerInfo.appServerRootPath, sOtherLibApp), _sMainApp));
            }
            return clnTier[sOtherLibApp];
        }

        public clsBussinessTierRequestControllerWin(clsAppServerBase oAppServerInfo, string sMainApp)
        {
            _oAppServerInfo = oAppServerInfo;
            _sMainApp = sMainApp;
        }

        public string getAppSetting(string sKey)
        {
            return getTier(_sMainApp).getAppSetting(sKey);
        }

        public void setCookie(string sKey, string sVal)
        {
            getTier(_sMainApp).setCookie(sKey, sVal);
        }

        public string getCookie(string sKey)
        {
            return getTier(_sMainApp).getCookie(sKey);
        }

        public CRUD.clsCRUD getCRUD(string sCRUDName)
        {
            clsRequestInfo oRequestInfo = getRequestPathInfo(sCRUDName);
            return getTier(oRequestInfo.appName).getCRUD(oRequestInfo.Path);
        }

        public clsMsg getData(string sPath, clsCmd cmd)
        {
            
            clsRequestInfo oRequestInfo = getRequestPathInfo(sPath);
            return getTier(oRequestInfo.appName).getData(oRequestInfo.Path, cmd);
        }

        public clsMsg getDropDownData(string sPath, clsCmd cmd)
        {
            clsRequestInfo oRequestInfo = getRequestPathInfo(sPath);
            return getTier(oRequestInfo.appName).getDropDownData(oRequestInfo.Path, cmd);
        }

        public clsMsg exec(string sPath, clsCmd cmd)
        {
            clsRequestInfo oRequestInfo = getRequestPathInfo(sPath);
            return getTier(oRequestInfo.appName).exec(oRequestInfo.Path, cmd);
        }

        public adapter.clsDataAdapterBase getAdapter(string sKey = "")
        {
            clsRequestInfo oRequestInfo = getRequestPathInfo(sKey);
            return getTier(oRequestInfo.appName).getAdapter(oRequestInfo.Path);
        }


        public sqlReport.SQLReportBase getSQLReport(string sPath, clsCmd cmd)
        {
            clsRequestInfo oRequestInfo = getRequestPathInfo(sPath);
            return getTier(oRequestInfo.appName).getSQLReport(oRequestInfo.Path,cmd);
        }

        public string getPath(string sPath)
        {
            clsRequestInfo oRequestInfo = getRequestPathInfo(sPath);
            return getTier(oRequestInfo.appName).getPath(oRequestInfo.Path);
        }

        public clsMsg getFileContent(string sPath, clsCmd cmd)
        {
            clsRequestInfo oRequestInfo = getRequestPathInfo(sPath);
            return getTier(oRequestInfo.appName).getFileContent(oRequestInfo.Path,cmd);
        }
    }
}
