using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace winui
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.loadTree(ui.getTreeXmlPath());
            this.Text = ui.ApplicationTitle;
            ui.oForms.setMainForm(this);
        }

        public void loadTree(string sPath)
        {
            var oTree = new TreeDesigner.clsTreeDesigner(treeView1, ui.getTreeXmlPath());
            oTree.readXml();
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

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
               
        }

        private void treeDesginerToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var frm = new TreeDesigner.frmTreeDesigner();
            frm.load(ui.getTreeXmlPath());
            frm.ShowDialog();

            this.loadTree(ui.getTreeXmlPath());


        }

    }
}
