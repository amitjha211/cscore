using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace middleware
{

    internal class clsObjectType
    {
        public string name, assemblyName, className;
    }

    internal class clsConnectionInfo
    {
        public string name,type,value;
    }

    internal class clsColInfo
    {
        string name, type;
        int size;
        bool required;
    }

    internal class clsTable
    {
        public string type, name,viewName;
        public string[] primaryKeyFields;
        public string[] displayFields;
        public string autoIncrementField;
        List<clsColInfo> cols;
    }


    internal class clsValidation
    {
        public List<object> list;
        public string name { get; set; }
    }

    internal class clsMiddleWareObjects
    {
        public List<clsObjectType> objectTypes;
        public List<clsConnectionInfo> dbConnections;
        public List<clsTable> tables;
    }


}
