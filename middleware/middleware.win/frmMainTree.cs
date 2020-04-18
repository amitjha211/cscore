using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using winui;
namespace middleware.win
{

    public partial class frmMainTree : frmBase
    {
        
        public frmMainTree()
        {
            InitializeComponent();
        }


        private void frmMainTree_Load(object sender, EventArgs e)
        {
            loadProperty();
            formService.frmMDIMain = this;
        }

        private void loadProperty()
        {

            this.Text = formService.oFormObjects.applicationTitle;
            fillTree(treeView1.Nodes, formService.oFormObjects.menu);
            treeView1.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(treeView1_NodeMouseDoubleClick);
        }

        private void openForm(string sKey)
        {


            formService.openForm(sKey);

            //var oFormKey = oApp.forms.Find(p => p.name == sKey);
            //if (oFormKey == null)
            //{
            //    ui.warn(string.Format("Form Key {0} not found !", sKey));
            //    return;
            //}

            //if (oApp.forms != null)
            //{
            //    var oFormInfo = oApp.forms.Find(p => p.name == sKey);

            //    if (oFormInfo.frm == null || (oFormInfo.frm != null && oFormInfo.frm.IsDisposed))
            //    {
            //        //if (!string.IsNullOrWhiteSpace(oFormInfo.filePath)) oFormInfo.frm = clsFormService.getFormFromFile(oFormInfo.filePath);

            //        if (!string.IsNullOrWhiteSpace(oFormInfo.assemblyName) && !string.IsNullOrWhiteSpace(oFormInfo.classPath))
            //        {
            //            oFormInfo.frm = g.createObjectFromAssemblyInfo(oFormInfo.assemblyName, oFormInfo.classPath) as Form;
            //            oFormInfo.frm.MdiParent = this;
            //        }
            //    }
            //    if (oFormInfo.frm != null && oFormInfo.frm.IsDisposed == false)
            //    {
            //        oFormInfo.frm.Show();
            //    }

            //}



        }

        void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                clsMenu oMenu = (clsMenu)e.Node.Tag;

                if (oMenu != null)
                    openForm(oMenu.formKey);
            }
        }

        private void fillTree(TreeNodeCollection rootTreeNode
            , List<clsMenu> data_nodes)
        {
            foreach (clsMenu _node in data_nodes)
            {
                TreeNode _treeNode = rootTreeNode.Add(_node.title);
                _treeNode.ImageKey = _node.icon;
                _treeNode.SelectedImageKey = _node.icon;
                _treeNode.Tag = _node;
            }

            int i = 0;

            foreach (clsMenu _node in data_nodes)
            {

                if (_node.submenus != null && _node.submenus.Count > 0)
                    fillTree(rootTreeNode[i].Nodes, _node.submenus);
                i++;
            }
        }






    }
}
