using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace middleware
{

    public interface iAppService
    {
        string getAppSetting(string skey);
        NTier.adapter.clsDataAdapterBase getAdapter(string sConnectionName = "main");

        clsAPIResponse call(string sPath, clsCmd cmd);

    }

    public class clsAppService : iAppService
    {

        public List<clsAPIRequest> lstRequest = new List<clsAPIRequest>();
        clsMiddleWareObjects middleObjects;

        List<clsValidation> validations = new List<clsValidation>();

        public clsAppService(string sPath)
        {
            
            string sJSON = System.IO.File.ReadAllText(sPath);
            middleObjects = NTier.myAssembly2.getObjectFromJSONString<middleware.clsMiddleWareObjects>(sJSON);

            var obj = Newtonsoft.Json.Linq.JObject.Parse(sJSON);


        }

        public NTier.adapter.clsDataAdapterBase getAdapter(string sConnectionName = "main")
        {
            clsConnectionInfo conInfo = middleObjects.dbConnections.Find(p => p.name == sConnectionName);
            if (conInfo != null)
            {
                return NTier.adapter.utility.createAdapter(conInfo.type, conInfo.value);
            }
            else
                return null;
        }



        public void compile()
        {
            foreach (var f in middleObjects.tables)
            {

                //GET
                var objGet = new crud.clsGetData();
                objGet.setAppService(this);

                var sTable = f.viewName.isEmpty() ? f.name : f.viewName;
                objGet.sql = "select * from " + sTable + " where 1=1";
                objGet.path = f.name + "/get";

                lstRequest.Add(objGet);
                ////////////////////////////////////////////////////////

                //SAVE
                var objSave = new crud.clsTableSave();
                objSave.setAppService(this);

                objSave.Table = f.name;

                if (f.primaryKeyFields != null && f.primaryKeyFields.Length > 0)
                {
                    objSave.idField = f.primaryKeyFields[0];
                    if (objSave.idField == f.autoIncrementField) objSave.isIdentity = true;
                }
                objSave.path = f.name + "/save";
                lstRequest.Add(objSave);
                //////////////////////////////////////////////////////////////
                //delete
                var objDelete = new crud.clsTableDelete();
                objDelete.setAppService(this);
                objDelete.Table = f.name;
                if (f.primaryKeyFields != null && f.primaryKeyFields.Length > 0)
                {
                    objDelete.idField = f.primaryKeyFields[0];
                    if (objDelete.idField == f.autoIncrementField) objDelete.isIdentity = true;
                }
                objDelete.path = f.name + "/delete";
                lstRequest.Add(objDelete);
                //////////////////////////////////////////////////////////////
                //DRP

                if (f.displayFields.Length > 0 && f.primaryKeyFields.Length > 0)
                {
                    string sDisplayField, sIDField;

                    sDisplayField = f.displayFields[0];
                    sIDField = f.primaryKeyFields[0];

                    string q = string.Format("select {0},{1} from {2}", sDisplayField, sIDField, f.name);
                    var objDrp = new crud.clsGetData();
                    objDrp.setAppService(this);
                    objDrp.sql = q;
                    objDrp.path = "drp/" + f.name;

                    lstRequest.Add(objDrp);
                }
                //////////////////////////////////////////////////////////////

            }
        }

        public clsAPIResponse call(string sPath, clsCmd cmd)
        {
            var obj = lstRequest.Find(p => p.path == sPath);
            if (obj == null) return new clsAPIResponse() { message = string.Format("path [{0}] not found !", sPath) };
            return obj.call(cmd);
        }

        public string getAppSetting(string skey)
        {
            return "not implemented";
        }
    }

}
