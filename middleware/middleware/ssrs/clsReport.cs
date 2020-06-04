using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace middleware.ssrs
{
    public class clsReport : db.clsDBObjectBase
    {

        public class clsReportDataSet
        {
            public string name, type, sql;
        }

        public string name, reportPath;

        List<clsReportDataSet> ds;


    }
}
