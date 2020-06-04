using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using winui.TreeDesigner;
using NTier.Request;

namespace winui
{
    public static class utility
    {

        private static Form _frmMdi;

        

        public static void setMDIForm(Form frm)
        {
            _frmMdi = frm;
        }

        public static void showTreeDesigner(Form frmMdi,string sPath)
        {
            var frm1 = new TreeDesigner.frmTreeDesigner();
            frm1.MdiParent = frmMdi;
            frm1.load(sPath);
            frm1.Show();
        }

        public static void loadTree(TreeView tree, string sPath)
        {
            clsTreeDesigner oTreeDesigner = new clsTreeDesigner(tree, sPath);
            oTreeDesigner.readXml();
        }

        public static void showForm(string sPath)
        {
            var frm = getForm(sPath);
            frm.MdiParent = _frmMdi;
            frm.Show();
        }

        public static iFormNavigator formNavigator(string sPath)
        {
            return new clsFormNavigatorFromXml(sPath);
        }

        public static Form getForm(string sPath)
        {

            //var oRequestPath =  new NTier.Request.requestPath(sPath);
            //var xDoc =  new xmlDoc//_AppServer.getXmlDoc(oRequestPath.getPath());

            //Form frm = null;

            //if (xDoc.SelectSingleNode("//request").Attributes["type"].InnerText == "form")
            //{
            //    var node = xDoc.SelectSingleNode("//request/frm[@name='" + oRequestPath.getQueryString("name") + "']");
            //    if (node.Attributes["type"].Value == "winGrid")
            //    {

            //        var frmGrd = new frmGridSimple(_AppServer,node);
            //        frmGrd.MdiParent = _frmMdi;
            //        frmGrd.path = sPath;
            //        frm = frmGrd;
            //    }

            //}
            //return frm;
            return null;
        }

        public static void OpenReport(string sTitle
            , NTier.sqlReport.SQLReportBase rpt)
        {
            var frm1 = new frmRPT_Viewer();
            frm1.Text = sTitle;
            //frm1.MdiParent = ui.frmMain;
            frm1.loadReport(sTitle, rpt.ds, rpt.output);
            frm1.Show();
        }



    }

}
