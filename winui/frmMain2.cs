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
    public partial class frmMain2 : Form
    {


        public frmMain2()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

            this.loadMenu(ui.getTreeXmlPath());

            ui.oForms.setMainForm(this);
            this.Text = ui.ApplicationTitle;
            
            var msg = ui.login();
            if (msg.Validated == false)
                this.Close();
        }

        public void loadMenu(string sPath)
        {
            var oTree = new TreeDesigner.clsTreeDesigner(null, ui.getTreeXmlPath());
            oTree.fillMenu(menuStrip1);
            setEvent(menuStrip1.Items);
        }


        private EventHandler dlgMenuItemClick = delegate(object sender, EventArgs e)
        {


            if (sender is ToolStripMenuItem)
            {
                var m = sender as ToolStripMenuItem;
                if (m.Tag != null)
                {
                    //g.openForm(m.Tag.ToString());
                    try
                    {
                        var nodeProperty = m.Tag as winui.TreeDesigner.nodeProperty;
                        if (nodeProperty.formPath.isEmpty() == false)
                            ui.showForm(nodeProperty.formPath, new clsCmd());
                    }
                    catch (Exception ex)
                    {
                        ui.warn(ex.Message);
                    }
                }
            }
        };


        private void setEvent(ToolStripItemCollection cln)
        {

            for (int i = 0; i < cln.Count; i++)
            {
                if (cln[i] is ToolStripMenuItem)
                {
                    var m = cln[i] as ToolStripMenuItem;
                    m.Click += dlgMenuItemClick;
                    if (m.DropDownItems.Count > 0) setEvent(m.DropDownItems);

                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

    }
}
