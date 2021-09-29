using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Xml;
using System.Data;
using System.Web;
namespace NTier.Request
{

    internal abstract class clsBussinessTier2Base : iBussinessTier
    {

        private myAssembly oAssembly = new myAssembly();

        clsAppServerBase _appServerInfo = null;
        NameValueCollection clnAppSettings = new NameValueCollection();
        adapter.clsDataAdapterBase _adapter = null;

        Dictionary<string, NTier.CRUD.clsCRUD> clnCRUDs = new Dictionary<string, CRUD.clsCRUD>();
        Dictionary<string, NTier.Request.clsRequestGetDataBase> clnGetData = new Dictionary<string, clsRequestGetDataBase>();
        Dictionary<string, NTier.Request.clsRequestGetDataBase> clnDropDown = new Dictionary<string, clsRequestGetDataBase>();
        Dictionary<string, NTier.Request.clsRequestCommandBase> clnCmd = new Dictionary<string, clsRequestCommandBase>();
        Dictionary<string, NTier.Request.clsRequestSQLReportBase> clnSQLReport = new Dictionary<string, clsRequestSQLReportBase>();
        Dictionary<string, NTier.Request.clsRequestFileData_Base> clnFileData = new Dictionary<string, clsRequestFileData_Base>();


        public abstract void setCookie(string sKey, string sValue);
        public abstract string getCookie(string sKey);

        public clsBussinessTier2Base(clsAppServerConfigFiles configFiles)
        {

            foreach (clsAppServerConfigFile configFile in configFiles)
            {
                loadXmlFile(configFile.keyAttr, configFile.path);
            }
        }

        public void loadXmlFile(string sKeyAttr, string sPath)
        {

            var xDoc = new XmlDocument();
            xDoc.Load(sPath);
            //////////////////////////////////
            fillAppSettings(sKeyAttr, xDoc);
            setAdapter(xDoc);
            setCRUD("", xDoc); // key attribute not required
            setGetData(sKeyAttr, xDoc);
            setDropDown(sKeyAttr, xDoc);
            setCMD(sKeyAttr, xDoc);
            setSQLReport(sKeyAttr, xDoc);
            setFileData(sKeyAttr, xDoc);

        }


        public clsBussinessTier2Base(clsAppServerBase appServerInfo
            , string sMainApp)
        {

            _appServerInfo = appServerInfo;

            var xDocParent = new XmlDocument();
            var xDoc = new XmlDocument();

            string sPath = _appServerInfo.getAppConfigFilePath();
            if (System.IO.File.Exists(sPath))
                xDoc.Load(_appServerInfo.getAppConfigFilePath());

            sPath = _appServerInfo.appServerRootPath + "\\" + sMainApp + ".xml";
            if (System.IO.File.Exists(sPath))
                xDocParent.Load(sPath);

            fillAppSettings("", xDoc);
            fillAppSettings("", xDocParent);

            setAdapter(xDoc);
            setAdapter(xDocParent);


            setCRUD("", xDoc);
            setCRUD("", xDocParent);


            setGetData("", xDoc);
            setGetData("", xDocParent);

            setDropDown("", xDoc);
            setDropDown("", xDocParent);


            setCMD("", xDoc);
            setCMD("", xDocParent);

            setSQLReport("", xDoc);
            setSQLReport("", xDocParent);

            setFileData("", xDoc);
            setFileData("", xDocParent);
        }

        private void setAdapter(XmlDocument xDoc)
        {

            if (xDoc == null) return;

            if (_adapter != null) return;


            var xNodeConnection = xDoc.SelectSingleNode("//appConfig/defaultConnectionString");
            if (xNodeConnection == null) return;

            string sConnectionString = xNodeConnection.InnerText;
            string sConnectionType = xNodeConnection.getXmlAttributeValue("type");

            _adapter = adapter.utility.createAdapter(sConnectionType, sConnectionString);

            /*
            oAssembly.appServerRootPath = appServerInfo.appServerRootPath;
            oAssembly.appName = appServerInfo.appName;
            oAssembly.loadDll(xDoc);
             */


        }

