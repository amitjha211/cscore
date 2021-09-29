using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Data;
using System.Web;

namespace webui
{
    public abstract class ServiceController3 : myController
    {

        int _start = 0;
        int _length = 10;
        int _draw = 1;

        protected void setPageSize(int iPageSize)
        {
            _length = iPageSize;
        }

        public int start
        {
            get
            {
                //get start to skip data
                if (Request["start"] != null && Request["start"] != "")
                    _start = Convert.ToInt32(Request["start"]);

                return _start;
            }
        }


        public int length
        {
            get
            {
                if (Request["length"] != null && Request["length"] != "")
                    _length = Convert.ToInt32(Request["length"]);

                return _length;
            }
        }

        public int draw
        {
            get
            {
                if (Request["draw"] != null && Request["draw"] != "")
                    _draw = Convert.ToInt32(Request["draw"]);

                return _draw;
            }
        }



        public ContentResult getdataAll(FormCollection frm)
        {


            System.Threading.Thread.Sleep(300);

            var cmd = new clsCmd();
            webUtil.addParamFromPost(cmd, frm);

            string sPath = Request.QueryString["path"];


            DataTable t = null;
            clsMsg result;

            result = webUtil.tier.getData(sPath, cmd);


            t = result.Obj as DataTable;

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(t), "application/json");
        }




        public ContentResult getdataPaging(FormCollection frm)
        {
            System.Threading.Thread.Sleep(300);


            var cmd = new clsCmd();
            webUtil.addParamFromPost(cmd, frm);
            string sPath = Request.QueryString["path"];


            string sSortType = cmd.getStringValue("$sort");
            if (cmd.ContainFields("$sort"))
            {
                cmd.Remove(cmd["$sort"]);
            }

            var result = webUtil.tier.getData(sPath, cmd);

            if (result.Validated)
            {
                DataTable t = result.Obj as DataTable;
                var tPaging = g.getJsonPaging(t, sSortType, start, length);

                return Content(Newtonsoft.Json.JsonConvert.SerializeObject(tPaging), "application/json");
            }
            else
            {
                var res = new { draw = draw, recordsTotal = 0, recordsFiltered = 0, data = "", error = true, error_msg = result.Message };
                return Content(Newtonsoft.Json.JsonConvert.SerializeObject(res), "application/json");
            }

        }


