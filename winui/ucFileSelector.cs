using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DAL;
namespace winui
{
    public partial class ucFileSelector : UserControl
    {
        public ucFileSelector()
        {
            InitializeComponent();
        }

        public bool showPicture
        {
            get { return pictureBox1.Visible; }
            set { pictureBox1.Visible = value; }
        }

        FileData selectedFile = null;

        public bool isFileSelected()
        {
            return selectedFile == null ? false : true;
        }

        public void setPhoto(FileData filePhoto)
        {
            if ( filePhoto.ContentType.isEmpty()==false )
                pictureBox1.Image = Image.FromStream(g.ConvertBytesToMemoryStream(filePhoto.Data));
        }

        private void btnFindFile_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK
                && System.IO.File.Exists(openFileDialog1.FileName))
            {
                //ui.alert(openFileDialog1.FileName);
                selectedFile = new FileData();
                selectedFile.FieldName = FieldName;
                selectedFile.FileName = "photo." + g.getFileExtension(openFileDialog1.FileName);
                selectedFile.ContentType = "image/jpeg";
                selectedFile.Data = g.ConvertFileToByteArray(openFileDialog1.FileName);
                pictureBox1.Image = Image.FromStream(g.ConvertBytesToMemoryStream(selectedFile.Data));
            }


        }


        public FileData getFileData()
        {
            return selectedFile;
        }

        private void btnDeleteFile_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            selectedFile = null;
        }

        public string FieldName { get; set; }

    }
}
