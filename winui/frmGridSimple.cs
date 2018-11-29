using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DAL;

using System.Xml;
using NTier.Request;
namespace winui
{
    public partial class frmGridSimple : Form
    {
        iBussinessTier _AppServer = null;
        XmlNode _xNode = null;
        //XmlDocument _xDoc = null;
        public frmGridSimple(iBussinessTier AppServer,XmlNode xNode) 
        {
            
           
            _AppServer = AppServer ;
            _xNode = xNode;
            InitializeComponent();
        }

        public string path { get; set; }
        public string title { get; set; }

        private string sDataPath { get; set; }
        private void frmGrd_Flat_Load(object sender, EventArgs e)
        {
            title = _xNode.getXmlText("title");
            sDataPath = _xNode.getXmlText("dataPath");
            this.Text = title;
            loadGrid();

        }

        private void loadGrid()
        {
            
            
            var colNodes = _xNode.SelectSingleNode("cols").ChildNodes;

            for (int i = 0; i < colNodes.Count;i++ )
            {

                var colNode = colNodes[i];
                
                string title = colNode.getXmlText("title");
                string dataField = colNode.getXmlText("datafield");
                int iWidth = g.parseInt(colNode.getXmlText("width"));
                iWidth = iWidth == 0 ? 100 : iWidth;

                grd1.addColumn(title, dataField, iWidth);
            }


            grd1.AutoGenerateColumns = false;
            grd1.DataSource = bindingSource1;
            bindingSource1.DataSource = _AppServer.getData(sDataPath, new clsCmd()).Obj;


           
        }
        
        
        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            //ui.grd_ExportToExcel(grd1);
        }

        

        private void exportToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ui.grd_ExportToExcel(grd1);
        }

        private void printListToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

    }
}
