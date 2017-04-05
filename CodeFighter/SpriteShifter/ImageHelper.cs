using System.Drawing;
using System.Drawing.Drawing2D;

namespace SpriteShifter
{
    static class ImageHelper
    {
        internal static Bitmap Superimpose(Bitmap target, Bitmap source, Point position)
        {
            using (Graphics g = Graphics.FromImage(target))
            {
                g.CompositingMode = CompositingMode.SourceOver;
                source.MakeTransparent();
                g.DrawImage(source, position);

                return target;
            }
        }
    }
}