        private void fillAppSettings(string sKeyAttr, XmlDocument xDoc)
        {

            //checking if xdoc is not available then return
            if (xDoc == null) return;
            XmlNodeList _nodeList = xDoc.SelectNodes("//appConfig/appSettings/appSetting");

            //if appsettings node not found, return;
            if (_nodeList == null) return;


            foreach (XmlNode node in _nodeList)
            {
                string sKey = sKeyAttr + node.getXmlAttributeValue("key");
                if (clnAppSettings.AllKeys.Contains(sKey) == false)
                {
                    clnAppSettings.Set(sKey, node.InnerText);
                }
            }

            if (_appServerInfo != null)
                clnAppSettings.Set("AppServerPath", _appServerInfo.getAppConfigFolder());

        }

        public void setCRUD(string sKeyAttr, XmlDocument xDoc)
        {

            if (xDoc == null) return;

            XmlNodeList xNodeList = xDoc.SelectNodes("//appConfig/cruds/crud");
            if (xNodeList == null) return;

            foreach (XmlNode xNode in xNodeList)
            {

                string sKey = sKeyAttr + xNode.getXmlAttributeValue("name");
                string sTableName = xNode.getXmlAttributeValue("tableName");
                string sViewName = xNode.getXmlAttributeValue("viewName");
                string sPrimaryKeyField = xNode.getXmlAttributeValue("primaryKey");
                bool isIdentity = xNode.getXmlAttributeValue("isIdentity") == "true" ? true : false;
                //
                if (!clnCRUDs.ContainsKey(sKey))
                {
                    var oCRUD = new NTier.CRUD.clsCRUD(_adapter, sTableName, sViewName, sPrimaryKeyField, isIdentity);

                    clnCRUDs.Add(sKey, oCRUD);
                }
                //return oCRUD;
            }
        }

        private void setGetData(string sKeyAttr, XmlDocument xDoc)
        {
            if (xDoc == null) return;
            XmlNodeList xNodeList = xDoc.SelectNodes("//appConfig/requestData[@type='getData']/dt");
            if (xNodeList == null) return;

            foreach (XmlNode xNode in xNodeList)
            {
                addGetData(sKeyAttr, xNode, clnGetData);
            }
        }

        private void setDropDown(string sKeyAttr, XmlDocument xDoc)
        {
            if (xDoc == null) return;
            XmlNodeList xNodeList = xDoc.SelectNodes("//appConfig/requestData[@type='DropDown']/dt");
            if (xNodeList == null) return;

            foreach (XmlNode xNode in xNodeList)
            {
                addGetData(sKeyAttr, xNode, clnDropDown);
            }
        }



        private void addGetData(string sKeyAttr, XmlNode xNode, Dictionary<string, NTier.Request.clsRequestGetDataBase> cln)
        {
            string sType = xNode.getXmlAttributeValue("type");
            string sKey = sKeyAttr + xNode.getXmlAttributeValue("name");

            if (cln.ContainsKey(sKey)) return;

            switch (sType)
            {
                case "view":
                    string sViewName = xNode.getXmlText("view");

                    var obj = new NTier.Request.clsRequestGetData_SimpleView(_adapter);
                    obj.viewName = sViewName;
                    obj.orderBy = xNode.getXmlText("orderby");
                    obj.setTier(this);

                    cln.Add(sKey, obj);

                    break;

                case "other":

                    string sAssemblyName = xNode.getXmlAttributeValue("assemblyName");
                    string sClassPath = xNode.getXmlAttributeValue("classPath");
                    string sFunc = xNode.getXmlAttributeValue("func");

                    var objOther = new NTier.Request.clsRequestGetData_FromAssembly(_adapter);
                    objOther.assemblyName = sAssemblyName;
                    objOther.classPath = sClassPath;
                    objOther.func = sFunc;
                    objOther.setTier(this);

                    cln.Add(sKey, objOther);
                    break;
                case "sql":
                    string sSQL = xNode.getXmlText("sql");

                    var objSQL = new NTier.Request.clsRequestGetData_sql(_adapter);
                    objSQL.sql = sSQL;
                    objSQL.setTier(this);
                    cln.Add(sKey, objSQL);
                    break;
            }
        }

