using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace middleware.db
{

    public class clsTableBase : clsDBObjectBase
    {

        public class clsTableUniqueKey
        {
            public string name;
            public List<string> cols;
        }

        public class clsTableCol
        {
            public string name, title, dataType;
            public int size;
            public bool required;
        }

        public class clsTableRelation
        {
            public string name, table_title, table;
            public List<string> cols;
        }

        public string type, name, view, title;
        public List<string> primaryKeyFields;
        public List<string> displayFields;
        public string autoIncrementField;
        public List<clsTableCol> cols;
        public List<clsTableUniqueKey> UniqueKeys;
        public List<clsTableRelation> Relations;

    }

    public class clsView : clsDBObjectBase
    {
        public string view;

        public clsAPIResponse get(clsCmd cmd)
        {

            var _adapter = _appService.getAdapter();
            var oView = new NTier.Request.clsGetDataView(_adapter);
            oView.viewName = this.view;

            DataTable t = oView.getData(cmd);

            return clsAPIResponse.ok(t);
        }
    }

    public class clsTable : clsTableBase
    {


        public clsAPIResponse get(clsCmd cmd)
        {

            var _adapter = _appService.getAdapter();
            var oView = new NTier.Request.clsGetDataView(_adapter);
            oView.viewName = this.view;

            DataTable t = oView.getData(cmd);

            return clsAPIResponse.ok(t);
        }

        public clsAPIResponse drp(clsCmd cmd)
        {
            var _adapter = _appService.getAdapter();
            var oView = new NTier.Request.clsGetDataView(_adapter);
            oView.viewName = this.view;

            string sDisplayField = this.displayFields[0];
            string sIDField = this.primaryKeyFields[0];


            DataTable t = oView.getDataByColumns(cmd, sDisplayField, sIDField);

            return clsAPIResponse.ok(t);
        }

        //public clsAPIResponse drp_old(clsCmd cmd)
        //{
        //    var _adapter = _appService.getAdapter();



        //    string sDisplayField = this.displayFields[0];
        //    string sIDField = this.primaryKeyFields[0];

        //    string q = string.Format("select {0},{1} from {2}", sDisplayField, sIDField, this.name);

        //    DataTable t = _adapter.getData(q);

        //    return clsAPIResponse.ok(t);
        //}


        public clsAPIResponse validation_delete(clsCmd cmd)
        {
            
            NTier.adapter.clsDataAdapterBase _adapter = _appService.getAdapter();

            if (this.Relations == null) return clsAPIResponse.ok();
            foreach (clsTableRelation relation in this.Relations)
            {

                var cmd2 = new clsCmd();
                StringBuilder sbSQL = new StringBuilder();

                sbSQL.AppendFormat("select count(*) from {0}", relation.table);

                for (int iField = 0; iField < this.primaryKeyFields.Count; iField++)
                {
                    cmd2.setValue(relation.cols[iField], cmd.getStringValue(this.primaryKeyFields[iField]));
                }

                cmd2.SQL = NTier.sqlbuilder.sqlUtility.joinWhereCondition(sbSQL.ToString(), cmd2);

                long iCount = Convert.ToInt64( _adapter.execScalar(cmd2));

                if (iCount > 0)
                {
                    return clsAPIResponse.get("This record can't be deleted, this record has reference in table ["+ relation.table_title +"]");
                }
            }


            return clsAPIResponse.ok();
        }

        public clsAPIResponse save(clsCmd cmd)
        {
            var oCRUD = new NTier.CRUD.clsCRUD(_appService.getAdapter(), name, name, autoIncrementField, true);
            var oResult = oCRUD.save(cmd);
            return new clsAPIResponse() { message = oResult.Message ,result = oResult.Obj};
        }

        public clsAPIResponse delete(clsCmd cmd)
        {


            var response = validation_delete(cmd);
            if (response.isValid == false) return response;

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
            return clsAPIResponse.get("not implemented");
            //var oCRUD = new NTier.CRUD.clsCRUD(_appService.getAdapter(), name, name, autoIncrementField, true);
            //try
            //{
            //    oCRUD.delete(cmd);
            //    return new clsAPIResponse() { message = "" };
            //}
            //catch (Exception ex)
            //{
            //    return new clsAPIResponse() { message = ex.Message };
            //}
        }
    }

}
