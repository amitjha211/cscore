using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DAL;
namespace MyWinControls
{
    public partial class frmRPT_Viewer : Form
    {
        public frmRPT_Viewer()
        {
            InitializeComponent();
        }

        private void frmRPT_Viewer_Load(object sender, EventArgs e)
        {

            this.WindowState = FormWindowState.Maximized;
        }

        public void loadReport(string sTtile, DataSet ds, byte[] output)
        {

            foreach (DataTable t in ds.Tables)
            {
                reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource(t.TableName, t));
            }

            reportViewer1.LocalReport.LoadReportDefinition(g.getMemoryStreamFrom(output));



            this.Text = sTtile;
            reportViewer1.RefreshReport();

            //output.Close();
            //output.Dispose();

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            reportViewer1.Dispose();
        }



    }
}
