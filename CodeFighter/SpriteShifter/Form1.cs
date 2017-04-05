using System;
using System.Windows.Forms;

namespace SpriteShifter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ddlFacing.DataSource = Enum.GetValues(typeof(ImageFacing));
            ddlFacing.SelectedIndex = 6;
            ddlInputFacing.DataSource = Enum.GetValues(typeof(ImageFacing));
            ddlInputFacing.SelectedIndex = 0;
            lblMessage.Text = string.Empty;

        }

        string[] selectedFilePath = new string[0];

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            if (ofdInput.ShowDialog() == DialogResult.OK)
            {
                selectedFilePath = ofdInput.FileNames;
                tbxSelectedFile.Text = string.Join(","+Environment.NewLine, selectedFilePath);
            }

        }



        private void btnConvert_Click(object sender, EventArgs e)
        {
            if(selectedFilePath.Length==0)
            {
                MessageBox.Show("You must select a file first!", "No File(s) Selected", MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(tbxInputSizeX.Text) || string.IsNullOrEmpty(tbxInputSizeY.Text))
            {
                MessageBox.Show("You must specify input X & Y first!", "No Input Sizes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(tbxOutputSizeX.Text) || string.IsNullOrEmpty(tbxOutputSizeY.Text))
            {
                MessageBox.Show("You must specify output X & Y first!", "No Output Sizes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int inX = 0;
            int inY = 0;
            int outX = 0;
            int outY = 0;
            if (int.TryParse(tbxInputSizeX.Text, out inX) && int.TryParse(tbxInputSizeY.Text, out inY)
                && int.TryParse(tbxOutputSizeX.Text, out outX) && int.TryParse(tbxOutputSizeY.Text, out outY))
            {
                Resizer r = new Resizer(ref lblMessage, ref sfdOutput);
                r.ResizeSpritesheet(selectedFilePath[0], inX, inY, outX, outY);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (selectedFilePath.Length == 0)
            {
                MessageBox.Show("You must select a file first!", "No File(s) Selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            

            ImageFacing startingFacing = (ImageFacing)ddlFacing.SelectedItem;
            ImageFacing inputFacing = (ImageFacing)ddlInputFacing.SelectedIndex;
            Rotator r = new Rotator(ref lblMessage, ref sfdOutput);

            if (cbxResize.Checked)
            {
                if (string.IsNullOrEmpty(tbxOutputSizeX.Text) || string.IsNullOrEmpty(tbxOutputSizeY.Text))
                {
                    MessageBox.Show("You must specify output X & Y first!", "No Output Sizes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                int outX = 0;
                int outY = 0;
                if (int.TryParse(tbxOutputSizeX.Text, out outX) && int.TryParse(tbxOutputSizeY.Text, out outY))
                {
                    r.CreateRotatedSpriteSheet(selectedFilePath[0], startingFacing, inputFacing, true, outX, outY);
                }
            }
            else
            {
                r.CreateRotatedSpriteSheet(selectedFilePath[0], startingFacing, inputFacing, false);
            }


        }

        private void btnAssemble_Click(object sender, EventArgs e)
        {
            if (selectedFilePath.Length == 0)
            {
                MessageBox.Show("You must select a file first!", "No File(s) Selected");
                return;
            }
            Assembler a = new Assembler(ref lblMessage, ref sfdOutput);

            if (cbxResize.Checked)
            {
                if (string.IsNullOrEmpty(tbxOutputSizeX.Text) || string.IsNullOrEmpty(tbxOutputSizeY.Text))
                {
                    MessageBox.Show("You must specify output X & Y first!", "No Output Sizes");
                    return;
                }
                
                int outX = 0;
                int outY = 0;
                if (int.TryParse(tbxOutputSizeX.Text, out outX) && int.TryParse(tbxOutputSizeY.Text, out outY))
                {
                    a.AssembleSpritesheet(selectedFilePath, true, outX, outY);
                }
            }
            else
            {
                a.AssembleSpritesheet(selectedFilePath, false);
            }
        }
    }


}
