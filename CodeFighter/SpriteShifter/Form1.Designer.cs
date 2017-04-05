namespace SpriteShifter
{
    partial class Form1
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
            this.ofdInput = new System.Windows.Forms.OpenFileDialog();
            this.sfdOutput = new System.Windows.Forms.SaveFileDialog();
            this.tbxSelectedFile = new System.Windows.Forms.TextBox();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.tbxInputSizeX = new System.Windows.Forms.TextBox();
            this.lblInputSz = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxInputSizeY = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxOutputSizeY = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbxOutputSizeX = new System.Windows.Forms.TextBox();
            this.btnConvert = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ddlInputFacing = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.ddlFacing = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cbxResize = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnAssemble = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // ofdInput
            // 
            this.ofdInput.DefaultExt = "png";
            this.ofdInput.FileName = "sprite";
            this.ofdInput.Multiselect = true;
            this.ofdInput.Title = "Open Image";
            // 
            // sfdOutput
            // 
            this.sfdOutput.DefaultExt = "png";
            this.sfdOutput.Title = "Save As";
            // 
            // tbxSelectedFile
            // 
            this.tbxSelectedFile.Location = new System.Drawing.Point(6, 52);
            this.tbxSelectedFile.Multiline = true;
            this.tbxSelectedFile.Name = "tbxSelectedFile";
            this.tbxSelectedFile.ReadOnly = true;
            this.tbxSelectedFile.Size = new System.Drawing.Size(235, 49);
            this.tbxSelectedFile.TabIndex = 1;
            this.tbxSelectedFile.TabStop = false;
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(6, 19);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(75, 23);
            this.btnSelectFile.TabIndex = 0;
            this.btnSelectFile.Text = "Select File";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // tbxInputSizeX
            // 
            this.tbxInputSizeX.Location = new System.Drawing.Point(125, 26);
            this.tbxInputSizeX.Name = "tbxInputSizeX";
            this.tbxInputSizeX.Size = new System.Drawing.Size(42, 20);
            this.tbxInputSizeX.TabIndex = 2;
            // 
            // lblInputSz
            // 
            this.lblInputSz.AutoSize = true;
            this.lblInputSz.Location = new System.Drawing.Point(6, 29);
            this.lblInputSz.Name = "lblInputSz";
            this.lblInputSz.Size = new System.Drawing.Size(84, 13);
            this.lblInputSz.TabIndex = 3;
            this.lblInputSz.Text = "Input Sprite Size";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(107, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(173, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Y";
            // 
            // tbxInputSizeY
            // 
            this.tbxInputSizeY.Location = new System.Drawing.Point(191, 26);
            this.tbxInputSizeY.Name = "tbxInputSizeY";
            this.tbxInputSizeY.Size = new System.Drawing.Size(43, 20);
            this.tbxInputSizeY.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(350, 236);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Y";
            // 
            // tbxOutputSizeY
            // 
            this.tbxOutputSizeY.Location = new System.Drawing.Point(370, 233);
            this.tbxOutputSizeY.Name = "tbxOutputSizeY";
            this.tbxOutputSizeY.Size = new System.Drawing.Size(43, 20);
            this.tbxOutputSizeY.TabIndex = 10;
            this.tbxOutputSizeY.Text = "32";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(283, 236);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "X";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(182, 236);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Output Sprite Size:";
            // 
            // tbxOutputSizeX
            // 
            this.tbxOutputSizeX.Location = new System.Drawing.Point(302, 233);
            this.tbxOutputSizeX.Name = "tbxOutputSizeX";
            this.tbxOutputSizeX.Size = new System.Drawing.Size(42, 20);
            this.tbxOutputSizeX.TabIndex = 7;
            this.tbxOutputSizeX.Text = "32";
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(3, 78);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(75, 23);
            this.btnConvert.TabIndex = 12;
            this.btnConvert.Text = "Resize";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(419, 236);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(52, 13);
            this.lblMessage.TabIndex = 13;
            this.lblMessage.Text = "message!";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblInputSz);
            this.groupBox1.Controls.Add(this.tbxInputSizeX);
            this.groupBox1.Controls.Add(this.btnConvert);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbxInputSizeY);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(265, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(260, 107);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Resize Horizontal Spritesheet";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ddlInputFacing);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.ddlFacing);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(265, 125);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(260, 105);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Create 8-direction rotation";
            // 
            // ddlInputFacing
            // 
            this.ddlInputFacing.FormattingEnabled = true;
            this.ddlInputFacing.Location = new System.Drawing.Point(122, 19);
            this.ddlInputFacing.Name = "ddlInputFacing";
            this.ddlInputFacing.Size = new System.Drawing.Size(121, 21);
            this.ddlInputFacing.TabIndex = 4;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(66, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "Input Facing";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 74);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Create";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ddlFacing
            // 
            this.ddlFacing.FormattingEnabled = true;
            this.ddlFacing.Items.AddRange(new object[] {
            "North",
            "North East",
            "East",
            "South East",
            "South",
            "South West",
            "West",
            "North West"});
            this.ddlFacing.Location = new System.Drawing.Point(122, 46);
            this.ddlFacing.Name = "ddlFacing";
            this.ddlFacing.Size = new System.Drawing.Size(121, 21);
            this.ddlFacing.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Starting Facing";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 236);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(149, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Resize to Output Dimensions?";
            // 
            // cbxResize
            // 
            this.cbxResize.AutoSize = true;
            this.cbxResize.Checked = true;
            this.cbxResize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxResize.Location = new System.Drawing.Point(161, 236);
            this.cbxResize.Name = "cbxResize";
            this.cbxResize.Size = new System.Drawing.Size(15, 14);
            this.cbxResize.TabIndex = 4;
            this.cbxResize.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnAssemble);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Location = new System.Drawing.Point(10, 125);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(249, 105);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Assemble Spritesheet";
            // 
            // btnAssemble
            // 
            this.btnAssemble.Location = new System.Drawing.Point(6, 67);
            this.btnAssemble.Name = "btnAssemble";
            this.btnAssemble.Size = new System.Drawing.Size(75, 23);
            this.btnAssemble.TabIndex = 1;
            this.btnAssemble.Text = "Assemble";
            this.btnAssemble.UseVisualStyleBackColor = true;
            this.btnAssemble.Click += new System.EventHandler(this.btnAssemble_Click);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(10, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(222, 36);
            this.label8.TabIndex = 0;
            this.label8.Text = "Select multiple files in the picker above.      Images will be sorted alphabetica" +
    "lly.";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnSelectFile);
            this.groupBox4.Controls.Add(this.tbxSelectedFile);
            this.groupBox4.Location = new System.Drawing.Point(12, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(247, 107);
            this.groupBox4.TabIndex = 17;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Select File(s)";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 261);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.cbxResize);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbxOutputSizeY);
            this.Controls.Add(this.tbxOutputSizeX);
            this.Controls.Add(this.label4);
            this.Name = "Form1";
            this.Text = "SpriteShifter";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog ofdInput;
        private System.Windows.Forms.SaveFileDialog sfdOutput;
        private System.Windows.Forms.TextBox tbxSelectedFile;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.TextBox tbxInputSizeX;
        private System.Windows.Forms.Label lblInputSz;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxInputSizeY;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbxOutputSizeY;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbxOutputSizeX;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox ddlFacing;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox cbxResize;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnAssemble;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox ddlInputFacing;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox4;
    }
}

