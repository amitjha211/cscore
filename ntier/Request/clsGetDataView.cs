using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DAL;
namespace NTier.Request
{
    public class clsGetDataView
    {
        public string viewName { get; set; }
        public string OrderBy { get; set; }
        NTier.adapter.clsDataAdapterBase _adapter;

        public clsGetDataView(NTier.adapter.clsDataAdapterBase adapter)
        {
            _adapter = adapter;
        }

        private void add_json_filter(clsCmd cmd)
        {
            if (cmd.ContainFields("_filter"))
            {
                string sFilterJson = cmd.getStringValue("_filter");

                var tFilter = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(sFilterJson);
                foreach (DataRow rFilter in tFilter.Rows)
                {
                    if (rFilter["val"].ToString().isEmpty() == false && rFilter["name"].ToString().isEmpty() == false)
                    {
                        var prm = cmd.setValue(rFilter["name"].ToString(), rFilter["val"].ToString());
                        prm.Operator = rFilter["operator"].ToString();
                    }
                }

                cmd.Remove(cmd["_filter"]);
            }
        }

        public DataTable getDataByColumns(clsCmd cmd, params string[] sColumns)
        {


            StringBuilder sbSQL = new StringBuilder();

            sbSQL.Append("select ");
            sbSQL.Append(string.Join(",", sColumns));
            sbSQL.Append(" from " + viewName + " where 1=1 ");

            string q = sbSQL.ToString();

            add_json_filter(cmd);

            cmd.SQL = NTier.sqlbuilder.sqlUtility.joinWhereCondition(q, cmd);

            if (!OrderBy.isEmpty()) cmd.SQL += " order by " + OrderBy;

            var t = _adapter.getData(cmd);

            return t;
        }

        public DataTable getData(clsCmd cmd)
        {



            string q = "select * from " + viewName + " where 1=1 ";
            cmd.SQL = NTier.sqlbuilder.sqlUtility.joinWhereCondition(q, cmd);

            add_json_filter(cmd);

            if (!OrderBy.isEmpty()) cmd.SQL += " order by " + OrderBy;

            var t = _adapter.getData(cmd);
            return t;
        }
    }
}
