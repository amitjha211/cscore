﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using NTier.sqlbuilder;
using NTier.adapter;
using System.Data;
namespace NTier.CRUD
{
    public class clsCRUD : sqlCRUD
    {
        clsDataAdapterBase _adapter = null;
        public clsCRUD(clsDataAdapterBase adapter
            , string sTableName
            , string sViewName
            , string sPrimaryKey
            , bool blnIsIdentity)
            : base(sTableName, sViewName, sPrimaryKey, blnIsIdentity, adapter.databaseType)
        {
            _adapter = adapter;
            databaseType = _adapter.databaseType;
        }


        private clsCmd getCmdFromRow(DataRow r)
        {
            var cmd = new clsCmd();

            foreach (DataColumn col in r.Table.Columns)
            {

                cmd.setValue(col.ColumnName, r[col.ColumnName]);
            }

            return cmd;
        }

        public void updateTable(DataTable t)
        {
            var tTable = _adapter.getData("select top 0 * from " + TableName);

            foreach (DataRow r in t.Rows)
            {
                var cmd2 = getCmdFromRow(r);

                cmd2 = getSaveCommand(tTable, cmd2);
                _adapter.exec(cmd2);
            }
        }




        DataTable tEmpty = null;

        private DataTable getEmptyTable()
        {


            if (tEmpty != null) return tEmpty;
            StringBuilder sb1 = new StringBuilder();
            if (_adapter.databaseType == "sqlite")
                sb1.AppendLine("select * from " + TableName + " limit 0");
            else
                sb1.AppendLine("select top 0 * from " + TableName);


            tEmpty = _adapter.getData(sb1.ToString());

            return tEmpty;
        }

        public void bulkUpload(DataTable t)
        {

            string q = getInsertCommand2(t);

            foreach (DataRow r in t.Rows)
            {
                clsCmd cmd2 = new clsCmd();
                cmd2.SQL = q;
                cmd2.AddValues(r);
                _adapter.exec(cmd2);
            }
        }

        public clsMsg save(clsCmd cmd)
        {

            string sSaveFlag = cmd.getIntValue(PrimaryKeyField) == 0 ? "insert" : "update";

            var t = getEmptyTable();
            var cmd2 = getSaveCommand(t, cmd);

            try
            {
                object obj = null;
                switch (_adapter.databaseType)
                {
                    case "mssql":
                        obj = _adapter.execScalar(cmd2);
                        break;
                    case "sqlite":
                        _adapter.exec(cmd2);

                        if (sSaveFlag == "insert")
                            obj = _adapter.execScalar("SELECT last_insert_rowid()");
                        else
                            obj = cmd.getIntValue(PrimaryKeyField);

                        break;
                }


                return g.msg("", obj);
            }
            catch (Exception ex)
            {
                return g.msg_exception(ex);
            }

        }

        public void delete(clsCmd cmd)
        {
            var iID = cmd.getIntValue("id");
            string q = "delete from " + TableName + " where " + " " + PrimaryKeyField + " = " + iID;
            _adapter.exec(q);
        }

        public clsMsg getData(clsCmd cmd)
        {
            var oViewData = new NTier.Request.clsGetDataView(this._adapter);
            oViewData.viewName = this.ViewName;
           
            try
            {
                DataTable t = oViewData.getData(cmd);
                return g.msg("", t);
            }
            catch (Exception ex)
            {
                return g.msg_exception(ex);
            }

        }

    }
}
