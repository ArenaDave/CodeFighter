using System;
using System.Drawing;
using System.Windows.Forms;

namespace SpriteShifter
{
    class Assembler
    {
        Label message;
        SaveFileDialog sfdOutput;

        public Assembler(ref Label message, ref SaveFileDialog sfd)
        {
            this.message = message;
            this.sfdOutput = sfd;
        }
        public void AssembleSpritesheet(string[] selectedFilePath, bool resize, int outX = 32, int outY = 32)
        {
            // sort alphabetically
            Array.Sort(selectedFilePath);
            // count of images
            int countOfImages = selectedFilePath.Length;
            int height = 0;
            int width = 0;
            int finalWidth = 0;
            Bitmap target = null;
            try
            {
                // load first image
                using (Bitmap image1 = new Bitmap(selectedFilePath[0]))
                {
                    // get dimensions and multiply by count to get full width
                    height = image1.Height;
                    width = image1.Width;
                    finalWidth = width * countOfImages;
                    // create target bitmap
                    target = new Bitmap(finalWidth, height);
                    target.MakeTransparent();
                    // write the first
                    target = ImageHelper.Superimpose(target, image1, new Point(0, 0));
                }
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Selected file is not an image!", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // for each after first
            for (int i = 1; i < countOfImages; i++)
            {
                // load the image
                try
                {
                    using (Bitmap currentImage = new Bitmap(selectedFilePath[i]))
                    {
                        // write it to the target at offset
                        target = ImageHelper.Superimpose(target, currentImage, new Point(i * width, 0));
                    }
                }
                catch (ArgumentException)
                {
                    MessageBox.Show("Selected file is not an image!", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // check for resize
            if (resize)
            {
                Resizer r = new Resizer(ref message, ref sfdOutput);
                r.ResizeSpritesheet(target, width, height, outX, outY);
            }
            else
            {
                // save file
                if (sfdOutput.ShowDialog() == DialogResult.OK)
                {
                    target.MakeTransparent();
                    target.Save(sfdOutput.FileName, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
        }
    }
}
