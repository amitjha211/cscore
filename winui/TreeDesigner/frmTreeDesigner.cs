using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace winui.TreeDesigner
{
    internal partial class frmTreeDesigner : Form
    {


        public frmTreeDesigner()
        {
            InitializeComponent();
        }
        clsTreeDesigner oTreeDesigner;

        private void btnChild_Click(object sender, EventArgs e)
        {
            oTreeDesigner.addChildNode(ui.inputBox("Please enter node title", ""));


        }

        private void frmTreeDesigner_Load(object sender, EventArgs e)
        {

        }

        public void load(string sPath)
        {
            oTreeDesigner = new clsTreeDesigner(treeView1, sPath);
            if (!System.IO.File.Exists(sPath)) return;
            oTreeDesigner.readXml();
            grdImageList.DataSource = oTreeDesigner.getImageList();
        }
        private void btnUp_Click(object sender, EventArgs e)
        {
            oTreeDesigner.moveUpSelectedNode();

        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            oTreeDesigner.moveDownSelectedNode();
        }



        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshTree();
        }

        private void RefreshTree()
        {
            oTreeDesigner.readXml();
            grdImageList.AutoGenerateColumns = false;
            grdImageList.DataSource = oTreeDesigner.getImageList();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            oTreeDesigner.save();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFile.Text = openFileDialog1.FileName;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            oTreeDesigner.addIcon(txtFile.Text
                , txtImageKey.Text);

            grdImageList.DataSource = oTreeDesigner.getImageList();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            propertyGrid1.SelectedObject = e.Node.Tag;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(treeView1.SelectedNode != null 
                && ui.confirm(string.Format("Are you sure want delete [{0}] Node ?",treeView1.SelectedNode.Text)))
                oTreeDesigner.deleteNode();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            oTreeDesigner.cutNode();
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(treeView1, e.Location);
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            oTreeDesigner.pasteNode();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            oTreeDesigner.addChildNode(ui.inputBox("Please enter node title", ""));

        }

        private void saveRefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {

            oTreeDesigner.save();

            //refresh
            oTreeDesigner.readXml();
            grdImageList.AutoGenerateColumns = false;
            grdImageList.DataSource = oTreeDesigner.getImageList();

        }


    }
}
