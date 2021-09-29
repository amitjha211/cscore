using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using NTier;
using NTier.Request;

namespace webui
{
    public class xmlMenu
    {

        private string _path = "";
        private XmlDocument xDoc;

        public xmlMenu(string sPath)
        {
            loadMe(sPath);
        }

        private void loadMe(string sPath)
        {
            _path = sPath;
            xDoc = new XmlDocument();
            xDoc.Load(_path);
        }

        private NTier.Request.iRoleAccess _role;

        public xmlMenu(NTier.Request.iRoleAccess oRole)
        {
            _role = oRole;
            loadMe(System.Web.HttpContext.Current.Server.MapPath("~/menu.xml"));
        }

        public List<XmlNode> getMainMenu()
        {

            XmlNodeList nodes = xDoc.SelectNodes("//menus/main");
            var lst = getAuthorzedNodeList(nodes);
            return lst;
        }



        public List<XmlNode> getSubMenu(XmlNode mainNode)
        {
            XmlNodeList nodes = mainNode.SelectNodes("sub");
            return getAuthorzedNodeList(nodes);
        }

        private List<XmlNode> getAuthorzedNodeList(XmlNodeList nodes)
        {
            List<XmlNode> nodes_return = new List<XmlNode>();

            foreach (XmlNode node in nodes)
            {
                var oMenuInfo = new xmlMenuNode(node,_role);
                if (oMenuInfo.isAuthorized()) nodes_return.Add(node);
            }

            return nodes_return;
        }





    }
}
