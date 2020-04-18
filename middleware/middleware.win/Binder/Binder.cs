using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace middleware.win
{

    public class clsBinder : List<BinderFieldBase>
    {

        //public List<FileBinderBase> FileList = new List<FileBinderBase>();

        public clsCmd getValues()
        {
            var cmd = new clsCmd();

            foreach (var f in this)
            {
                cmd.setValue(f.Field, f.getValue());
            }

            
            return cmd;
        }

        public void setValuesInRow(DataRow r)
        {
            foreach (var f in this)
            {
                r[f.Field] = f.getValue();
            }
        }



        public BinderText add(TextBox txt, string sField)
        {
            var binder_text = new BinderText(txt, sField);
            this.Add(binder_text);
            return binder_text;
        }

        public BinderCheckbox add(CheckBox chk1, string sField)
        {
            var binder_chk = new BinderCheckbox(chk1, sField);
            this.Add(binder_chk);
            return binder_chk;
        }

        public BinderDropDown add(ComboBox cmb, string sField)
        {
            var binder_Drp = new BinderDropDown(cmb, sField, null);
            this.Add(binder_Drp);
            return binder_Drp;
        }


        public BinderDatePicker add(DateTimePicker dtp, string sField)
        {
            var binder_Dtp = new BinderDatePicker(dtp, sField);
            this.Add(binder_Dtp);
            return binder_Dtp;
        }

        //GvS.Controls.HtmlTextbox 
        //public BinderHtmlEditor add(GvS.Controls.HtmlTextbox dtp, string sField)
        //{
        //    var binder_Html = new BinderHtmlEditor(dtp, sField);
        //    this.Add(binder_Html);
        //    return binder_Html;
        //}



        public void setValue(DataRow r)
        {

            try
            {
                foreach (var f in this)
                    f.setValue(r);
            }
            catch (Exception ex)
            {
                ui.alert(ex.Message);
            }
        }


        public void setControl(Control ControlList)
        {
            setDataField(ControlList);
        }

        public void setDataField(Control ControlList)
        {
            if (ControlList.Controls.Count == 0) return;

            foreach (Control c in ControlList.Controls)
            {
                if (c is TabControl)
                {
                    foreach (TabPage tabPage1 in (c as TabControl).TabPages)
                        if (tabPage1.Controls.Count > 0)
                            setDataField(tabPage1);
                    continue;
                }
                else if (c is GroupBox || c is Panel)
                {
                    if (c.Controls.Count > 0) setDataField(c);
                    continue;
                }

                //////////////////////////////////////////////
                if (c is TextBox || c is ComboBox || c is DateTimePicker || c is CheckBox )
                {
                    string sFieldName = "";

                    if (c is TextBox || c is ComboBox) sFieldName = c.Text;
                    if( (c is DateTimePicker || c is CheckBox || c is ComboBox ) && c.Tag != null) sFieldName = c.Tag.ToString();


                    sFieldName = sFieldName.Replace("{", "");
                    sFieldName = sFieldName.Replace("}", "");

                    if (string.IsNullOrEmpty(sFieldName)) continue;

                    /////////////////////////////////////////////////////////////////////


                    if (c is TextBox)
                        add(c as TextBox, sFieldName);
                    if (c is ComboBox)
                        add(c as ComboBox, sFieldName);
                    if (c is CheckBox)
                        add(c as CheckBox, sFieldName);
                    if (c is DateTimePicker)
                        add(c as DateTimePicker, sFieldName);
                 

                }

                
            }
        }

        public void clearText()
        {
            foreach (var f in this) f.clear();
        }


    }
}
