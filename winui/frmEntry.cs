using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;

namespace winui
{
    public class frmEntry : winui.frmBase
    {

        protected Binder _binder = new Binder();

        protected int iID = 0;
        protected string sPrimaryKeyField = "";
        protected string sPath_searchDropDown = "";
        protected string sPath_get = "";
        protected string sPath_save = "";
        protected string sPath_delete = "";



        protected virtual void Delete()
        {
            if (iID == 0)
            {
                ui.alert("Please select the record, and try again !");
            }

            if (ui.confirm("Are you sure want delete this record ? "))
            {
                var cmd = new clsCmd();
                cmd.setValue(sPrimaryKeyField, iID);
                var msg = _Tier.exec(sPath_delete, cmd);

                if (msg.Validated)
                {
                    ui.alert("Record deleted successfully.");
                    clearTxt();
                }
                else
                    ui.warn(msg.Message);


            }
        }

        protected virtual void Search()
        {
            var tSrc = _Tier.getDropDownData(sPath_searchDropDown,new clsCmd()).Obj as DataTable;
            var src = ui.showSearchBox(tSrc, "Search Item", null);

            if (src.validated)
            {
                fillMe(src.getID());
            }
        }


        protected virtual void clearTxt()
        {
            _binder.clearText();
            iID = 0;
        }


        protected clsCmd getFieldCmd()
        {

            var cmd = _binder.getValues();
            cmd.setValue(sPrimaryKeyField, iID);

            return cmd;
        }


        protected virtual clsCmd getCmd()
        { 
            var cmd = _binder.getValues();
            cmd.setValue(sPrimaryKeyField, iID);

            return cmd;
        }
        protected virtual void Save()
        {

            var cmd = getCmd();
            var msg = _Tier.exec(sPath_save, cmd);

            if (msg.Validated)
            {
                ui.alert("Record saved successfully.");
            }
            else
            {
                ui.warn(msg.Message);
            }
        }



        public virtual void fillMe(int ID)
        {

            iID = ID;

            if (iID == 0)
            {
                clearTxt();
            }

            var cmd = new clsCmd();
            cmd.setValue(sPrimaryKeyField, iID);

            var t = _Tier.getData(sPath_get, cmd).Obj as DataTable;
            if (t.Rows.Count > 0)
                _binder.setValue(t.Rows[0]);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frmEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(652, 286);
            this.Name = "frmEntry";
            this.Load += new System.EventHandler(this.frmEntry_Load);
            this.ResumeLayout(false);

        }

        private void frmEntry_Load(object sender, EventArgs e)
        {

        }


    }
}
