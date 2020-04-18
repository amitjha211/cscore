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

    public partial class frmGridSelectMultiple : frmBaseEntry
    {

        private class clsGridSearch
        {
            DataGridView _grd;
            TextBox _txt;
            ComboBox _drp;
            BindingSource _bindingSource;



            public BindingSource bindingSource
            {
                get
                {
                    return _bindingSource;
                }
            }
            public DataTable getDataTable()
            {
                return _bindingSource.DataSource as DataTable;
            }

            public clsGridSearch(DataGridView grd
                , TextBox txt
                , ComboBox drp
                , Button btnOK = null)
            {

                _grd = grd;
                _txt = txt;
                _drp = drp;

                if (_grd.DataSource is BindingSource)
                {
                    _bindingSource = _grd.DataSource as BindingSource;

                }


                DataTable tDrp = new DataTable();

                tDrp.Columns.Add("title", typeof(string));
                tDrp.Columns.Add("field", typeof(string));

                foreach (DataGridViewColumn gridcol in _grd.Columns)
                {

                    var r = tDrp.NewRow();

                    r["title"] = gridcol.HeaderText;
                    r["field"] = gridcol.DataPropertyName;

                    tDrp.Rows.Add(r);
                }

                _drp.ValueMember = "field";
                _drp.DisplayMember = "title";
                _drp.DataSource = tDrp;

                _drp.SelectedIndex = 0;


                _txt.TextChanged += new EventHandler(_txt_TextChanged);

            }

            void _txt_TextChanged(object sender, EventArgs e)
            {
                string sField = _drp.SelectedValue.ToString();
                string sValue = _txt.Text;

                if (sField.isEmpty()) return;

                if (_txt.Text.isEmpty())
                {
                    _bindingSource.Filter = "";
                }
                else
                    _bindingSource.Filter = string.Format(" {0} Like '{1}%'", _drp.SelectedValue.ToString(), _txt.Text);

            }

        }

        public frmGridSelectMultiple()
        {
            InitializeComponent();
        }

        public clsGrid gridInfo { get; set; }
        private clsGridSearch _grdSrc;

        public void compile()
        {

            if (gridInfo != null)
            {
                this.Text = gridInfo.title;
                //lblTitle.Text = gridInfo.title;
                gridInfo.appService = this.appService;
                gridInfo.formService = this.formService;
                gridInfo.createCols(this.dataGridView1, false);
                gridInfo.reloadGrid(this.dataGridView1);
                _grdSrc = new clsGridSearch(dataGridView1, txtSrc, drpSrcField);

                DataTable t = dataGridView1.getTable();
                

            }



            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            compile();
            
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
            {
                e.SuppressKeyPress = true;

                DataRowView r = dataGridView1.CurrentRow.DataBoundItem as DataRowView;
                
                invoke_command("row_selected", dataGridView1.CurrentRow.DataBoundItem);

            }

            //if (e.KeyCode == Keys.Space)
            //{
            //    invoke_command("row_selected", dataGridView1.CurrentRow.DataBoundItem);
            //}

            if (e.KeyCode == Keys.Up)
            {
                if (dataGridView1.CurrentRow.Index == 0)
                {
                    txtSrc.Focus();
                }
            }
        }

        private void txtSrc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                dataGridView1.Focus();
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }





    }
}