        [HttpPost, ValidateInput(false)]
        public virtual JsonResult UpdateModule(FormCollection frm)
        {
            string sPath = Request.QueryString["path"];


            var cmd = new clsCmd();
            webUtil.addParamFromPost(cmd, frm);


            if (!sPath.isEmpty())
            {
                try
                {
                    var result = webUtil.tier.exec(sPath, cmd);
                    return Json(new { msg = result.Message, data = result.Obj }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { msg = ex.Message, data = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                object n = null;
                return Json(new { msg = "You have not specified [Module Name] and OperationName", data = n }, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost, ValidateInput(false)]
        public JsonResult setReport(FormCollection frm)
        {

            string sPath = Request.QueryString["path"];

            var cmd = new clsCmd();
            webUtil.addParamFromPost(cmd, frm);


            if (!string.IsNullOrWhiteSpace(sPath))
            {
                try
                {
                    var rpt = webUtil.tier.getSQLReport(sPath, cmd);
                    Session["rpt"] = rpt;
                    Session["rptName"] = rpt.downloadName.isEmpty() ? sPath : rpt.downloadName;
                    return Json(new { msg = "", data = "" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { msg = ex.Message, data = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                object n = null;
                return Json(new { msg = "You have not specified [Report path]", data = n }, JsonRequestBehavior.AllowGet);
            }
        }



        public FileResult downloadSQLReport()
        {
            var rpt = (NTier.sqlReport.iSQLReport)Session["rpt"];

            string sPath = Server.MapPath("~/output.pdf");

            using (FileStream fs = new FileStream(sPath, FileMode.Create))
            {

                string sFileName = Session["rptName"] == null ? "output" : Session["rptName"].ToString();
                string sFileType = Request.QueryString["filetype"].isEmpty() == null ? "pdf" : Request.QueryString["filetype"];

                //Extension manipulation
                string sFileExtension = "pdf";

                switch (sFileType.ToLower())
                {
                    case "excel":
                        sFileExtension = "xls";
                        break;
                    default:
                        sFileExtension = "pdf";
                        break;
                }

                //end 
                rpt.render(sFileType, fs);
                sFileName += string.Format(".{0}", sFileExtension);
                return File(g.ConvertStreamToByteArray(fs), "application/unknown", sFileName);
            }
        }


        public JsonResult setExcelForDownload(FormCollection frm)
        {
            //string sModuleName = Request.QueryString["ModuleName"];
            //string sSubModuleName = Request.QueryString["SubModuleName"];

            //var cmd = new clsCmd();
            //addParamFromPost(cmd, frm);
            //var t = _oBL.getExcelData(sModuleName, sSubModuleName, cmd);
            //Session["excel_data"] = t;
            //return Json(new { msg = "" });

            return null;
        }

        public FileResult DownloadExcel()
        {
            //var t = Session["excel_data"] as System.Data.DataTable;
            //var f = _oBL.ConvertToExcelFile(t);
            //return File(f.Data, "application/unknown", "data.xls");

            return null;
        }



        public clsMsg getFileFromQueryString(System.Collections.Specialized.NameValueCollection qs)
        {

            string sPath = qs["path"];

            string[] sKeys = qs.AllKeys;
            var cmd = new clsCmd();

            foreach (string sKey in sKeys)
            {
                if (sKey.isEmpty() == false && sKey.ToUpper() != "PATH")
                {
                    cmd.setValue(sKey, qs[sKey]);
                }
            }

            var msg = webUtil.tier.getFileContent(sPath, cmd);
            return msg;


        }

        public ActionResult getFileContent()
        {
            var oFile = getFileFromQueryString(Request.QueryString).Obj as FileData;

            //if (oFile != null && oFile.FileName.isEmpty() == false)
            //    oFile.ContentType = getContentType2(oFile.FileName);

            return File(oFile.Data, oFile.ContentType);

            
        }


        [HttpPost]
        public ActionResult setFileForDownload(FormCollection frm)
        {


            string sPath = Request.QueryString["path"];

            var cmd = new clsCmd();
            webUtil.addParamFromPost(cmd, frm);


            if (!string.IsNullOrWhiteSpace(sPath))
            {
                try
                {
                    var msg = webUtil.tier.getFileContent(sPath, cmd);

                    if (msg.Validated)
                    {
                        Session["File"] = msg.Obj as FileData;
                        return Json(new { msg = "", data = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { msg = msg.Message, data = "" }, JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception ex)
                {
                    return Json(new { msg = ex.Message, data = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                object n = null;
                return Json(new { msg = "You have not specified [Report path]", data = n }, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult DownloadFile()
        {
            FileData oFile = Session["File"] as FileData;
            return File(oFile.Data, "application/unknown", oFile.FileName);
        }


        [NonAction]
        private string getContentType2(string sFilePath)
        {
            string sExtension = System.IO.Path.GetExtension(sFilePath);

            string sContentType = "";

            switch (sExtension.ToLower())
            {
                case ".js":
                    sContentType = "text/javascript";
                    break;
                case ".html":
                case ".htm":
                    sContentType = "text/html";
                    break;
                case ".pdf":
                    sContentType = "application/pdf";
                    break;
                case ".css":
                    sContentType = "text/css";
                    break;
                case ".xml":
                    sContentType = "text/xml";
                    break;
                case ".jpg":
                case ".gif":
                    sContentType = "image/*";
                    break;

                default:
                    sContentType = "application/unknown";
                    break;
            }

            return sContentType;
        }

        [NonAction]
        private ActionResult getContentType(string sFilePath)
        {
            string sExtension = System.IO.Path.GetExtension(sFilePath);

            string sContentType = "";
            switch (sExtension.ToLower())
            {
                case ".js":
                    sContentType = "text/javascript";
                    break;
                case ".html":
                case ".htm":
                    sContentType = "text/html";
                    break;
                case ".pdf":
                    sContentType = "application/pdf";
                    break;
                case ".css":
                    sContentType = "text/css";
                    break;
                case ".xml":
                    sContentType = "text/xml";
                    break;
                case ".jpg":
                case ".gif":
                    sContentType = "image/*";
                    break;

                default:
                    sContentType = "application/unknown";
                    break;
            }


            switch (sExtension.ToLower())
            {
                case ".pdf":
                    return File(System.IO.File.ReadAllBytes(sFilePath), sContentType);
                case ".gif":
                case ".jpg":
                    return File(System.IO.File.ReadAllBytes(sFilePath), sContentType, "test.gif");
                default:
                    return Content(System.IO.File.ReadAllText(sFilePath), sContentType);
            }

        }

        public ActionResult appServiceContent()
        {
            string qPath = Request.QueryString["path"];
            qPath = qPath.Replace("/", "\\");
            string sPath = ui.appServicePath + qPath;
            return getContentType(sPath);
        }


    }
}