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

    public partial class frmGrid1 : frmBaseEntry
    {
        private class clsGridSearch
        {
            DataGridView _grd;
            TextBox _txt;
            ComboBox _drp;
            BindingSource _bindingSource;


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
                    _bindingSource.Filter = string.Format("{0} Like '{1}%'", _drp.SelectedValue.ToString(), _txt.Text);

            }

        }

        public frmGrid1()
        {
            InitializeComponent();
        }

        public clsGrid gridInfo { get; set; }
        private clsGridSearch _grdSrc;

        public void compile()
        {

            if (gridInfo != null)
            {
                gridInfo.appService = this.appService;
                gridInfo.formService = this.formService;

                this.Text = gridInfo.title;
                lblTitle.Text = gridInfo.title;
                gridInfo.bindGrid(this.dataGridView1, new clsCmd());
                _grdSrc = new clsGridSearch(dataGridView1, txtSrc, drpSrcField);
            }


            if (gridInfo.crud_save.isEmpty())
            {
                btnSave.Visible = false;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.ReadOnly = true;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }


            //add buttons
            if (gridInfo.actionButtons != null && gridInfo.actionButtons.Count > 0)
            {
                int iLeft, iTop, iHeight, iWidth;
                iLeft = 2;
                iHeight = 22;
                iWidth = 119;
                iTop = 20;

                for (int iButton = 0; iButton < gridInfo.actionButtons.Count; iButton++)
                {

                    var f = gridInfo.actionButtons[iButton];

                    Button btn = new Button();
                    btn.Name = "btnAction" + iButton;
                    btn.Text = f.title;
                    btn.Tag = f;
                    btn.Left = iLeft;
                    btn.Top = iTop;
                    btn.Height = iHeight;
                    btn.Width = iWidth;
                    btn.Click += new EventHandler(btn_Click);

                    pnlActionButtons.Controls.Add(btn);

                    iTop += iHeight + 2;
                }

            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            compile();
        }

        void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            clsGridActionButton oAction = btn.Tag as clsGridActionButton;

            DataRowView r = dataGridView1.CurrentRow.DataBoundItem as DataRowView;
            formService.openFormEntryDialog(oAction.formKey, r.Row);

            gridInfo.reloadGrid(dataGridView1);

            //clsMiddlewareUI.formService.openForm("", "");
            //ui.warn(btn.Text);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            DataTable t = _grdSrc.getDataTable().GetChanges();

            if (t == null)
            {
                ui.alert("No any changes detected in grid");
                return;
            }

            foreach (DataRow r in t.Rows)
            {
                clsCmd cmd = new clsCmd();
                cmd.AddValues(r);
                var response = appService.call(gridInfo.crud_save, cmd);
                if (response.message.isEmpty() == false)
                {
                    ui.warn(response.message);
                    break;
                }
            }

            gridInfo.reloadGrid(dataGridView1);
            ui.alert("Record Saved successfully.");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                ui.warn("Please select the record !");
                return;
            }

            DataRowView r = dataGridView1.CurrentRow.DataBoundItem as DataRowView;

            var cmd2 = new clsCmd();
            cmd2.AddValues(r.Row);


            if (ui.confirm("Are you sure want to delete selected record"))
            {

                var result = appService.call(gridInfo.crud_delete, cmd2);

                if (result.isValid)
                {
                    gridInfo.reloadGrid(dataGridView1);
                }
                else
                {
                    ui.warn(result.message);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            gridInfo.reloadGrid(dataGridView1);
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {

            DataRow r = _grdSrc.getDataTable().NewRow();
            formService.openFormEntryDialog(gridInfo.editForm, r);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

            if (dataGridView1.CurrentRow == null)
            {
                ui.warn("Please select the record !");
                return;
            }

            DataRowView r = dataGridView1.CurrentRow.DataBoundItem as DataRowView;
            formService.openFormEntryDialog(gridInfo.editForm, r.Row);
        }

    }
}
