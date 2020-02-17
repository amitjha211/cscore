using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using DAL;

namespace winui
{

    public class MyGrid : DataGridView
    {

        public class event_dropDownValueChanged
        {
            public DataRowView rDropDown;
            public DataRowView r;
        }

        private string _spDelete = "";
        private string _sPrimaryKeyFieldName = "";

        public MyGrid()
        {
            this.AllowUserToDeleteRows = false;
        }
        private BindingSource _bindingSource1
        {
            get
            {
                return (BindingSource)this.DataSource;
            }
        }

        private string sActiveField = "";
        private DataRowView _r;
        private ComboBox __drp;
        private ComboBox _drp
        {
            get
            {
                if (__drp == null)
                {
                    __drp = new ComboBox();
                    __drp.Visible = false;
                    __drp.AutoCompleteMode = AutoCompleteMode.Suggest;
                    __drp.AutoCompleteSource = AutoCompleteSource.ListItems;
                    __drp.KeyDown += new KeyEventHandler(__drp_KeyDown);
                    __drp.Validated += new EventHandler(__drp_Validated);
                    this.Controls.Add(__drp);
                }

                return __drp;
            }
        }

        private class FieldInfo
        {
            public DataTable srcTable;
            public string ValueMember = "";
            public string DisplayMember = "";

        }

        private System.Collections.Generic.Dictionary<string, FieldInfo> _cln = new System.Collections.Generic.Dictionary<string, FieldInfo>();

        public void setDropDown(DataTable t)
        {
            setDropDown(t, t.Columns[0].ColumnName, t.Columns[t.Columns.Count - 1].ColumnName);
        }

        
        public void setDropDown(DataTable t, string sDisplayMember, string sValueMember)
        {
            //lock the column

            int i = 0;
            while (i < this.Columns.Count)
            {
                if (this.Columns[i].DataPropertyName.ToUpper() == sDisplayMember.ToUpper())
                {
                    this.Columns[i].ReadOnly = true;
                    break;
                }
                i++;
            }
            //////////////////////////

            var ob = new FieldInfo();

            ob.srcTable = t;
            ob.DisplayMember = sDisplayMember;
            ob.ValueMember = sValueMember;
            _cln.Add(sDisplayMember, ob);
        }

        public void addColumn(string sColumnHeader, string sDataField, int iWidth)
        {
            var col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = sDataField;
            col.HeaderText = sColumnHeader;
            col.Width = iWidth;
            this.Columns.Add(col);
        }



        public void addButtonColumn(string sColumnHeader, string sCommandName, int iWidth)
        {

            var col = new DataGridViewButtonColumn();

            col.Name = sCommandName;
            col.HeaderText = sColumnHeader;
            col.Width = iWidth;
            col.Text = sCommandName;
            this.Columns.Add(col);

        }

        public void addCheckBoxColumn(string sColumnHeader, string sDataField, int iWidth)
        {

            var col = new DataGridViewCheckBoxColumn();
            col.HeaderText = sColumnHeader;
            col.Width = iWidth;
            col.DataPropertyName = sDataField;
            this.Columns.Add(col);
        }

