using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using DAL;
using NTier.Request;
namespace winui
{

    internal class clsFormNavigatorFromXml : iFormNavigator
    {

        
        NTier.Request.iBussinessTier _Tier;
        private myAssembly oAssembly = new myAssembly();

        public void setTier(NTier.Request.iBussinessTier oTier)
        {
            _Tier = oTier;
        
            oAssembly.appServerRootPath = ui.appServerRootPath;
            oAssembly.appName = ui.appName;
            oAssembly.loadDll(xmlDoc);


        }


        Form _frmMain = null;

        XmlDocument xmlDoc = null;
        Dictionary<string, Form> oForms = new Dictionary<string, Form>();




        public clsFormNavigatorFromXml(string sPath)
        {
            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(System.IO.File.ReadAllText(sPath));
        }

        public void setMainForm(Form frmMain)
        {
            _frmMain = frmMain;
        }

        public Form getForm(string sKey, clsCmd cmd)
        {



            Form frm = null;

            //Control
            if (oForms.ContainsKey(sKey))
            {
                frm = oForms[sKey];
                if (frm.IsDisposed) oForms.Remove(sKey);
                return frm;
            }

            XmlNode el = xmlDoc.SelectSingleNode("//request/frm[@formKey='" + sKey + "']");

            string sAssemblyName = el.getXmlAttributeValue("assemblyName");
            string sFormPath = el.getXmlAttributeValue("formPath");
            string sFormClassPath = el.getXmlAttributeValue("formClassPath");

            if (sFormClassPath.isEmpty())
            {
                winui.iFormFactory oFormFactory = oAssembly.createInstance( sAssemblyName, sAssemblyName + ".formService") as winui.iFormFactory;
                oFormFactory.setTier(_Tier);
                frm = oFormFactory.getForm(sFormPath);
            }
            else
            {

                var frm1 = oAssembly.createInstance(sAssemblyName, sFormClassPath) as winui.frmBase;
                frm1.setTier(_Tier);
                frm = frm1;

            }

            if (frm == null)
                throw new Exception(string.Format("[{0}] assembly name and class path [{1}] not found !", sAssemblyName, sFormPath));
            return frm;
        }

        private string getPackagePath(string packageFile)
        {
            string sPath = ui.appServerRootPath + "\\packages\\" + packageFile;
            return sPath;
        }


        //private void loadDll()
        //{
        //    XmlNodeList package_nodes = xmlDoc.SelectNodes("//request/packages/package");
        //    foreach (XmlNode package_node in package_nodes)
        //    {
        //        string sPath = getPackagePath(package_node.getXmlAttributeValue("assemblyName"));
        //        oAssembly.add(sPath);
        //    }
        //}


        public void showForm(string skey, clsCmd cmd)
        {
            var frm = getForm(skey, cmd);
            //test 
            frm.MdiParent = _frmMain;
            frm.Show();
            frm.Activate();
        }



    }
}
