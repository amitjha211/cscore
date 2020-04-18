using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace middleware.win
{

    public class BinderCheckbox : BinderFieldBase
    {
        CheckBox _chk1;
        public BinderCheckbox(CheckBox chk1, string sField)
        {
            _chk1 = chk1;
            objectControl = chk1;
            this.Field = sField;
        }

        public override void setValue(object val)
        {
            _chk1.Checked = val == DBNull.Value ? false : Convert.ToBoolean(val); ;
        }

        public override object getValue()
        {
            return _chk1.Checked;
        }

        public override void clear()
        {
            _chk1.Checked = false;
        }
    }
}