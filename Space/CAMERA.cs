using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Space
{
    public class CAMERA
    {
        public Point3D pos;
        public Point3D facing;
        public Point3D[] axes;
        public Point3D up;
        public double angleh, anglev;
        public double zoom;        // distance from camera to plane
        public double front, back; // clipping planes
        public short projection;
        public CAMERA()
        {
            pos = new Point3D(0, 0, 0);
            facing = new Point3D(0, 1, 0);
            up = new Point3D(0, 0, 1);
            axes = new Point3D[]
            {
                new Point3D(1,0,0),
                new Point3D(0,1,0),
                new Point3D(0,0,1),
            };
            angleh = 45.0;
            anglev = 45.0;
            zoom = 1.0;
            front = 1.0; 
            back = 1000.0; // viewing range
            projection = 0;

        }
        public Point3D viewVec()
        {
            return new Point3D(axes[1] - pos);
        }
        public Point3D sideVec()
        {
            return new Point3D(axes[0] - pos);
        }
        public Point3D upVec()
        {
            return new Point3D(axes[2] - pos);
        }
        
        public void rotateCamera(double rotx, double rotz, double roty) // x is up/down, z is left/right
        {
            double radrotx = Math.PI * rotx / 180;
            double radrotz = -Math.PI * rotz / 180;
            double radroty = Math.PI * roty / 180;
            double cosx = Math.Cos(radrotx);
            double sinx = Math.Sin(radrotx);
            double cosz = Math.Cos(radrotz);
            double sinz = Math.Sin(radrotz);
            double cosy = Math.Cos(radroty);
            double siny = Math.Cos(radroty);
            facing = facing - pos;
            axes[0] -= pos;
            axes[1] -= pos;
            axes[2] -= pos;
            if (rotx != 0)
            {
                facing = new Point3D(facing.x, facing.y * cosx + facing.z * sinx, facing.y * -1 * sinx + facing.z * cosx); // x
                //orient = new Point3D(orient.x, orient.y * cosx + orient.z * sinx, orient.y * -1 * sinx + orient.z * cosx); // x
                axes[1] = new Point3D(axes[1].x, axes[1].y * cosx + axes[1].z * sinx, axes[1].y * -1 * sinx + axes[1].z * cosx); // x
                axes[2] = new Point3D(axes[2].x, axes[2].y * cosx + axes[2].z * sinx, axes[2].y * -1 * sinx + axes[2].z * cosx); // x
            }
            if (rotz != 0)
            {
                facing = new Point3D(facing.x * cosz + facing.y * -1 * sinz, facing.x * sinz + facing.y * cosz, facing.z); // z

                axes[0] = new Point3D(axes[0].x * cosz + axes[0].y * -1 * sinz, axes[0].x * sinz + axes[0].y * cosz, axes[0].z); // z
                axes[1] = new Point3D(axes[1].x * cosz + axes[1].y * -1 * sinz, axes[1].x * sinz + axes[1].y * cosz, axes[1].z); // z
            }
            if (roty != 0)
            {
                axes[0] = new Point3D(axes[0].x * cosy + axes[0].z * -1 * siny, axes[0].y, axes[0].x * siny + axes[0].z * cosy); // y
                axes[2] = new Point3D(axes[2].x * cosy + axes[2].z * -1 * siny, axes[2].y, axes[2].x * siny + axes[2].z * cosy); // y

            }
            facing = facing + pos;
            
            axes[0].normalize();
            axes[1].normalize();
            axes[2].normalize();
            axes[0] += pos;
            axes[1] += pos;
            axes[2] += pos;
            /*
            orient = orient - pos;
            orient = new Point3D(orient.x, orient.y * cosx + orient.z * sinx, orient.y * -1 * sinx + orient.z * cosx);
            
            
            orient = new Point3D(orient.x * cosz + orient.y * -1 * sinz, orient.x * sinz + orient.y * cosz, orient.z);
            orient = orient + pos;
            */
        }
    }
}