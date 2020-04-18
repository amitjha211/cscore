using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyWinControls
{
    public partial class ucDuration : UserControl
    {
        public ucDuration()
        {
            InitializeComponent();
        }

        public string getStartDate()
        {
            return dtpStart.Value.ToString("dd/MMM/yyyy");
        }

        public string getEndDate()
        {
            return dtpStart.Value.ToString("dd/MMM/yyyy");
        }
    }
}
