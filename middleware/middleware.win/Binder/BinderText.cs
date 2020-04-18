using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace middleware.win
{

    public class BinderText : BinderFieldBase
    {
        Control txt1;
        public BinderText(Control txt, string sField)
        {
            txt1 = txt;
            objectControl = txt;
            this.Field = sField;
        }
        public override void setValue(object val)
        {
            txt1.Text = val.ToString();
        }

        public override object getValue()
        {
            return txt1.Text;
        }
        public override void clear()
        {
            txt1.Text = "";
        }
    }
}