        private void setCMD(string sKeyAttr, XmlDocument xDoc)
        {
            if (xDoc == null) return;

            XmlNodeList xNodeList = xDoc.SelectNodes("//appConfig/requestData[@type='cmd']/cmd");
            foreach (XmlNode xNode in xNodeList)
            {

                string sNodeName = xNode.getXmlAttributeValue("name");
                if (clnCmd.ContainsKey(sNodeName)) continue;

                string sKey = sKeyAttr + sNodeName;
                string sType = xNode.getXmlAttributeValue("type");
                string sCRUDName = xNode.getXmlAttributeValue("crudName");



                switch (sType)
                {
                    case "save":

                        var objSave = new clsRequestCommand_save();
                        objSave.crudName = sCRUDName;
                        setValidationFrom_Node(xNode, objSave.oValidation);
                        objSave.setTier(this);

                        clnCmd.Add(sKey, objSave);
                        break;

                    case "delete":


                        var objDelete = new clsRequestCommand_delete();
                        objDelete.crudName = sCRUDName;
                        setValidationFrom_Node(xNode, objDelete.oValidation);
                        objDelete.setTier(this);

                        clnCmd.Add(sKey, objDelete);
                        break;
                    case "other":
                        string sAssemblyName = xNode.getXmlAttributeValue("assemblyName");
                        string sClassPath = xNode.getXmlAttributeValue("classPath");
                        string sFunc = xNode.getXmlAttributeValue("func");


                        var objother = new clsRequestCommand_other(oAssembly);
                        objother.AssemblyName = sAssemblyName;
                        objother.classPath = sClassPath;
                        objother.func = sFunc;

                        setValidationFrom_Node(xNode, objother.oValidation);

                        objother.setTier(this);

                        clnCmd.Add(sKey, objother);
                        break;

                    case "cmd":
                        string sSQL = xNode.getXmlAttributeValue("assemblyName");
                        var objSQL = new clsRequestCommand_sql();
                        objSQL.sql = sSQL;
                        setValidationFrom_Node(xNode, objSQL.oValidation);
                        objSQL.setTier(this);

                        clnCmd.Add(sKey, objSQL);
                        break;
                }
            }
        }


        private void setSQLReport(string sKeyAttr, XmlDocument xDoc)
        {
            if (xDoc == null) return;

            XmlNodeList xNodeList = xDoc.SelectNodes("//appConfig/requestData[@type='sqlreport']/sqlreport");

            foreach (XmlNode xNode in xNodeList)
            {

                string sKey = sKeyAttr + xNode.getXmlAttributeValue("name");
                string sRdlPath = xNode.getXmlAttributeValue("rdlPath");
                string downloadName = xNode.getXmlText("downloadName");

                var osqlReport = new clsRequest_sqlReport();
                osqlReport.setTier(this);
                setValidationFrom_Node(xNode, osqlReport.oValidation);
                setSQLReportDs(osqlReport, xNode);
                osqlReport.rdlPath = sRdlPath;
                osqlReport.downloadName = downloadName;

                clnSQLReport.Add(sKey, osqlReport);
            }
        }


        private void setFileData(string sKeyAttr, XmlDocument xDoc)
        {
            if (xDoc == null) return;

            XmlNodeList xNodeList = xDoc.SelectNodes("//appConfig/requestData[@type='file']/file");
            if (xNodeList == null) return;
            foreach (XmlNode xNode in xNodeList)
            {

                string sKey = sKeyAttr + xNode.getXmlAttributeValue("name");
                string sAssemblyName = xNode.getXmlAttributeValue("assemblyName");
                string sClassPath = xNode.getXmlAttributeValue("classPath");
                string sFunc = xNode.getXmlAttributeValue("func");

                var obj = new clsRequestFileData_Assembly(oAssembly);
                obj.setTier(this);

                obj.assemblyName = sAssemblyName;
                obj.classPath = sClassPath;
                obj.func = sFunc;
                clnFileData.Add(sKey, obj);
            }
        }


        private void setSQLReportDs(clsRequest_sqlReport oSQLReport
            , XmlNode xNodeParent)
        {

            var dsNodes = xNodeParent.SelectNodes("ds");

            foreach (XmlNode xNode in dsNodes)
            {

                var sType = xNode.getXmlAttributeValue("type");
                var sName = xNode.getXmlAttributeValue("name");
                var sPath = xNode.getXmlAttributeValue("path");

                DataTable t = null;
                sqlReportTableBase oSQLReportTbl = null;
                switch (sType)
                {
                    case "sql":
                        var objTbl_sql = new sqlReportTableSQL();
                        objTbl_sql.setTier(this);
                        objTbl_sql.name = sName;
                        objTbl_sql.sql = xNode.InnerText;

                        oSQLReport.ds.Add(objTbl_sql);
                        break;
                    case "bll":
                        var objTbl_bll = new sqlReportTableBll();
                        objTbl_bll.setTier(this);
                        objTbl_bll.name = sName;
                        objTbl_bll.path = sPath;
                        oSQLReport.ds.Add(objTbl_bll);
                        break;
                }


            }
        }




