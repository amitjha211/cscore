using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace winui.appConfig
{
    public partial class frmGetData : Form
    {
        public frmGetData()
        {
            InitializeComponent();
        }

        private void frmGetData_Load(object sender, EventArgs e)
        {

        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            FillData();

        }

        private void FillData()
        {
            try
            {

                var result = ui.oTier.getData(textBox1.Text, new clsCmd());
                if (result.Validated)
                {
                    var t = result.Obj as DataTable;
                    dataGridView1.DataSource = t;

                }
                else
                    ui.warn(result.Message);


            }
            catch (Exception ex)
            {
                ui.warn(ex.Message);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FillData();
            }
        }
    }
}
