using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using NTier;
using NTier.Request;

namespace webui
{
    public class xmlMenuNode
    {

        public string type { get; set; }
        public string title { get; set; }
        public string Icon { get; set; }
        public string link { get; set; }
        public List<XmlNode> childMenus { get; set; }

        public string access { get; set; }
        iRoleAccess _role;
        public xmlMenuNode(XmlNode node,iRoleAccess oRole)
        {

            _role = oRole;


            this.title = node.getXmlAttributeValue("title");
            this.Icon = node.getXmlAttributeValue("icon");
            this.childMenus = getAuthorzedNodeList(node.SelectNodes("sub"));
            this.type = node.getXmlAttributeValue("type");
            this.access = node.getXmlAttributeValue("access");
            this.link = node.getXmlAttributeValue("link");
            
        }

        //BussinessLogic _oBL = new BussinessLogic(new AppConfigWeb());

        private List<XmlNode> getAuthorzedNodeList(XmlNodeList nodes)
        {
            List<XmlNode> nodes_return = new List<XmlNode>();

            foreach (XmlNode node in nodes)
            {
                if (isAuthorized(node.getXmlAttributeValue("access"))) nodes_return.Add(node);
            }

            return nodes_return;
        }


        public bool isAuthorized()
        {
            return isAuthorized(this.access);
        }

        public bool isAuthorized(string sAccess)
        {
            if (sAccess.isEmpty()) return true;

            return _role.isAuthorized(sAccess);

        }


    }
}
