using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NTier.Request;

namespace winui
{

    public partial class frmBase : Form
    {
        public frmBase()
        {
            InitializeComponent();
        }

        protected iBussinessTier _Tier;

        private void frmBase_Load(object sender, EventArgs e)
        {

        }

        public void setTier(iBussinessTier oTier)
        {
            _Tier = oTier;
        }

    }
}
