using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyWinControls
{
    internal partial class frmSearchBox : Form
    {
        public frmSearchBox()
        {
            InitializeComponent();
        }

        public DataRow row = null;

        private void frmSearchBox_Load(object sender, EventArgs e)
        {

        }

        public void callSearch(DataTable t, DAL.GridColumns cols)
        {
            foreach (DAL.ColMap col in cols)
            {
                grd.addColumn(col.title, col.datafield, col.width);
            }

            grd.AutoGenerateColumns = false;
            bindingSource1.DataSource = t;
            grd.DataSource = bindingSource1;
            ucGridSearch.setGrid(grd);


        }


        public void callSearch(DataTable t)
        {
            bindingSource1.DataSource = t;
            grd.DataSource = bindingSource1;

            grd.Columns[grd.Columns.Count - 1].Visible = false;
            ucGridSearch.setGrid(grd);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (grd.CurrentRow == null)
            {
                ui.alert("Please select the row !");
                return;
            }

            row = ((DataRowView)grd.CurrentRow.DataBoundItem).Row;

            this.Close();

        }

        private void grd_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void grd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up && grd.CurrentRow != null && grd.CurrentRow.Index == 0)
                ucGridSearch.Focus();

            if (e.KeyCode == Keys.Enter)
                btnOK_Click(null, null);
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void bindingSource1_ListChanged(object sender, ListChangedEventArgs e)
        {
            lblRecordCount.Text = bindingSource1.Count.ToString() ;
        }

        private void grd_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            
        }

        
    }
}
