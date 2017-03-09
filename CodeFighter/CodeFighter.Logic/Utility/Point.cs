using System.Globalization;

namespace CodeFighter.Logic.Utility
{
    public struct Point
    {
        public static readonly Point Empty = new Point();

        #region variables
        private int x;

        public int X
        {
            get { return x; }
            set { x = value; }
        }
        private int y;

        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        #endregion

        #region constructor
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        #endregion

        #region operators

        public static bool operator ==(Point left, Point right)
        {
            return left.X == right.X && left.Y == right.Y;
        }

        public static bool operator !=(Point left, Point right)
        {
            return !(left == right);
        }

        public static Point operator +(Point pt, Size sz)
        {
            return Add(pt, sz);
        }
  
        public static Point operator -(Point pt, Size sz)
        {
            return Subtract(pt, sz);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Point)) return false;
            Point comp = (Point)obj;
            return comp.X == this.X && comp.Y == this.Y;
        }

        public override int GetHashCode()
        {
            return unchecked(x ^ y);
        }
        #endregion

        #region methods
        public void Offset(int dx, int dy)
        {
            X += dx;
            Y += dy;
        }
        public void Offset(Point p)
        {
            Offset(p.X, p.Y);
        }

        public override string ToString()
        {
            return "{X=" + X.ToString(CultureInfo.CurrentCulture) + ",Y=" + Y.ToString(CultureInfo.CurrentCulture) + "}";
        }

        public static Point Add(Point pt, Size sz)
        {
            return new Point(pt.X + sz.Width, pt.Y + sz.Height);
        }

        public static Point Subtract(Point pt, Size sz)
        {
            return new Point(pt.X - sz.Width, pt.Y - sz.Height);
        }
        #endregion


    }
}
