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

    public class clsAppService2 : iAppService
    {

        private List<clsCallObjects> lstRequest = new List<clsCallObjects>();
        clsJSONParser oJSONParser = new middleware.clsJSONParser();

        clsMiddleWareObjects middleObjects;

        List<clsValidation> validations = new List<clsValidation>();

        public void includeFiles(List<string> lstPath)
        {

            foreach (string sPath in lstPath)
            {

                string sJSON = System.IO.File.ReadAllText(sPath);
                var _middleObjects = oJSONParser.getJSONObject(sJSON, typeof(clsMiddleWareObjects)) as clsMiddleWareObjects;

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
            compile_table();
            compile_view();
            compile_customModules();
        }


        public void compile_view()
        {
            foreach (var fview in middleObjects.views)
            {
                fview.setAppService(this);
                var objCaller = new clsCallObjects();
                objCaller.Add(fview, "get");
                objCaller.path = fview.view + "/get";
                objCaller.compile();
                lstRequest.Add(objCaller);
            }
        }

        public void compile_table()
        {

            foreach (var fTable in middleObjects.tables)
            {

                fTable.setAppService(this);

                string[] methods = new string[] { "get", "save", "delete", "drp" };

                foreach (string sMethod in methods)
                {
                    var objCaller = new clsCallObjects();
                    objCaller.Add(fTable, sMethod);
                    objCaller.path = fTable.name + "/" + sMethod;

                    if (sMethod == "save" )
                    {
                        if (fTable.cols != null && fTable.cols.Count > 0)
                        {
                            foreach (var fCol in fTable.cols)
                            {
                                var objValidateBasic = new middleware.Validations.vField();

                                objValidateBasic.field = fCol.name;
                                objValidateBasic.title = fCol.title;
                                objValidateBasic.type = fCol.dataType;
                                objValidateBasic.size = fCol.size;
                                objValidateBasic.required = fCol.required;

                                objValidateBasic.setAppService(this);

                                objCaller.Add(objValidateBasic, "call");
                            }
                        }
                        if (fTable.UniqueKeys != null && fTable.UniqueKeys.Count > 0)
                        {

                            foreach (var fUniqueKey in fTable.UniqueKeys)
                            {
                                var objValidateUniqueKey = new middleware.Validations.vUnique();
                                objValidateUniqueKey.setAppService(this);
                                objValidateUniqueKey.name = fUniqueKey.name;
                                objValidateUniqueKey.idField = fTable.autoIncrementField;
                                objValidateUniqueKey.table = fTable.name;

                                foreach (var sField in fUniqueKey.cols)
                                {
                                    string sTitle = sField;
                                    if (fTable.cols != null && fTable.cols.Count > 0)
                                    {
                                        var fcol = fTable.cols.Find(p => p.name.ToUpper() == sField.ToUpper());
                                        if (fcol != null) sTitle = fcol.title;
                                    }

                                    objValidateUniqueKey.addField(sField, sTitle);
                                }
                                objCaller.Add(objValidateUniqueKey, "call");
                            }
                        }
                    }
                    objCaller.compile();
                    lstRequest.Add(objCaller);
                }
            }
        }

        public void compile_customModules()
        {

            foreach (var fcustomModule in middleObjects.customModules)
            {
                
                iAPIRequestBase objCustom = g.createObjectFromAssemblyInfo(fcustomModule.assemblyName,fcustomModule.classPath) as iAPIRequestBase;

                objCustom.setAppService(this);
                foreach (string sMethod in fcustomModule.methods)
                {
                    var objCaller = new clsCallObjects();
                    
                    objCaller.Add(objCustom, sMethod);
                    objCaller.path = fcustomModule.name + "/" + sMethod;
                    objCaller.compile();

                    lstRequest.Add(objCaller);
                }
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
