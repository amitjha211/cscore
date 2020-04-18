using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace middleware.win
{
    public static class Extensions
    {
        public static void addColumn(this  DataGridView grd,string sColumnHeader, string sDataField, int iWidth)
        {
            var col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = sDataField;
            col.HeaderText = sColumnHeader;
            col.Width = iWidth;
            grd.Columns.Add(col);
        }

    }
}
