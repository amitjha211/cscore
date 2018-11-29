using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace winui
{
    public class searchResult
    {

        public string PrimaryKeyField { get; set; }
        clsCmd cmd = new clsCmd();


        public searchResult(DataRow r)
        {

            if (r != null)
            {
                cmd.AddValues(r);
            }
        }

        public bool validated
        {
            get
            {
                return cmd.Count > 0 ? true : false;
            }
        }

        public object getVal(string sField)
        {
            return cmd[sField].Value;
        }

        public int getID()
        {
            if (PrimaryKeyField.isEmpty() == false)
                return cmd.getIntValue(PrimaryKeyField);
            else
                return g.parseInt(cmd[cmd.Count - 1].Value);
        }
    }
}
