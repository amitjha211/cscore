using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using DAL;
namespace winui
{

    public class BinderDatePicker : BinderFieldBase
    {
        DateTimePicker _dtp;

        public BinderDatePicker(DateTimePicker dtp, string sField)
        {
            _dtp = dtp;
            this.Field = sField;
        }

        public override void setValue(object val)
        {

            if (val == DBNull.Value)
            {
                if (_dtp.ShowCheckBox)
                {
                    _dtp.Checked = false;
                    return;
                }
            }
            else
            {
                _dtp.Value = Convert.ToDateTime(val);
            }
        }

        public override object getValue()
        {
            if (_dtp.ShowCheckBox && _dtp.Checked == false) return DBNull.Value;
            return _dtp.Value.ToString("dd/MMM/yyyy") ;
        }
        public override void clear()
        {
            _dtp.Value = System.DateTime.Today;
        }
    }
}