using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace middleware.crud
{

    class clsTableDelete : clsAPIRequest
    {
        public string Table { get; set; }
        public string idField { get; set; }
        public bool isIdentity { get; set; }

        public override clsAPIResponse call(clsCmd cmd)
        {
            var oCRUD = new NTier.CRUD.clsCRUD(_appService.getAdapter(), Table, Table, idField, isIdentity);

            try
            {
                oCRUD.delete(cmd);
                return new clsAPIResponse() { message = "" };
            }
            catch (Exception ex)
            {
                return new clsAPIResponse() { message = ex.Message };
            }

        }
    }
}
