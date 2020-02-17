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
        public bool isValid
        {
            get { return this.message.isEmpty() ? true : false; }
        }

        public static clsAPIResponse get(string msg, object obj = null)
        {
            return new clsAPIResponse() { message = msg, result = obj };
        }
        public static clsAPIResponse ok( object obj = null)
        {
            return new clsAPIResponse() { message = "", result = obj };
        }
    }

    public interface iAPIRequestBase
    {
        void setAppService(iAppService appService);
    }

    public interface iAPIRequest : iAPIRequestBase
    {
        void setAppService(iAppService appService);
        clsAPIResponse call(clsCmd cmd);
    }
    
    public abstract class clsAPIRequest : iAPIRequest
    {
        protected iAppService _appService;
        public void setAppService(iAppService appService)
        {
            _appService = appService;
        }
        public string path { get; set; }
        public abstract clsAPIResponse call(clsCmd cmd);
    }
}
