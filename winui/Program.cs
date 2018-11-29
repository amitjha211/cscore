using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
namespace winui
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        static string sForm = "";

        [STAThread]
        static void Main(string[] args)
        {
            //assemblyTest();
            //return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            setVar(args);

            if (ui.appName.isEmpty())
            {
                Console.Write("Please enter the AppName : ");
                ui.appName = Console.ReadLine();
            }

            if (ui.appName.isEmpty()) return;

            if (args.Length > 0)
                sForm = args[0];

            if (sForm.isEmpty())
            {
                Console.Write("Enter The Form Command : ");
                sForm = Console.ReadLine();
            }
            if (sForm.isEmpty()) return;

            ui.initApp();
            startForm(sForm);
        }

        private static void setVar(string[] args)
        {
            ui.appServerRootPath = Environment.GetEnvironmentVariable("appServerRootFolder");
            ui.appName = Environment.GetEnvironmentVariable("appName");

            if (args.Length == 2 && (args[0] == "tree" || args[0] == "main" || args[0] == "main2" || args[0] == "util") && args[1].isEmpty() == false)
            {
                ui.appName = args[1];
            }
        }

        
        private static void startForm(string sformKey = "")
        {
            Form frm1 = null;
            if (sformKey.isEmpty() == false)
                switch (sformKey)
                {
                    case "main":
                        var frmMain = new frmMain();
                     
                        frm1 = frmMain;
                        break;
                    case "main2":
                        var frmMain2 = new frmMain2();
                        frmMain2.loadMenu(ui.getTreeXmlPath());
                        frm1 = frmMain2;
                        break;
                    case "tree":
                        string sPath = ui.getTreeXmlPath();
                        var frmTree = new TreeDesigner.frmTreeDesigner();
                        frmTree.load(sPath);
                        frm1 = frmTree;
                        break;
                    case "util":
                        var frmUtil = new appConfig.frmMainAppConfig();
                        frm1 = frmUtil;
                        break;

                }

            if (frm1 != null)
            {
                frm1.ShowDialog();
                //Application.Run(frm1);
            }//frm1.ShowDialog();
        }
    }
}
