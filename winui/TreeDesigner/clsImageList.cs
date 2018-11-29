using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace winui.TreeDesigner
{

    public class clsImageList
    {

        DataTable t = new DataTable();
        public clsImageList()
        {

            t.Columns.Add("image_data",typeof(byte[]));
            t.Columns.Add("image_key", typeof(string));
            t.Columns.Add("image_file_name", typeof(string));
        }

        public void loadXml(XElement elMenu)
        {
            t.Rows.Clear();
            
            foreach (XElement el in elMenu.Elements("img"))
            {
                var r = t.NewRow();
                r["image_key"] = el.Attribute("key").Value;
                r["image_file_name"] = el.Attribute("fileName").Value;
                r["image_data"] = Convert.FromBase64String(el.Value);
                t.Rows.Add(r);
            }

        }

        public void loadXml(string sPath)
        {
            XDocument xDoc = XDocument.Load(sPath);
            var el = xDoc.Element("imageList");
            loadXml(el);
        }

        public clsMsg addImage(string sPath, string skey)
        {

            if (skey.isEmpty()) return g.msg("Please specify key");
            if (sPath.isEmpty()) return g.msg("Please specify File path");

            ///////////////////////

            string sImageFileName = Path.GetFileName(sPath);
            var data  = File.ReadAllBytes(sPath);
            ////////////

            var r = t.NewRow();
            r["image_data"] = data;
            r["image_file_name"] = sImageFileName;
            r["image_key"] = skey;


            t.Rows.Add(r);

            return g.msg("");
        }

        public DataTable getData()
        {
            return t;
        }

        public XElement getInXml()
        {
            XElement elImageList = new XElement("imageList");

            foreach (DataRow r in t.Rows)
            {
                var el = new XElement("img");
                el.SetAttributeValue("key", r["image_key"].ToString());
                el.SetAttributeValue("fileName",r["image_key"].ToString());

                byte[] data = (byte[])r["image_data"];
                el.SetValue(Convert.ToBase64String(data));
                elImageList.Add(el);
            }

            
            return elImageList;

        }
    }
}
