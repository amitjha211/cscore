using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace winui.appConfig
{
    public partial class frmConnectionString : Form
    {
        public frmConnectionString()
        {
            InitializeComponent();
        }


        clsAppConfig oAppConfig = new clsAppConfig(ui.getAppConfigXmlPath());



        
        private void btnSave_Click(object sender, EventArgs e)
        {

            oAppConfig.defaultConnectionString = textBox1.Text;
            oAppConfig.save();
            ui.alert("Connection string information saved successfully");
            
        }

        private void frmConnectionString_Load(object sender, EventArgs e)
        {
            textBox1.Text = oAppConfig.defaultConnectionString;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
