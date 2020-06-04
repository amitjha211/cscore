using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace middleware.Validations
{
    public class vBase : clsAPIRequest
    {
        public override clsAPIResponse call(clsCmd cmd)
        {
            return clsAPIResponse.ok();
        }
    }

    public class vField : vBase
    {
        public string field, type, title;
        public int size;
        public bool required;

        public override clsAPIResponse call(clsCmd cmd)
        {

           

            StringBuilder sbMsg = new StringBuilder();

            string sVal = cmd.getStringValue(field);

            if (required && sVal.isEmpty())
            {
                sbMsg.AppendFormat("Field {0} can't be empty !", this.title);
                return clsAPIResponse.get(sbMsg.ToString(), null);
            }

            if (type == "number" && !g.isNumeric(sVal))
            {
                sbMsg.Clear();
                sbMsg.AppendFormat("The value [{0}] specified for field [{1}] is invalid, only accepts numeric value.", field, sVal);
                return clsAPIResponse.get(sbMsg.ToString());
            }

            if ((type == "text" || type.isEmpty()) && size > 0 && sVal.Length > size)
            {

                sbMsg.Clear();
                sbMsg.AppendFormat("The Max length of field [{0}] is {1}.", field, size);
                return clsAPIResponse.get(sbMsg.ToString());
            }

            return clsAPIResponse.ok();
        }

    }

    public class vUnique : vBase
    {
        

        public class vUniqueCol
        {
            public string field, title;
            public string getfieldTitle()
            {
                return title.isEmpty() ? field : title;
            }
        }
        public string table, idField,name;
        public List<vUniqueCol> fields = new List<vUniqueCol>();

        public void addField(string sField, string sTitle)
        {
            var f = new vUniqueCol();
            f.field = sField;
            f.title = sTitle;
            fields.Add(f);
        }

        public override clsAPIResponse call(clsCmd cmd)
        {

            var _adapter = _appService.getAdapter();

            if (fields.Count > 0)
            {

                int iID = cmd.getIntValue(idField);
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.AppendFormat("select Count(*) from {0} where {1} != @{1}  ", table,idField);

                var cmd2 = new clsCmd();

                List<string> lstValues = new List<string>();
                List<string> lstFieldTitle = new List<string>();

                foreach (var f in fields)
                {
                    sbSQL.AppendFormat(" and {0} = @{0} ", f.field);
                    cmd2.setValue(f.field, cmd[f.field].Value);
                    lstValues.Add(cmd2.getStringValue(f.field));
                    lstFieldTitle.Add(f.getfieldTitle());
                }

                cmd2.setValue(idField, cmd.getIntValue(idField));
                cmd2.SQL = sbSQL.ToString();
                int iCount = g.parseInt(_adapter.execScalar(cmd2));

                if (iCount > 0)
                {

                    if (fields.Count > 1)
                    {
                        return clsAPIResponse.get(string.Format("The Values [{0}] for Fields [{1}] already exists, can't accept duplicate !", string.Join(",", lstValues.ToArray()), string.Join(",", lstFieldTitle.ToArray())));
                    }
                    else if (fields.Count == 1)
                    {

                        return clsAPIResponse.get(string.Format("The Value [{0}] for Field [{1}] already exists, can't accept duplicate value !", lstValues[0], lstFieldTitle[0]));
                    }
                }

            }

            return clsAPIResponse.ok();
        }


    }



}
