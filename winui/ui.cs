using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using NTier.Request;
using winui;

namespace System
{
    public class ui
    {
        public static string ApplicationTitle = "";

        public static string appName = "";
        public static string appServerRootPath = "";

        public static iBussinessTier oTier;
        public static iFormNavigator oForms;

        public static void alert(string msg)
        {
            MessageBox.Show(msg, ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void warn(string msg)
        {
            MessageBox.Show(msg, ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static bool confirm(string msg)
        {
            if (MessageBox.Show(msg, ApplicationTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                return true;
            else
                return false;
        }

        public static string inputBox(string sTitle, string sDefaultVal = "")
        {
            return "";//Common.DB.Windows.g.inputBox(sTitle, sDefaultVal);
        }


        public static void fillList(DataTable src
            , ListControl cmb)
        {

            if (string.IsNullOrEmpty(cmb.DisplayMember))
                cmb.DisplayMember = src.Columns[0].ColumnName;
            if (string.IsNullOrEmpty(cmb.ValueMember))
                cmb.ValueMember = src.Columns[src.Columns.Count - 1].ColumnName;

            var row = src.NewRow();

            row[0] = "--Select--";
            row[src.Columns.Count - 1] = DBNull.Value;

            src.Rows.InsertAt(row, 0);

            cmb.DataSource = src;

            
        }

        public static void setMainForm(Form frmMain)
        {
            oForms.setMainForm(frmMain);
        }



        public static searchResult showSearchBox(DataTable t, string strTitle = "Search", DAL.GridColumns cols = null)
        {

            return null;
            /*
            var frm1 = new frmSearchBox();
            frm1.Text = strTitle;

            if (cols == null)
            {
                frm1.callSearch(t);
            }
            else
                frm1.callSearch(t, cols);


            frm1.ShowDialog();

            var result = new searchResult(frm1.row);

            if (cols != null) result.PrimaryKeyField = cols.PrimaryKeyField;

            frm1.Dispose();

            return result;
             */
        }


        public static clsMsg login()
        {
            var frm = new frmLogin();
            frm.setTier(oTier);
            frm.ShowDialog();

            return frm.result;
        }


        public static string getAppName()
        {
            return Environment.GetEnvironmentVariable("appName");
        }
        public static string getAppServerRootFolder()
        {
            return Environment.GetEnvironmentVariable("appServerRootFolder");
        }


        public static void showForm(string sKey
            , clsCmd cmd)
        {
            oForms.showForm(sKey, cmd);
            
        }
        internal static string getTreeXmlPath()
        {
            string sPath = appServerRootPath + "\\apps\\" + ui.appName + "\\wintree.xml";
            return sPath;
        }

        internal static string getAppConfigXmlPath()
        {
            string sPath = appServerRootPath + "\\apps\\" + ui.appName + "\\appConfig.xml";
            return sPath;
        }

        internal static string getFormConfigXmlPath()
        {
            string sPath = appServerRootPath + "\\apps\\" + ui.appName + "\\winforms.xml";
            return sPath;
        
        }

        public static void initApp()
        {
            initBll();
            oForms = winui.utility.formNavigator(getFormConfigXmlPath());
            oForms.setTier(oTier);
        }


        public static string showOpenFile(string sFilter = "")
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = sFilter;

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                return openFile.FileName;
            }

            return "";
            
        }

        public static void initBll()
        {
            var oAppServer = new NTier.clsAppServerInfo(ui.appServerRootPath, ui.appName);
            oTier = NTier.Request.utility.createBussinessTierControllerFromXmlForWin2(oAppServer,ui.appName);
            ui.ApplicationTitle = oTier.getAppSetting("applicationTitle");
        }
    }
}
