using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using middleware.db;

namespace middleware
{

    public interface iAppService
    {
        string getAppSetting(string skey);
        NTier.adapter.clsDataAdapterBase getAdapter(string sConnectionName = "main");
        clsAPIResponse call(string sPath, clsCmd cmd);
    }

    public class clsAppService_old : iAppService
    {

        public List<clsAPIRequest> lstRequest = new List<clsAPIRequest>();
        clsJSONParser oJSONParser = new middleware.clsJSONParser();

        clsMiddleWareObjects middleObjects ;

        List<clsValidation> validations = new List<clsValidation>();

        public void includeFiles(List<string> lstPath)
        {

            foreach (string sPath in lstPath)
            {
                
                string sJSON = System.IO.File.ReadAllText(sPath);
                var _middleObjects = oJSONParser.getJSONObject(sJSON,typeof(clsMiddleWareObjects)) as clsMiddleWareObjects;

                if (middleObjects == null)
                {
                    middleObjects = _middleObjects;
                    return;
                }


                

                foreach (var obj in _middleObjects.dbConnections)
                {
                    middleObjects.dbConnections.Add(obj);
                }


                foreach (var obj in _middleObjects.objectTypes)
                {
                    middleObjects.objectTypes.Add(obj);
                }


                foreach (var obj in _middleObjects.tables)
                {
                    middleObjects.tables.Add(obj);
                }

            }
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

                var sTable = f.view.isEmpty() ? f.name : f.view;
                objGet.sql = "select * from " + sTable + " where 1=1";
                objGet.path = f.name + "/get";

                lstRequest.Add(objGet);
                ////////////////////////////////////////////////////////

                //SAVE
                var objSave = new crud.clsTableSave();
                objSave.setAppService(this);

                objSave.Table = f.name;

                if (f.primaryKeyFields != null && f.primaryKeyFields.Count > 0)
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
                if (f.primaryKeyFields != null && f.primaryKeyFields.Count > 0)
                {
                    objDelete.idField = f.primaryKeyFields[0];
                    if (objDelete.idField == f.autoIncrementField) objDelete.isIdentity = true;
                }
                objDelete.path = f.name + "/delete";
                lstRequest.Add(objDelete);
                //////////////////////////////////////////////////////////////
                //DRP

                if (f.displayFields.Count > 0 && f.primaryKeyFields.Count > 0)
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
