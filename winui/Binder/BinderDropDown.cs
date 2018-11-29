using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using DAL;
namespace winui
{

    public class BinderDropDown : BinderFieldBase
    {


        ComboBox _drp;

        private void fillList(DataTable src, ListControl cmb)
        {
            if (string.IsNullOrEmpty(cmb.DisplayMember)) cmb.DisplayMember = src.Columns[0].ColumnName;
            if (string.IsNullOrEmpty(cmb.ValueMember)) cmb.ValueMember = src.Columns[src.Columns.Count - 1].ColumnName;
            cmb.DataSource = src;
        }

        public BinderDropDown(ComboBox drp, string sField, DataTable t)
        {
            _drp = drp;
            //fillList(t, drp);
            this.Field = sField;
        }


        public override void setValue(object val)
        {
            if (_drp.DataSource == null)
                _drp.Text = val.ToString();
            else
                _drp.SelectedValue = val;
        }

        public override object getValue()
        {
            if (_drp.DataSource != null)
                if (_drp.SelectedValue == null)
                    return DBNull.Value;
                else
                    return _drp.SelectedValue;
            else
                return _drp.Text;
        }

        public override void clear()
        {
            if (_drp.DataSource != null)
                _drp.SelectedIndex = -1;
            else
                _drp.Text = "";
        }
    }
}