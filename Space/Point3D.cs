using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Space
{
    public class Point3D
    {
        public double x, y, z;
        public Point3D(double xx, double yy, double zz)
        {
            x = xx;
            y = yy;
            z = zz;
        }
        public Point3D()
        {
            x = 0;
            y = 0;
            z = 0;
        }
        public Point3D(Point3D p)
        {
            x = p.x;
            y = p.y;
            z = p.z;
        }
        public Point3D normalize()
        {
            double div = Math.Sqrt(x * x + y * y + z * z);
            return new Point3D(x / div, y / div, z / div);
        }
        public Point3D cross(Point3D p2)
        {
            Point3D p1 = this.normalize();
            Point3D p3;
            p3 = new Point3D(0, 0, 0);
            p3.x = p1.y * p2.z - p1.z * p2.y;
            p3.y = p1.z * p2.x - p1.x * p2.z;
            p3.z = p1.x * p2.y - p1.y * p2.x;
            return p3;
        }
        public static Point3D operator +(Point3D one, Point3D two)
        {
            return new Point3D(one.x + two.x, one.y + two.y, one.z + two.z);
        }
        public static Point3D operator -(Point3D one, Point3D two)
        {
            return new Point3D(one.x - two.x, one.y - two.y, one.z - two.z);
        }
        public static Point3D operator *(Point3D one, double two)
        {
            return new Point3D(one.x * two, one.y * two, one.z * two);
        }
        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", trunc(this.x), trunc(this.y), trunc(this.z), trunc(Math.Sqrt(x * x + y * y + z * z)));
        }
        public string trunc(double a)
        {
            string ret = "" + a;
            if (ret.IndexOf('.') == -1) return ret;
            return ret.Substring(0, ret.IndexOf('.') + 3);
        }
    }
}