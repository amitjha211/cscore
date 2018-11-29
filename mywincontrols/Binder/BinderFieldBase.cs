using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using DAL;
namespace MyWinControls
{

    public abstract class BinderFieldBase
    {
        public string Field { get; set; }

        public abstract void setValue(object val);
        public abstract object getValue();
        public abstract void clear();

        public void setValue(DataRow r)
        {
            this.setValue(r[Field]);
        }
    }

}