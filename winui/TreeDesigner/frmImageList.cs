using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DAL;
namespace winui.TreeDesigner
{


    internal partial class frmImageList : Form
    {

        clsImageList oImageList = new clsImageList();

        public frmImageList()
        {
            InitializeComponent();
        }
        

       

        private void frmImageList_Load(object sender, EventArgs e)
        {

            //oImageList.loadXml("D:\\imageList.xml");

            grdImageList.AutoGenerateColumns = false;
            grdImageList.DataSource = oImageList.getData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            oImageList.getInXml().Save("D:\\imageList.xml");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            oImageList.addImage(txtFile.Text
                , txtImageKey.Text);
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFile.Text = openFileDialog1.FileName;
            }
        }

    }
}
