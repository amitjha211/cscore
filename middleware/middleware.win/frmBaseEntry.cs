using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace middleware.win
{

    public partial class frmBaseEntry : frmBase
    {
        public frmBaseEntry()
        {
            InitializeComponent();
        }


        public Action<string, object> formCommand;

        public void invoke_command(string sCommand, object obj)
        {
            if (formCommand != null) formCommand(sCommand, obj);

        }
        protected object callingObject;

        public void fillDropDown(ListControl drp1, string sDataPath, string sDisplayMember = "", string sValueMember = "")
        {
            fillDropDown(drp1, sDataPath, new clsCmd(), sDisplayMember, sValueMember);
        }
        public void fillDropDown(ListControl drp1, string sDataPath, clsCmd cmd, string sDisplayMember = "", string sValueMember = "")
        {


            var _response = appService.call(sDataPath, cmd);
            if (_response.isValid)
            {

                DataTable t = _response.result as DataTable;

                drp1.DisplayMember = sDisplayMember.isEmpty() ? t.Columns[0].ColumnName : sDisplayMember;
                drp1.ValueMember = sValueMember.isEmpty() ? t.Columns[t.Columns.Count - 1].ColumnName : sValueMember;
                drp1.DataSource = t;
            }
        }



        protected void setGridDelete(System.Windows.Forms.DataGridView grd, string sDeleteCommandPath, string sPrimaryKeyField)
        {

            grd.KeyDown += delegate(object sender, KeyEventArgs e)
            {

                if (e.Control == true && e.KeyCode == Keys.Delete)
                {

                    deleteRow(grd, sDeleteCommandPath, sPrimaryKeyField);
                }

            };

        }


        public void deleteRow(System.Windows.Forms.DataGridView grd1, string sDeleteCommand, string sPrimaryKeyField)
        {
            //var oCRUD = _Tier.getCRUD(sCRUD);
            var grd = grd1.DataSource as BindingSource;

            if (grd.Current == null)
            {
                ui.alert("Please selec the row and try !");
            }


            if (ui.confirm("Are you sure want to delete selected Row !"))
            {
                var r = grd.Current as DataRowView;
                int iID = g.parseInt(r[sPrimaryKeyField]);

                if (iID == 0)
                {
                    grd.RemoveCurrent();
                }
                else
                {
                    //string sMsg = tier1.delete(Convert.ToInt32(r[tier1.PrimaryKey]));


                    var cmd = new clsCmd();
                    cmd.setValue(sPrimaryKeyField, iID);
                    var msg = appService.call(sDeleteCommand, cmd);
                    if (msg.isValid)
                    {
                        grd.RemoveCurrent();
                        ui.alert("Selected row deleted successfully ...................[Done]");
                    }
                    else
                        ui.alert(msg.message);
                }

            }
        }



        private void frmBase_Load(object sender, EventArgs e)
        {

        }


    }
}
