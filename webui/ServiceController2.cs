using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace webui
{
    public class ServiceController2 : ServiceControllerBase
    {
        public override string AppName
        {
            get { return Request.QueryString["appName"]; }
        }
        public override string AppServerRootPath
        {
            get { return ui.appServicePath + "\\AppServer"; }
        }
    }
}
