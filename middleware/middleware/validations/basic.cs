using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace middleware.validations
{
    internal class basic : clsAPIRequest
    {

        public string name {get;set;}
        public string title {get;set;}
        public string dataType {get;set;}
        public override clsAPIResponse call(clsCmd cmd)
        {
            return null;
        }
    }
}
