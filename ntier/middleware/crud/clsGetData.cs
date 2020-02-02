using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace middleware.crud
{
    internal class clsGetData : clsAPIRequest
    {
        public string sql { get; set; }
        public override clsAPIResponse call(clsCmd cmd)
        {
            var _adapter = _appService.getAdapter();

             DataTable t =  _adapter.getData(sql);

             return new clsAPIResponse() { result = t };
            
        }
    }
}
