using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
namespace winui.appConfig
{
    internal class clsAppConfig
    {
        XmlDocument xDoc = null;
        string _xmlPath = "";

        public clsAppConfig(string sXmlPath)
        {
            _xmlPath = sXmlPath;

            xDoc = new XmlDocument();

            xDoc.Load(_xmlPath);
            
        }

        public string defaultConnectionString
        {
            get
            {
                return xDoc.getXmlText("//appConfig/defaultConnectionString");
            }
            set 
            {
                XmlNode node = xDoc.SelectSingleNode("//appConfig/defaultConnectionString");
                if (node != null) node.InnerText = value;
            }
        }


        public DataTable getDataList()
        {

            return null;    
        }

        public void save()
        {
            xDoc.Save(_xmlPath);
        }
    }
}