        void __drp_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    _drp.Visible = false;
                    break;
            }
        }

        void __drp_Validated(object sender, EventArgs e)
        {
            setValue();
            _drp.Visible = false;
            this.Focus();

        }

        void _grd_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (_drp.Visible == true)
            {
                _drp.Visible = false;
            }
        }


        void _grd_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == '\r' || e.KeyChar == '\t') return;
            if (this.CurrentCell == null) return;
            string sFieldName = this.Columns[this.CurrentCell.ColumnIndex].DataPropertyName;


            FieldInfo oFieldInfo;
            if (_cln.ContainsKey(sFieldName))
            {
                oFieldInfo = _cln[sFieldName];
            }
            else
            {
                _drp.Visible = false;
                return;
            }



            DataTable t = oFieldInfo.srcTable;

            _drp.DropDownStyle = ComboBoxStyle.DropDown;

            _drp.DataSource = t;
            _drp.DisplayMember = t.Columns[0].ColumnName;
            _drp.ValueMember = t.Columns[t.Columns.Count - 1].ColumnName;
            System.Drawing.Rectangle rect1 = this.GetCellDisplayRectangle(this.CurrentCell.ColumnIndex, this.CurrentCell.RowIndex, false);
            _drp.Left = rect1.X;
            _drp.Top = rect1.Y;
            _drp.Width = rect1.Width;
            _drp.Height = rect1.Height;

            _drp.Visible = true;
            _drp.Focus();
            SendKeys.Send(e.KeyChar.ToString());

            _r = (DataRowView)this.Rows[this.CurrentCell.RowIndex].DataBoundItem;
            sActiveField = sFieldName;

        }

        void _grd_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (this.CurrentCell.ColumnIndex == (this.Columns.Count - 1) && this.CurrentCell.RowIndex == (this.Rows.Count - 1))
                    {
                        _bindingSource1.AddNew();
                        this.CurrentCell = this.Rows[this.RowCount - 1].Cells[0];
                    }
                    break;
            }

            //TODO: Adjust this
            if (_spDelete != "" && e.Control == true && e.KeyCode == Keys.Delete)
            {
                ui.alert("Underconstruction");
                //g.clsDB.DataGrid_DeleteRow(this, _spDelete, _sPrimaryKeyFieldName);
            }
        }

        /*
        public void setDelete(Tier2.DataTier2 tier1, string sDeleteColName = "")
        {

            if (!string.IsNullOrWhiteSpace(sDeleteColName))
            {
                this.CellContentClick += delegate(object sender, DataGridViewCellEventArgs e)
                {
                    if (this.Columns[e.ColumnIndex].Name.ToLower() == sDeleteColName.ToLower())
                    {
                        deleteRow(tier1);
                    }
                };
            }


            this.KeyDown += delegate(object sender, KeyEventArgs e)
            {

                if (e.Control == true && e.KeyCode == Keys.Delete)
                {

                    deleteRow(tier1);
                }

            };

        }
        */

        //public void deleteRow(Tier2.DataTier2 tier1)
        //{
        //    var grd = this.DataSource as BindingSource;

        //    if (grd.Current == null)
        //    {
        //        ui.alert("Please selec the row and try !");
        //    }



        //    if (ui.confirm("Are you sure want to delete selected Row !"))
        //    {
        //        var r = grd.Current as DataRowView;


        //        if (g.isNull(r[tier1.PrimaryKey], 0).ToString() == "0")
        //        {
        //            grd.RemoveCurrent();
        //        }
        //        else
        //        {
        //            string sMsg = tier1.delete(Convert.ToInt32(r[tier1.PrimaryKey]));
        //            if (string.IsNullOrWhiteSpace(sMsg))
        //            {
        //                grd.RemoveCurrent();
        //                ui.alert("Selected row deleted successfully ...................[Done]");
        //            }
        //            else
        //                ui.alert(sMsg);
        //        }


        //    }
        //}

        public Action<object, event_dropDownValueChanged> onDropDownValueChanged;

        private void setValue()
        {
            if (_r == null) return;
            if (_drp.SelectedIndex == -1) return;
            _r[_cln[sActiveField].DisplayMember] = _drp.Text;
            _r[_cln[sActiveField].ValueMember] = _drp.SelectedValue;

            if (onDropDownValueChanged != null)
                onDropDownValueChanged(this, new event_dropDownValueChanged() { rDropDown = (DataRowView)_drp.SelectedItem, r = _r });
        }

        //TODO : Adjust this
        public bool save(string spInsert, string spUpdate, string sPrimaryKeyFieldName)
        {

            //_bindingSource1.EndEdit();
            //g.clsDB.UpdateTable((DataTable)_bindingSource1.DataSource, spUpdate, spInsert, sPrimaryKeyFieldName);
            ui.alert("underconstruction");
            return true;
        }

        public void fillGrid(DataTable t, bool isAddNewRow = true)
        {
            this.AutoGenerateColumns = false;
            _bindingSource1.DataSource = t;
            if (isAddNewRow && _bindingSource1.Count == 0)
            {
                _bindingSource1.AddNew();
                ///////////////////////////////////////////////////////////
                if (this.Rows.Count > 0)
                {
                    var r = this.Rows[0].DataBoundItem as DataRowView;
                    foreach (DataColumn c in r.DataView.Table.Columns)
                    {
                        if (c.DataType == typeof(int) || c.DataType == typeof(decimal) || c.DataType == typeof(double))
                        {
                            r[c.ColumnName] = 0;
                        }
                    }

                }
            }


            this.DataSource = _bindingSource1;
            this.Refresh();
        }



        //public void fillGrid(string q)
        //{

        //    this.AutoGenerateColumns = false;
        //    _bindingSource1.DataSource = g.clsDB.getTable(q);

        //    if (this.Rows.Count == 0)
        //    {
        //        _bindingSource1.AddNew();
        //    }

        //    this.DataSource = _bindingSource1;

        //}
        public void updateValue(string sFieldName, object oValue)
        {
            updateValue(this, sFieldName, oValue);
        }
        public static void updateValue(DataGridView _grd, string sFieldName, object oValue)
        {
            int i = 0;
            while (i < _grd.Rows.Count)
            {

                DataRowView r = (DataRowView)_grd.Rows[i].DataBoundItem;
                if (r != null)
                {
                    r[sFieldName] = oValue;
                }
                i++;
            }
        }


        //void _grd_KeyDown(object sender, KeyEventArgs e)
        //{
        //    switch (e.KeyCode)
        //    {
        //        case Keys.Enter:
        //            if (_grd.CurrentCell.ColumnIndex == (_grd.Columns.Count - 1) && _grd.CurrentCell.RowIndex == (_grd.Rows.Count - 1))
        //            {
        //                _bindingSource1.AddNew();
        //                _grd.CurrentCell = _grd.Rows[_grd.RowCount - 1].Cells[1];
        //            }
        //            break;
        //    }

        //}

        //public void addColTextBox(string sDataField)
        //{

        //}

        public DataTable getTable()
        {
            _bindingSource1.EndEdit();
            return (DataTable)_bindingSource1.DataSource;
        }

        public object getValue(int iRow, string sField, object NullValue)
        {

            DataRowView r = (DataRowView)this.Rows[iRow].DataBoundItem;

            if (r[sField] == DBNull.Value)
            {
                return NullValue;
            }
            else
            {
                return r[sField];
            }
            //return null;
        }

        public object getValue(int iRow, string sField)
        {

            return getValue(iRow, sField, null);
        }

        public decimal getVal(int iRow, string sField)
        {
            return Convert.ToDecimal(getValue(iRow, sField, 0));
        }



        public int getIntValue(string sField)
        {
            if (this.CurrentRow == null) return 0;

            var r = (this.CurrentRow.DataBoundItem as DataRowView).Row;

            return g.parseInt(r[sField].ToString());

        }

        public void setValue(int iRow, string sField, object value)
        {
            DataRowView r = (DataRowView)this.Rows[iRow].DataBoundItem;
            r[sField] = value;
        }


        public void loadMe()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // MyWindowGrid
            // 
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this._grd_KeyDown);
            this.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this._grd_CellEnter);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this._grd_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }


        public void setDelete(string spDelete, string sPrimaryKeyFieldName)
        {
            _sPrimaryKeyFieldName = sPrimaryKeyFieldName;
            _spDelete = spDelete;
        }

    }
}
