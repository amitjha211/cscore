using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace middleware.db
{


    public interface iDBObjectBase
    {
        void setAppService(iAppService appService);
    }

    internal class clsDBObjectBase : iDBObjectBase
    { 
        protected iAppService _appService;
        public void setAppService(iAppService appService)
        {
            _appService = appService;
        }
    }

    internal class clsConnectionInfo
    {
        public string name, type, value;
    }

    internal class clsTableUniqueKey
    {
        public string name;
        public List<string> cols;
    }
    internal class clsTableCol
    {
        public string name, title, dataType;
        public int size;
        public bool required;
    }

    internal class clsTableRelation
    {
        public string name, primaryTable, foreignTable;
        public List<string> primaryFields;
        public List<string> foreignFields;
    }

    internal class clsTable : clsDBObjectBase
    {
        public string type, name, view, title;
        public List<string> primaryKeyFields;
        public List<string> displayFields;
        public string autoIncrementField;
        public List<clsTableCol> cols;
        public List<clsTableUniqueKey> UniqueKeys;

        public  clsAPIResponse get(clsCmd cmd)
        {
            var _adapter = _appService.getAdapter();

            DataTable t = _adapter.getData("select * from " + view);

            return new clsAPIResponse() { result = t };
        }

        public clsAPIResponse drp(clsCmd cmd)
        {
            var _adapter = _appService.getAdapter();

            string sDisplayField = this.displayFields[0];
            string sIDField = this.primaryKeyFields[0];

            string q = string.Format("select {0},{1} from {2}", sDisplayField, sIDField, this.name);

            DataTable t = _adapter.getData(q);

            return clsAPIResponse.ok(t);
        }
        public clsAPIResponse save(clsCmd cmd)
        {
            var oCRUD = new NTier.CRUD.clsCRUD(_appService.getAdapter(), name, name, autoIncrementField, true);
            var oResult = oCRUD.save(cmd);
            return new clsAPIResponse() { message = oResult.Message };
        }

        public clsAPIResponse delete(clsCmd cmd)
        {
            var oCRUD = new NTier.CRUD.clsCRUD(_appService.getAdapter(), name, name, autoIncrementField, true);

            try
            {
                oCRUD.delete(cmd);
                return clsAPIResponse.ok();
            }
            catch (Exception ex)
            {
                return new clsAPIResponse() { message = ex.Message };
            }
        }

        public clsAPIResponse call(clsCmd cmd)
        {
            var oCRUD = new NTier.CRUD.clsCRUD(_appService.getAdapter(), name, name, autoIncrementField, true);

            try
            {
                oCRUD.delete(cmd);
                return new clsAPIResponse() { message = "" };
            }
            catch (Exception ex)
            {
                return new clsAPIResponse() { message = ex.Message };
            }

        }
    }


}
