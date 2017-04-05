using ImageProcessor;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace SpriteShifter
{
    enum ImageFacing
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    }


    class Rotator
    {
        Label message;
        SaveFileDialog sfdOutput;

        public Rotator(ref Label message, ref SaveFileDialog sfd)
        {
            this.message = message;
            this.sfdOutput = sfd;
        }

        internal void CreateRotatedSpriteSheet(string selectedFilePath, ImageFacing startingFacing, ImageFacing inputFacing, bool resize, int outX = 32, int outY = 32)
        {
            Bitmap source = null;
            try
            {
                source = new Bitmap(selectedFilePath);
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Selected file is not an image!", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // resize to square
            source = padImage(source);

            Bitmap target = new Bitmap(source.Width * 8, source.Height);
            target.MakeTransparent(target.GetPixel(0, 0));

            int degreesRotation = 45 * (startingFacing - inputFacing);

            using (ImageFactory imgFac = new ImageFactory())
            {
                for (int facingCount = 0; facingCount < 8; facingCount++)
                {
                    message.Text = string.Format("Updating Facing {0} of 8", facingCount + 1);
                    message.Refresh();

                    using (Stream ms = new MemoryStream())
                    {

                        // rotate image
                        imgFac.Load(source).Rotate(degreesRotation).Save(ms);
                        Bitmap currentImage = new Bitmap(ms);
                        currentImage.MakeTransparent();

                        // add the image at offset
                        int x = source.Width * facingCount;
                        int y = 0;

                        target = ImageHelper.Superimpose(target, padImage(currentImage, target.Height), new Point(x, y));

                        // change rotation
                        degreesRotation += 45;

                    }
                }
            }


            // check for resize
            if (resize)
            {
                Resizer r = new Resizer(ref message, ref sfdOutput);
                r.ResizeSpritesheet(target, source.Width, source.Height, outX, outY);
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

        Bitmap padImage(Image originalImage)
        {
            int largestDimension = Math.Max(originalImage.Height, originalImage.Width);
            largestDimension = Convert.ToInt32(Math.Round(Math.Sqrt(largestDimension * largestDimension + largestDimension * largestDimension)));
            return padImage(originalImage, largestDimension);
        }

        Bitmap padImage(Image originalImage, int targetDimension)
        {
            Size squareSize = new Size(targetDimension, targetDimension);
            return padImage(originalImage, squareSize);
        }

        Bitmap padImage(Image originalImage, Size squareSize)
        {

            Bitmap squareImage = new Bitmap(squareSize.Width, squareSize.Height);
            using (Graphics graphics = Graphics.FromImage(squareImage))
            {
                graphics.FillRectangle(Brushes.White, 0, 0, squareSize.Width, squareSize.Height);
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;

                graphics.DrawImage(originalImage, (squareSize.Width / 2) - (originalImage.Width / 2), (squareSize.Height / 2) - (originalImage.Height / 2), originalImage.Width, originalImage.Height);
            }
            return squareImage;
        }
    }
}
