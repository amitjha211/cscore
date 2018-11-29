using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using DAL;
namespace MyWinControls
{

    public class BinderHtmlEditor : BinderFieldBase
    {
        GvS.Controls.HtmlTextbox  txt1;
        public BinderHtmlEditor(GvS.Controls.HtmlTextbox  txt, string sField)
        {
            txt1 = txt;
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