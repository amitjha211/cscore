using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace middleware.crud
{

    class clsTableSave : clsAPIRequest
    {

        public string Table { get; set; }
        public string idField {get;set;}
        public bool isIdentity { get; set; }

        public override clsAPIResponse call(clsCmd cmd)
        {
            var oCRUD = new NTier.CRUD.clsCRUD(_appService.getAdapter(),Table,Table,idField,isIdentity);
            var oResult  = oCRUD.save(cmd);
            return new clsAPIResponse() { message = oResult.Message };
        }
    }
}
