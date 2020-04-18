using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using DAL;

namespace MyWinControls
{

    public class searchResult
    {

        public string PrimaryKeyField { get; set; }
        clsCmd cmd = new clsCmd();


        public searchResult(DataRow r)
        {

            if (r != null)
            {
                cmd.AddValues(r);
            }
        }

        public bool validated
        {
            get
            {
                return cmd.Count > 0 ? true : false;
            }
        }

        public object getVal(string sField)
        {
            return cmd[sField].Value;
        }

        public int getID()
        {
            if (PrimaryKeyField.isEmpty() == false)
                return cmd.getIntValue(PrimaryKeyField);
            else
                return g.parseInt(cmd[cmd.Count - 1].Value);
        }
    }



    public class ui
    {

        public static string ApplicationTitle { get; set; }

        public static void fillList(DataTable src
            , ListControl cmb)
        {

            if (string.IsNullOrEmpty(cmb.DisplayMember))
                cmb.DisplayMember = src.Columns[0].ColumnName;
            if (string.IsNullOrEmpty(cmb.ValueMember))
                cmb.ValueMember = src.Columns[src.Columns.Count - 1].ColumnName;

            var row = src.NewRow();

            row[0] = "--Select--";
            row[src.Columns.Count - 1] = DBNull.Value;

            src.Rows.InsertAt(row, 0);

            cmb.DataSource = src;
        }

        public static searchResult showSearchBox(DataTable t, string strTitle = "Search", DAL.GridColumns cols = null)
        {

            var frm1 = new frmSearchBox();
            frm1.Text = strTitle;

            if (cols == null)
            {
                frm1.callSearch(t);
            }
            else
                frm1.callSearch(t, cols);


            frm1.ShowDialog();

            var result = new searchResult(frm1.row);

            if (cols != null) result.PrimaryKeyField = cols.PrimaryKeyField;

            frm1.Dispose();

            return result;
        }

        public static void OpenReport(string sTitle, DAL.SQLReport rpt)
        {
            var frm1 = new frmRPT_Viewer();
            frm1.Text = sTitle;
            //frm1.MdiParent = ui.frmMain;
            frm1.loadReport(sTitle, rpt.ds, rpt.output);
            frm1.Show();
        }

        public static frmRPT_Viewer getReportForm(string sTitle, DAL.SQLReport rpt)
        {
            var frm1 = new frmRPT_Viewer();
            frm1.Text = sTitle;
            //frm1.MdiParent = ui.frmMain;
            frm1.loadReport(sTitle, rpt.ds, rpt.output);
            return frm1;
        }

        public static void grid_CheckAll(MyGrid grd, CheckBox chk1, string sField)
        {
            var blnVal = chk1.Checked;
            //
            var t = grd.getTable();
            foreach (DataRow r in t.Rows)
            {
                r[sField] = blnVal;
            }
            grd.Refresh();
        }

        public static void OpenReport(string sTitle, DataSet ds, byte[] output)
        {

            var frm1 = new frmRPT_Viewer();
            frm1.loadReport(sTitle, ds, output);
            frm1.Show();
        }


        public static void grd_ExportToExcel(MyGrid grd)
        {

            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "Excel Files|*.xls";

            if (fd.ShowDialog() == DialogResult.OK)
            {
                DAL.ExportToExcel xls = new ExportToExcel();

                for (int i = 0; i < grd.Columns.Count; i++)
                {
                    if (grd.Columns[i].Visible == true
                        && grd.Columns[i].DataPropertyName.isEmpty() == false)

                        xls.addColumn(grd.Columns[i].DataPropertyName, grd.Columns[i].HeaderText);
                }

                var t = grd.getTable();
                System.IO.File.WriteAllBytes(fd.FileName, xls.exportToExcel(t));
            }
            else
                return;
        }

        
        public static void alert(string msg)
        {
            MessageBox.Show(msg, ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void warn(string msg)
        {
            MessageBox.Show(msg, ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static bool confirm(string msg)
        {
            if (MessageBox.Show(msg, ApplicationTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                return true;
            else
                return false;
        }


        public static string inputBox(string sTitle, string sDefaultVal = "")
        {
            return Common.DB.Windows.g.inputBox(sTitle, sDefaultVal);
        }

    }
}
