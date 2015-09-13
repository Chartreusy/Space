using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space
{
    public class Figure3D
    {
        public Point3D origin;
        public Point3D[] path;
        public Figure3D() { }
        public Figure3D(Point3D origin, Point3D[] path)
        {
            this.origin = origin;
            this.path = path;    
        }
        public Figure3D(Point3D origin)
        {
            this.origin = origin;
            this.makeRectangle(origin);
        }
        public void makeRectangle(Point3D origin)
        {
            this.origin = origin;
            double neg = -20;
            double pov = 20;
            double near = 100;
            double far = 140;
            path = new Point3D[]
            {   
                new Point3D(neg, near, pov),
                new Point3D(neg + 20, far, pov + 20),
                new Point3D(neg + 20, far, -pov + 20),
                new Point3D(neg + 20, near, -pov + 20),
                new Point3D(neg, near, -pov),
                
                new Point3D(neg, near, pov),
                new Point3D(pov, near, pov),
                new Point3D(pov, near, neg),
                new Point3D(neg, near, neg),
                new Point3D(neg, near, pov),
                
                new Point3D(pov, near, pov),
                new Point3D(pov + 20, far, pov + 20),
                new Point3D(pov + 20, far, -pov + 20),

                new Point3D(pov + 20, far, pov + 20),
                new Point3D(neg + 20, far, pov + 20),
                new Point3D(neg + 20, far, neg + 20),
                new Point3D(pov + 20, far, neg + 20),
                new Point3D(pov + 20, far, -pov + 20),
                new Point3D(pov, near, -pov),
            };
            
            
        }
    }
}
