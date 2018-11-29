using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace winui.appConfig
{
    public partial class frmMainAppConfig : Form
    {
        public frmMainAppConfig()
        {
            InitializeComponent();
        }

        private void frmMainAppConfig_Load(object sender, EventArgs e)
        {
            loadTree(ui.getTreeXmlPath());
            ui.oForms.setMainForm(this);

            this.Text = "Application Server : " + ui.appName;
        }

        private void loadTree(string sPath)
        {
            var oTree = new TreeDesigner.clsTreeDesigner(treeView1, ui.getTreeXmlPath());
            oTree.readXml();

        }

        private void treeDesignerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sPath = ui.getTreeXmlPath();
            var frmTree = new TreeDesigner.frmTreeDesigner();
            frmTree.load(sPath);
            frmTree.MdiParent = this;
            frmTree.Show();
        }

        private void runMainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmMain = new frmMain();
            frmMain.Show();
            frmMain.Activate();
                     
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node != null && e.Node.Tag != null)
            {
                var treeNode = e.Node.Tag as TreeDesigner.nodeProperty;
                if (treeNode.formPath.isEmpty() == false)
                    ui.showForm(treeNode.formPath, new clsCmd());
            }
        }

        private void btnRefreshTree_Click(object sender, EventArgs e)
        {
            loadTree(ui.getTreeXmlPath());
        }

        private void connectionStringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm1 = new frmConnectionString();
            frm1.ShowDialog();
        }

        private void testGetDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm1 = new frmGetData();
            frm1.MdiParent = this;
            frm1.Show();
            frm1.Activate();
        }

    }
}
