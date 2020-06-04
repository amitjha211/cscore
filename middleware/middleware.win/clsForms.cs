using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace middleware.win
{

    public class clsFormInfo : clsObjectBase
    {

        public string title, assemblyName, classPath;
        public List<string> methods;

        private Form frm;

        public clsAPIResponse getForm(clsCmd cmd)
        {

            if (frm != null && frm.IsDisposed) frm = null;


            if (frm == null)
            {
                frm = Activator.CreateInstance(assemblyName, classPath).Unwrap() as Form;

            }


            return new clsAPIResponse() { result = frm };
        }

    }

    public class clsMenu : clsObjectBase
    {
        public string title, formKey,icon;
        public List<clsMenu> submenus;
    }

    public class clsTypeInfo : clsObjectBase
    {
        public string assemblyName, classPath;
    }

    public class clsObjectMap : clsObjectBase
    {
        public string objectKey, formKey;
    }

    public class clsFormObjects
    {
        public string applicationTitle;
        public List<clsTypeInfo> types;
        public List<clsObjectMap> objectMap;
        public List<clsGrid> grids;
        public List<clsFormInfo> forms;
        public List<clsMenu> menu;
    }

    public class clsFormService
    {


        private clsAppService2 _appService = null;

        public clsFormService(clsAppService2 appService)
        {
            _appService = appService;
        }


        public Form frmMDIMain;

        private List<clsCallObjects> lstRequest = new List<clsCallObjects>();

        clsJSONParser oJSONParser = new middleware.clsJSONParser();
        internal clsFormObjects oFormObjects;

        public void includeFiles(List<string> lstPath)
        {

            foreach (string sPath in lstPath)
            {

                string sJSON = System.IO.File.ReadAllText(sPath);
                oJSONParser.loadType(sJSON);

                var _middleObjects = oJSONParser.getJSONObject(sJSON, typeof(clsFormObjects)) as clsFormObjects;

                if (oFormObjects == null)
                {
                    oFormObjects = _middleObjects;
                    return;
                }


                //foreach (var obj in _middleObjects)
                //{
                //    middleObjects.dbConnections.Add(obj);
                //}


                //foreach (var obj in _middleObjects.objectTypes)
                //{
                //    middleObjects.objectTypes.Add(obj);
                //}


                //foreach (var obj in _middleObjects.tables)
                //{
                //    middleObjects.tables.Add(obj);
                //}

            }
        }

        public void compile()
        {

            foreach (var f in oFormObjects.grids)
            {

                string[] methods = new string[] { "getForm", "getSearchForm" };
                foreach (string sMethod in methods)
                {
                    var objCaller = new clsCallObjects();
                    objCaller.Add(f, sMethod);
                    objCaller.path = f.name + "/" + sMethod;
                    objCaller.compile();
                    lstRequest.Add(objCaller);
                }
            }
            foreach (var f in oFormObjects.forms)
            {

                //string[] methods = new string[] { "getForm" };
                foreach (string sMethod in f.methods)
                {
                    var objCaller = new clsCallObjects();
                    objCaller.Add(f, sMethod);
                    objCaller.path = f.name + "/" + sMethod;
                    objCaller.compile();
                    lstRequest.Add(objCaller);
                }
            }
        }

        public clsAPIResponse call(string sPath, clsCmd cmd)
        {
            var obj = lstRequest.Find(p => p.path == sPath);
            if (obj == null) return new clsAPIResponse() { message = string.Format("path [{0}] not found !", sPath) };
            return obj.call(cmd);
        }

        public void openForm(string path)
        {
            openForm(path, new clsCmd());
        }

        private void setProp(Form frm, bool setMDIForm = true)
        {
            if (setMDIForm) frm.MdiParent = this.frmMDIMain;

            if (frm is frmBase)
            {
                var frm2 = frm as frmBase;

                frm2.appService = _appService;
                frm2.formService = this;
            }
        }

        public void openForm(string path, clsCmd cmd)
        {
            var res = call(path, cmd);

            if (res.result is Form)
            {
                var frm = res.result as Form;
                setProp(frm);
                frm.Show();
                frm.Activate();
            }
        }

        public Form getForm(string path, clsCmd cmd)
        {
            var res = call(path, cmd);

            if (res.result is Form)
            {
                var frm = res.result as Form;
                setProp(frm);
                return frm;     
            }

            return null;


        }



        public void openFormEntryDialog(string path
            , DataRow r)
        {
            var res = call(path, new clsCmd());

            if (res.result is Form)
            {
                var frm = res.result as Form;
                //frm.MdiParent = clsMiddlewareUI.frmMDIMain;
                setProp(frm, false);

                var m = frm.GetType().GetMethod("fillMe");
                if (m != null)
                {
                    m.Invoke(frm, new object[] { r });
                }

                frm.ShowDialog();
                frm.Dispose();
            }
        }


        public clsAPIResponse openDialog(string path
            , Action<string, object> aFormCommand = null)
        {
            var res = call(path, new clsCmd());

            if (res.result is frmBaseEntry)
            {
                var frm = res.result as frmBaseEntry;


                setProp(frm, false);

                frm.formCommand = aFormCommand;
                frm.ShowDialog();

                var cmd = frm.Tag;
                frm.Dispose();

                if (cmd == null)
                    return clsAPIResponse.get("Not selected");
                else
                    return clsAPIResponse.ok(cmd);

            }

            return clsAPIResponse.ok("Path  not found !");
        }

        public void fillDropDown(ComboBox drp1, string sDataPath)
        {
            fillDropDown(drp1, sDataPath, new clsCmd());
        }

        public void fillDropDown(ComboBox drp1
            , string sDataPath
            , clsCmd cmd
            , string sDisplayMember = ""
            , string sValueMember = "")
        {

            var _response = _appService.call(sDataPath, cmd);
            if (_response.isValid)
            {
                DataTable t = _response.result as DataTable;

                drp1.DisplayMember = sDisplayMember.isEmpty() ? t.Columns[0].ColumnName : sDisplayMember;
                drp1.ValueMember = sValueMember.isEmpty() ? t.Columns[t.Columns.Count - 1].ColumnName : sValueMember;
                drp1.DataSource = t;
            }
        }


    }
}

