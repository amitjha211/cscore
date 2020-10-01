using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace NTier.adapter
{
    partial class utility
    {

        public static clsDataAdapterBase getSQLLiteAdapter(string sConnectionString)
        {
            var _adapter = new clsSQLiteAdapter(sConnectionString);
            return _adapter;

        }


        public static void setCommand(clsCmd cmd
            , System.Data.SQLite.SQLiteCommand sqlcmd
            , CommandType iCommandType = CommandType.Text)
        {
            foreach (var f in cmd)
            {
                sqlcmd.Parameters.AddWithValue(f.Name, f.Value);
            }

            sqlcmd.CommandType = iCommandType;
            sqlcmd.CommandText = cmd.SQL;
        }




    }
}
