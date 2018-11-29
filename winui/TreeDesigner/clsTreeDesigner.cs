using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Xml;
using System.Xml.Linq;
using System.Data;
using System.Drawing;
using DAL;

namespace winui.TreeDesigner
{

    public class nodeProperty
    {
        public string title { get; set; }
        public string imageKey { get; set; }
        public string formPath { get; set; }
    }

    class clsTreeDesigner
    {


        clsImageList oImageList = new clsImageList();
        TreeView _tree = null;
        private string _path;

        public clsTreeDesigner(TreeView treeview1, string sPath)
        {
            _tree = treeview1;
            _path = sPath;
        }


        public clsMsg addIcon(string sPath
                , string skey)
        {
            return oImageList.addImage(sPath, skey);

        }



        public DataTable getImageList()
        {
            return oImageList.getData();
        }

        public void addChildNode(string sText = "")
        {

            if (_tree.SelectedNode != null)
            {
                string sTitle = sText;
                if (sText.isEmpty())
                    sText = "Node" + _tree.SelectedNode.Nodes.Count + 1;
                var node = _tree.SelectedNode.Nodes.Add(sText);
                node.Tag = new nodeProperty() { imageKey = "", title = sText };
            }
            else
                ui.warn("Please select the tree node");

        }


        public void moveUpSelectedNode()
        {
            //validation control
            if (_tree.SelectedNode == null)
            {
                ui.warn("Please select the node");
                return;
            }

            if (_tree.SelectedNode.Index <= 0) return;



            // begin
            int iIndex = _tree.SelectedNode.Index - 1;
            TreeNode newNode = (TreeNode)_tree.SelectedNode.Clone();

            var parentNode = _tree.SelectedNode.Parent;
            _tree.SelectedNode.Remove();
            parentNode.Nodes.Insert(iIndex, newNode);
            _tree.SelectedNode = newNode;

        }

        public void moveDownSelectedNode()
        {
            //Common Declaration
            var parentNode = _tree.SelectedNode.Parent;

            //validation control
            if (_tree.SelectedNode == null)
            {
                ui.warn("Please select the node");
                return;
            }

            if (_tree.SelectedNode.Index >= (parentNode.Nodes.Count - 1))
                return;



            // begin
            int iIndex = _tree.SelectedNode.Index + 1;
            TreeNode newNode = (TreeNode)_tree.SelectedNode.Clone();


            _tree.SelectedNode.Remove();
            parentNode.Nodes.Insert(iIndex, newNode);
            _tree.SelectedNode = newNode;


        }




        public void save()
        {
            XElement el = new XElement("Tree");
            XElement elNodes = new XElement("nodes");
            el.Add(elNodes);

            addXElement(elNodes, _tree.Nodes);
            el.Add(oImageList.getInXml());

            if (System.IO.File.Exists(_path)) System.IO.File.Delete(_path);
            using (var st = new System.IO.FileStream(_path, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite))
            {

                el.Save(st);
            }
        }


        public void saveInXml(string sPath)
        {
            XElement el = new XElement("Tree");
            addXElement(el, _tree.Nodes);
            el.Save(sPath);
        }

        private void addXElement(XElement elParent, TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                XElement el = new XElement("node");

                var nodeInfo = (nodeProperty)node.Tag;


                el.SetAttributeValue("title", node.Text);
                if (nodeInfo != null)
                {
                    el.SetAttributeValue("imageKey", nodeInfo.imageKey);
                    el.SetAttributeValue("formPath", nodeInfo.formPath);
                }
                elParent.Add(el);

                if (node.Nodes != null && node.Nodes.Count > 0)
                {
                    addXElement(el, node.Nodes);
                }
            }
        }


        public void fillMenu(MenuStrip menu1)
        {
            //cleaning process and declaration.
            menu1.Items.Clear();

            var xDoc = XDocument.Load(_path);

            //Image List
            var elImageList = xDoc.Element("Tree").Element("imageList");
            oImageList.loadXml(elImageList);

            var imgList = new ImageList();
            imgList.Images.Clear();
            var tImageList = oImageList.getData();

            foreach (DataRow r in tImageList.Rows)
            {

                byte[] imageData = (byte[])r["image_data"];
                var ms = g.ConvertBytesToMemoryStream(imageData);
                imgList.Images.Add(r["image_key"].ToString(), Image.FromStream(ms));
            }
            menu1.ImageList = imgList;

            //fill Tree Node
            var lstElments = xDoc.Element("Tree").Elements("nodes").Elements("node").ToList();
            if (lstElments.Count > 0)
                fillMenu(lstElments[0].Elements().ToList(), menu1.Items, imgList);



        }

