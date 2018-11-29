using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace winui
{
    public partial class frmLogin : frmBase
    {

        public frmLogin() 
        {
            InitializeComponent();
        }
        public clsMsg result;
        
        private void frmLogin_Load(object sender, EventArgs e)
        {
            result = g.msg("You are unable to logged in !");
        }
        

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var cmd = new clsCmd();
            cmd.setValue("userid", txtUserID.Text);
            cmd.setValue("pwd", txtPwd.Text);
            var msg = _Tier.exec("login", cmd);

            if (msg.Validated )
            {
                result = msg;

                this.Close();
            }
            else
                ui.warn(msg.Message);

        }

    }
}
