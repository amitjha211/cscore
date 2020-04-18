using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace middleware.db
{

    public interface iDBObjectBase
    {
        void setAppService(iAppService appService);
    }

    public class clsDBObjectBase : iDBObjectBase
    { 
        protected iAppService _appService;
        public void setAppService(iAppService appService)
        {
            _appService = appService;
        }
    }

    internal class clsConnectionInfo
    {
        public string name, type, value;
    }

    

    


}
