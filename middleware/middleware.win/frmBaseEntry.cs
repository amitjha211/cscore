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
        
        public void fillDropDown(string sDataPath
            , ComboBox drp1)
        {


            var _response = appService.call(sDataPath, new clsCmd());
            if (_response.isValid)
            {

                DataTable t = _response.result as DataTable;

                drp1.DisplayMember = t.Columns[0].ColumnName;
                drp1.ValueMember = t.Columns[t.Columns.Count - 1].ColumnName;
                drp1.DataSource = t;
            }
        }

        private void frmBase_Load(object sender, EventArgs e)
        {

        }


    }
}
