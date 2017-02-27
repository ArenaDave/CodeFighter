using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFighter.Logic
{
    public struct Size
    {
        public static readonly Size Empty = new Size();
        
        #region variables
        private int width;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        private int height;

        public int Height
        {
            get { return height; }
            set { height = value; }
        }
        #endregion

        #region constructors
        public Size(Point pt)
        {
            width = pt.X;
            height = pt.Y;
        }

        public Size(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
        #endregion

        #region operators
        public static Size operator +(Size sz1, Size sz2)
        {
            return Add(sz1, sz2);
        }

        /// <include file='doc\Size.uex' path='docs/doc[@for="Size.operator-"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Contracts a <see cref='System.Drawing.Size'/> by another <see cref='System.Drawing.Size'/>
        ///       .
        ///    </para>
        /// </devdoc>
        public static Size operator -(Size sz1, Size sz2)
        {
            return Subtract(sz1, sz2);
        }

        /// <include file='doc\Size.uex' path='docs/doc[@for="Size.operator=="]/*' />
        /// <devdoc>
        ///    Tests whether two <see cref='System.Drawing.Size'/> objects
        ///    are identical.
        /// </devdoc>
        public static bool operator ==(Size sz1, Size sz2)
        {
            return sz1.Width == sz2.Width && sz1.Height == sz2.Height;
        }

        /// <include file='doc\Size.uex' path='docs/doc[@for="Size.operator!="]/*' />
        /// <devdoc>
        ///    <para>
        ///       Tests whether two <see cref='System.Drawing.Size'/> objects are different.
        ///    </para>
        /// </devdoc>
        public static bool operator !=(Size sz1, Size sz2)
        {
            return !(sz1 == sz2);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Size))
                return false;

            Size comp = (Size)obj;
            // Note value types can't have derived classes, so we don't need to 
            // check the types of the objects here.  -- Microsoft, 2/21/2001
            return (comp.width == this.width) &&
                   (comp.height == this.height);
        }

        public override int GetHashCode()
        {
            return width ^ height;
        }
        #endregion

        #region methods
        public static Size Add(Size sz1, Size sz2)
        {
            return new Size(sz1.Width + sz2.Width, sz1.Height + sz2.Height);
        }

        public static Size Subtract(Size sz1, Size sz2)
        {
            return new Size(sz1.Width - sz2.Width, sz1.Height - sz2.Height);
        }

        public override string ToString()
        {
            return "{Width=" + width.ToString(CultureInfo.CurrentCulture) + ", Height=" + height.ToString(CultureInfo.CurrentCulture) + "}";
        }
        #endregion
    }
}
