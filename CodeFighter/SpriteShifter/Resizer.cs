using ImageProcessor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SpriteShifter
{
    class Resizer
    {
        Label message;
        SaveFileDialog sfdOutput;

        public Resizer(ref Label message, ref SaveFileDialog sfd)
        {
            this.message = message;
            this.sfdOutput = sfd;
        }

        public void ResizeSpritesheet(string selectedFilePath, int inX, int inY, int outX, int outY)
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
            ResizeSpritesheet(source, inX, inY, outX, outY);
        }
        public void ResizeSpritesheet(Bitmap source, int inX, int inY, int outX, int outY)
        {

            Size sourceSize = new Size(inX, inY);
            Size targetSize = new Size(outX, outY);

            // get count of frames
            int countOfFrames = Convert.ToInt32(Math.Truncate(Convert.ToDouble(source.Width) / Convert.ToDouble(sourceSize.Width)));

            // create target bitmap
            Bitmap target = new Bitmap(targetSize.Width * countOfFrames, targetSize.Height);

            // get scale
            double scale = Convert.ToDouble(sourceSize.Width) / Convert.ToDouble(targetSize.Width);

            // for each frame
            for (int frame = 0; frame < countOfFrames; frame++)
            {
                message.Text = "Updating Frame #" + (frame + 1).ToString() + " of " + countOfFrames.ToString();
                message.Refresh();

                int sourceXOffset = frame * sourceSize.Width;
                int targetXOffset = frame * targetSize.Width;

                convertAllPixels(source, targetSize, target, scale, sourceXOffset, targetXOffset);
            }
            // if the input size is smaller than the output, apply gaussian blur to de-pixelate
            Stream resultStream;
            bool useStream = applyGaussianBlur(inX, inY, outX, outY, target, scale, out resultStream);
            // show the save file dialog and then save to the resulting location.
            saveConversion(target, resultStream, useStream);


        }

        private static void convertAllPixels(Bitmap source, Size targetSize, Bitmap target, double scale, int sourceXOffset, int targetXOffset)
        {
            // for each pixel of target bitmap by x/y coordinates
            for (int x = 0; x < targetSize.Width; x++)
                for (int y = 0; y < targetSize.Height; y++)
                {
                    convertByPixel(source, target, scale, sourceXOffset, targetXOffset, x, y);
                }
        }

        private bool applyGaussianBlur(int inX, int inY, int outX, int outY, Bitmap target, double scale, out Stream resultStream)
        {
            resultStream = new MemoryStream();

            if (inX < outX || inY < outY)
            {
                message.Text = "Performing Gaussian Blur...";
                message.Refresh();
                using (ImageFactory imgFac = new ImageFactory())
                {
                    imgFac.Load(target).GaussianBlur(Convert.ToInt32(Math.Round(1 / scale))).ReplaceColor(Color.Black, Color.Transparent, 16).Save(resultStream);
                }
                return true;
            }
            return false;
        }

        private void saveConversion(Bitmap target, Stream resultStream, bool useStream)
        {
            if (sfdOutput.ShowDialog() == DialogResult.OK)
            {
                if (useStream)
                {
                    using (FileStream file = new FileStream(sfdOutput.FileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                    {
                        byte[] bytes = new byte[resultStream.Length];
                        resultStream.Read(bytes, 0, (int)resultStream.Length);
                        file.Write(bytes, 0, bytes.Length);
                        resultStream.Close();
                    }
                }
                else
                {
                    target.Save(sfdOutput.FileName, System.Drawing.Imaging.ImageFormat.Png);
                }
                message.Text = "Save Complete!";
            }
            else
                message.Text = "Conversion Discarded.";
        }

        private static void convertByPixel(Bitmap source, Bitmap target, double scale, int sourceXOffset, int targetXOffset, int x, int y)
        {
            // get position in source (position * scale)
            Point positionInSource = new Point(sourceXOffset + Convert.ToInt32(Math.Round(x * scale)), Convert.ToInt32(Math.Round(y * scale)));

            // get surrounding <scale> pixels from source
            List<Color> colors = new List<Color>();
            for (int i = 0; i < scale; i++)
                for (int j = 0; j < scale; j++)
                {
                    if (positionInSource.X + i < source.Width && positionInSource.Y + j < source.Height)
                        colors.Add(source.GetPixel(positionInSource.X + i, positionInSource.Y + j));
                }

            // average colormap for pixels
            Color finalColor = getAverageColor(colors);

            // apply to target pixel
            target.SetPixel(targetXOffset + x, y, finalColor);
        }

        private static Color getAverageColor(List<Color> colors)
        {
            Color finalColor = Color.Transparent;
            if (colors.Count > 0)
            {
                List<CIELab> labs = new List<CIELab>();

                foreach (Color c in colors)
                    labs.Add(ColorHelper.RGBtoLab(c.R, c.G, c.B));

                double avgL = labs.Average(z => z.L);
                double avgA = labs.Average(z => z.A);
                double avgB = labs.Average(z => z.B);
                double avgT = colors.Average(z => z.A);

                RGB average = ColorHelper.LabtoRGB(avgL, avgA, avgB);

                finalColor = Color.FromArgb(Convert.ToInt32(Math.Round(avgT)), average.Red, average.Green, average.Blue);
            }
            else
            {
                if (colors.Count > 0)
                    finalColor = colors.First();
            }

            return finalColor;
        }
    }
}
