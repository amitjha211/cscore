using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace middleware.win
{
    public partial class frmBase : Form
    {
        public clsAppService2 appService;
        public clsFormService formService;
        public frmBase()
        {
            InitializeComponent();
        }

        private void frmBase_Load(object sender, EventArgs e)
        {

        }
    }
}
