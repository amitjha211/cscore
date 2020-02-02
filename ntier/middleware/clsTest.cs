using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace middleware
{

    public class clsTest
    {

        public void test1(string sPath)
        {

            var obj = new clsAppService(sPath);
            obj.compile();




            //insert test
            //var cmd = new clsCmd();
            //cmd.setValue("id", "0");
            //cmd.setValue("roleName", "Data Entry");

            //var t = obj.call("sysRole/save", cmd);



            //save test
            //var cmd = new clsCmd();
            //cmd.setValue("id", "3");
            //cmd.setValue("roleName", "aj");

            //var responseSave = obj.call("sysRole/save", cmd);


            //delete test
            var cmd = new clsCmd();
            cmd.setValue("id", "4");
            var responseDelete = obj.call("sysRole/delete", cmd);
            //drp test
            var t = obj.call("drp/sysRole", new clsCmd()).result as DataTable;

        }

    }
}
