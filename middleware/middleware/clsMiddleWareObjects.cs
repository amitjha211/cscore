using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using middleware.db;

namespace middleware
{


    public class clsEnv
    {
        public string key, value;
    }
    internal class clsObjectType
    {
        public string name, assemblyName, className;
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
        public List<clsView> views;
        public List<clsCustomModule> customModules;
    }


}
