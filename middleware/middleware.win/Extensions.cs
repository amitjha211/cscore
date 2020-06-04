using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace middleware.win
{
    public static class Extensions
    {
        
        public static void addColumn(this  DataGridView grd, string sColumnHeader, string sDataField, int iWidth)
        {
            var col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = sDataField;
            col.HeaderText = sColumnHeader;
            col.Width = iWidth;
            grd.Columns.Add(col);
        }

        public static DataTable getDataTable(this DataGridView grd)
        {

            if (grd.DataSource is BindingSource)
            {
                return (grd.DataSource as BindingSource).DataSource as DataTable;
            }
            else
                return grd.DataSource as DataTable;
        }


        public static void setFilter(this  DataGridView grd
            , TextBox txt
            , ComboBox drp
            , Button btnOK = null)
        {

            BindingSource _bindingSource = null;

            if (grd.DataSource is BindingSource)
            {
                _bindingSource = grd.DataSource as BindingSource;
            }


            DataTable tDrp = new DataTable();

            tDrp.Columns.Add("title", typeof(string));
            tDrp.Columns.Add("field", typeof(string));

            foreach (DataGridViewColumn gridcol in grd.Columns)
            {

                var r = tDrp.NewRow();

                r["title"] = gridcol.HeaderText;
                r["field"] = gridcol.DataPropertyName;

                tDrp.Rows.Add(r);
            }

            drp.ValueMember = "field";
            drp.DisplayMember = "title";
            drp.DataSource = tDrp;
            drp.SelectedIndex = 0;

            txt.TextChanged += delegate(object sender, EventArgs e) {

                string sField = drp.SelectedValue.ToString();
                string sValue = txt.Text;

                if (sField.isEmpty()) return;

                if (txt.Text.isEmpty())
                {
                    _bindingSource.Filter = "";
                }
                else
                    _bindingSource.Filter = string.Format("{0} Like '{1}%'", drp.SelectedValue.ToString(), txt.Text);
            };
        }


    }
}
