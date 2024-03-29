﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DAL;

namespace NTier.sqlbuilder
{
    public class sqlUtility
    {

        public static void addFilterFromJsonString(clsCmd cmd)
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


        public static string getWhereCondition(clsCmd prms, params string[] sIgnoreFields)
        {
            if (prms == null) return "";
            List<string> lst = new List<string>();

            foreach (var f in prms)
            {
                if (f.Name.Contains(sIgnoreFields) == false)
                {
                    string sOperator = string.IsNullOrWhiteSpace(f.Operator) ? "=" : f.Operator;
                    string sVal = f.Name;
                    sOperator = sOperator.Replace("_", " ");

                    switch (sOperator.ToUpper())
                    {
                        case "NOT LIKE":
                        case "LIKE":
                            sVal = "'%' + @" + f.Name + " + '%'";
                            break;
                        case "BETWEEN":
                            sVal = string.Format(" @{0} AND @{0}2 ", f.Name);
                            break;
                        default:
                            sVal = "@" + f.Name;
                            break;
                    }

                    string sField = string.IsNullOrWhiteSpace(f.TableName) ? f.Name : f.TableName + "." + f.Name;
                    lst.Add(string.Format(" ( {0} {1} {2})", sField, sOperator, sVal));
                    

                }
            }

            return string.Join(" AND ", lst.ToArray());
        }
        public static string joinWhereCondition(string sSelectSQL, clsCmd cmd, params string[] sIgnoreFields)
        {
            string sWhere = getWhereCondition(cmd, sIgnoreFields);

            if (string.IsNullOrWhiteSpace(sWhere))
                return sSelectSQL;
            else
                if (sSelectSQL.Contains("where", "WHERE"))
                    return sSelectSQL + "  AND " + sWhere;
                else
                    return sSelectSQL + " WHERE " + sWhere;
        }

    }
}