        private void setValidationFrom_Node(XmlNode xNode, NTier.Validations.clsValidation oValidation)
        {
            //var oValidation = new NTier.Validations.clsValidation();
            XmlNodeList xNodeValidations = xNode.SelectNodes("validations/validate");

            foreach (XmlNode xnodeValidate in xNodeValidations)
            {
                string sType = xnodeValidate.getXmlAttributeValue("type");

                string sFieldName = xnodeValidate.getXmlAttributeValue("fieldName");
                string sFieldTitle = xnodeValidate.getXmlAttributeValue("fieldTitle");
                bool required = xnodeValidate.getXmlAttributeValue("required") == "true" ? true : false;
                int iMaxLength = g.parseInt(xnodeValidate.getXmlAttributeValue("maxLength"));

                switch (sType)
                {
                    case "basic":
                        oValidation.addTextField(sFieldName, sFieldTitle, iMaxLength);
                        break;
                    case "unique":
                        string sTableName = xnodeValidate.getXmlAttributeValue("tableName");
                        string sPrimaryKey = xnodeValidate.getXmlAttributeValue("primaryKey");
                        //Validation delete 
                        oValidation.addDuplicate(_adapter, sTableName, sPrimaryKey, sFieldName, sFieldTitle, required);
                        break;
                    case "drp":
                        oValidation.addDropDownField(sFieldName, sFieldTitle);
                        break;
                    case "email":
                        oValidation.addEmailField(sFieldName, sFieldTitle, required);
                        break;
                    case "numeric":
                        oValidation.addNumberField(sFieldName, sFieldTitle, required, true, iMaxLength);
                        break;
                    case "check":
                        string checkContraintValues = xnodeValidate.getXmlAttributeValue("values");
                        oValidation.addCheckConstraint(sFieldName, sFieldTitle, checkContraintValues);
                        break;
                }

            }
            //return oValidation.validate(cmd);
        }

        public string getAppSetting(string sKey)
        {
            if (sKey == "AppServerPath")
                return _appServerInfo.getAppConfigFolder();

            return clnAppSettings[sKey];
        }


        public CRUD.clsCRUD getCRUD(string sCRUDName)
        {
            return clnCRUDs[sCRUDName];
        }

        public clsMsg getData(string sPath, clsCmd cmd)
        {
            if (!clnGetData.ContainsKey(sPath))
            {
                throw new Exception(string.Format("Path key [{0}]not found !", sPath));
            }
            var obj = clnGetData[sPath];
            return obj.getData(cmd);
        }

        public clsMsg getDropDownData(string sPath, clsCmd cmd)
        {
            if (!clnDropDown.ContainsKey(sPath))
            {
                throw new Exception(string.Format("Path key [{0}]not found !", sPath));
            }


            var obj = clnDropDown[sPath];
            return obj.getData(cmd);
        }

        public clsMsg exec(string sPath, clsCmd cmd)
        {
            if (!clnCmd.ContainsKey(sPath))
            {
                throw new Exception(string.Format("Path key [{0}]not found !", sPath));
            }

            return clnCmd[sPath].exec(cmd);

        }

        public adapter.clsDataAdapterBase getAdapter(string sKey = "")
        {
            return _adapter;
        }

        public NTier.sqlReport.SQLReportBase getSQLReport(string sPath, clsCmd cmd)
        {
            if (!clnSQLReport.ContainsKey(sPath))
            {
                throw new Exception(string.Format("Path key [{0}] not found !", sPath));
            }

            return clnSQLReport[sPath].getReport(cmd);

        }

        public string getPath(string sPath)
        {
            return _appServerInfo.getAppResourcePath(sPath);
        }

        public clsMsg getFileContent(string sPath, clsCmd cmd)
        {
            if (!clnFileData.ContainsKey(sPath))
            {
                throw new Exception(string.Format("Path key [{0}] not found !", sPath));
            }

            return clnFileData[sPath].getFileData(cmd);
        }
    }
}