        private void fillMenu(List<XElement> lstElement
            , ToolStripItemCollection menuItems
            , ImageList imgList)
        {

            foreach (var el in lstElement)
            {

                string skey = "";
                string sTitle = el.Attribute("title").Value;
                if (el.Attribute("imageKey") != null)
                    skey = el.Attribute("imageKey").Value;
                string sPath = "";
                if (el.Attribute("formPath") != null)
                    sPath = el.Attribute("formPath").Value;

                ToolStripMenuItem menuItem = new ToolStripMenuItem();//menuItems.Add(sTitle);
                menuItem.Text = sTitle;
                menuItem.Tag = new nodeProperty() { title = sTitle, imageKey = skey, formPath = sPath };
                menuItem.ImageKey = skey;
                menuItem.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                if (imgList.Images.ContainsKey(skey))
                    menuItem.Image = imgList.Images[skey];
                //menuItem.SelectedImageKey = skey;
                menuItems.Add(menuItem);

                var lstElementChild = el.Elements("node").ToList();
                if (lstElementChild.Count > 0) fillMenu(lstElementChild, menuItem.DropDownItems, imgList);
            }
        }






        public void readXml()
        {
            readXml(_path);
        }
        public void readXml(string sPath)
        {
            //cleaning process and declaration.
            _tree.Nodes.Clear();

            var xDoc = XDocument.Load(sPath);

            //Image List
            var elImageList = xDoc.Element("Tree").Element("imageList");
            oImageList.loadXml(elImageList);

            if (_tree.ImageList != null)
            {
                _tree.ImageList.Images.Clear();
                var tImageList = oImageList.getData();
                foreach (DataRow r in tImageList.Rows)
                {

                    byte[] imageData = (byte[])r["image_data"];
                    var ms = g.ConvertBytesToMemoryStream(imageData);
                    _tree.ImageList.Images.Add(r["image_key"].ToString(), Image.FromStream(ms));


                }
            }

            //fill Tree Node
            var lstElments = xDoc.Element("Tree").Elements("nodes").Elements("node").ToList();
            readXElement(lstElments, _tree.Nodes);

        }

        private void readXElement(List<XElement> lstElement, TreeNodeCollection nodes)
        {

            foreach (var el in lstElement)
            {

                string skey = "";
                string sTitle = el.Attribute("title").Value;
                if (el.Attribute("imageKey") != null)
                    skey = el.Attribute("imageKey").Value;
                string sPath = "";
                if (el.Attribute("formPath") != null)
                    sPath = el.Attribute("formPath").Value;

                var xNode = nodes.Add(sTitle);
                xNode.Tag = new nodeProperty() { title = sTitle, imageKey = skey, formPath = sPath };
                xNode.ImageKey = skey;
                xNode.SelectedImageKey = skey;


                var lstElementChild = el.Elements("node").ToList();
                if (lstElementChild.Count > 0) readXElement(lstElementChild, xNode.Nodes);
            }
        }


        TreeNode _xCopiedNode = null;

        internal void deleteNode()
        {

            if (_tree.SelectedNode == null) return;
            _tree.SelectedNode.Remove();
        }


        internal void cutNode()
        {
            if (_tree.SelectedNode == null) return;

            if (_xCopiedNode != null)
            {
                _xCopiedNode.ForeColor = Color.Black;
                _xCopiedNode = null;
            }


            _xCopiedNode = _tree.SelectedNode;
            _xCopiedNode.ForeColor = Color.Red;

        }


        internal void pasteNode()
        {
            if (_tree.SelectedNode != null && _xCopiedNode != null)
            {
                TreeNode xNode = _xCopiedNode.Clone() as TreeNode;
                _xCopiedNode.Remove();
                _xCopiedNode = null;
                _tree.SelectedNode.Nodes.Add(xNode);
            }

        }


    }
}
