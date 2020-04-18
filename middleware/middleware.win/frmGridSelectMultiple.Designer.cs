namespace middleware.win
{
    partial class frmGridSelectMultiple 
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.pnlActionButtons = new System.Windows.Forms.Panel();
            this.pnlCRUDButton = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.drpSrcField = new System.Windows.Forms.ComboBox();
            this.txtSrc = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel5 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new winui.MyGrid();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.pnlRight.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.pnlActionButtons);
            this.pnlRight.Controls.Add(this.pnlCRUDButton);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Location = new System.Drawing.Point(764, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(3);
            this.pnlRight.Size = new System.Drawing.Size(126, 414);
            this.pnlRight.TabIndex = 0;
            this.pnlRight.Visible = false;
            // 
            // pnlActionButtons
            // 
            this.pnlActionButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlActionButtons.Location = new System.Drawing.Point(3, 165);
            this.pnlActionButtons.Name = "pnlActionButtons";
            this.pnlActionButtons.Size = new System.Drawing.Size(120, 246);
            this.pnlActionButtons.TabIndex = 1;
            // 
            // pnlCRUDButton
            // 
            this.pnlCRUDButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCRUDButton.Location = new System.Drawing.Point(3, 3);
            this.pnlCRUDButton.Name = "pnlCRUDButton";
            this.pnlCRUDButton.Size = new System.Drawing.Size(120, 162);
            this.pnlCRUDButton.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.drpSrcField);
            this.panel2.Controls.Add(this.txtSrc);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(764, 30);
            this.panel2.TabIndex = 1;
            // 
            // drpSrcField
            // 
            this.drpSrcField.FormattingEnabled = true;
            this.drpSrcField.Location = new System.Drawing.Point(8, 6);
            this.drpSrcField.Name = "drpSrcField";
            this.drpSrcField.Size = new System.Drawing.Size(166, 21);
            this.drpSrcField.TabIndex = 1;
            // 
            // txtSrc
            // 
            this.txtSrc.Location = new System.Drawing.Point(180, 6);
            this.txtSrc.Name = "txtSrc";
            this.txtSrc.Size = new System.Drawing.Size(339, 21);
            this.txtSrc.TabIndex = 0;
            this.txtSrc.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSrc_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(337, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = " - Press down Arrow to Enter in Grid, Press space In Grid to add Item";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 377);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(764, 37);
            this.panel3.TabIndex = 2;
            this.panel3.Visible = false;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 30);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(10, 347);
            this.panel4.TabIndex = 3;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(761, 30);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 347);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.dataGridView1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(10, 30);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(751, 347);
            this.panel5.TabIndex = 5;
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.DataSource = this.bindingSource1;
            this.dataGridView1.Location = new System.Drawing.Point(6, 6);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(742, 335);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
            // 
            // frmGridSelectMultiple
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 414);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pnlRight);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmGridSelectMultiple";
            this.Text = "Grid Test";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnlRight.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel5;
        winui.MyGrid dataGridView1;
        private System.Windows.Forms.TextBox txtSrc;
        private System.Windows.Forms.ComboBox drpSrcField;
        private System.Windows.Forms.Panel pnlActionButtons;
        private System.Windows.Forms.Panel pnlCRUDButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.BindingSource bindingSource1;

    }
}

