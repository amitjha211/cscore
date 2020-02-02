using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace middleware
{

    
    public class clsAPIResponse
    {
        public object result;
        public string message;
    }

    public abstract class clsAPIRequest
    {
        protected iAppService _appService;
        public void setAppService(iAppService appService)
        {
            _appService = appService;
        }
        public string path { get; set;}

        public abstract clsAPIResponse call(clsCmd cmd);

        
        
    }
}
