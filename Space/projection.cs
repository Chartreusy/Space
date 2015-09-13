using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using System.Math; 
/*
http://www.codeincodeblock.com/2012/03/projecting-3d-world-co-ordinates-into.html
*/
namespace Space
{
    class projection
    {
        private Point3D origin;
        private Point3D e1, e2, n1, n2;
        private CAMERA camera;
        private SCREEN screen;
        private double tanthetah, tanthetav;
        private Point3D basisa, viewVec, basisc;
        private double EPSILON;
        private double DTOR;// 0.01745329252
        public Point2D p1;
        public Point2D p2;
        public projection(CAMERA camera)
        {
            EPSILON = 0.001;
            DTOR = 0.01745329252;
            this.camera = camera;
            screen = new SCREEN();
            origin = new Point3D();
            basisa = new Point3D();
            viewVec = new Point3D();
            basisc = new Point3D();
            p1 = new Point2D();
            p2 = new Point2D();

            e1 = new Point3D();
            e2 = new Point3D();
            n1 = new Point3D();
            n2 = new Point3D();
            if (Trans_Initialise() != true)
            {
                //MessageBox.Show("Error in initializing variable");
            }
        }

        public bool Trans_Initialise()
        {
            /* Is the camera position and view vector coincident ? */
            if (EqualVertex(camera.axes[1], camera.pos))
            {
                return (false);
            }

            /* Is there a legal camera up vector ? */
            if (EqualVertex(camera.up, origin))
            {
                return (false);
            }

            viewVec.x = camera.axes[1].x - camera.pos.x;// view direction
            viewVec.y = camera.axes[1].y - camera.pos.y;
            viewVec.z = camera.axes[1].z - camera.pos.z;
            Normalise(viewVec);

            basisa = CrossProduct(camera.up, viewVec);
            Normalise(basisa);

            /* Are the up vector and view direction colinear */
            if (EqualVertex(basisa, origin))
            {
                return (false);
            }

            basisc = CrossProduct(viewVec, basisa);

            /* Do we have legal camera apertures ? */
            if (camera.angleh < EPSILON || camera.anglev < EPSILON)
            {
                return (false);
            }

            /* Calculate camera aperture statics, note: angles in degrees */
            tanthetah = Math.Tan(camera.angleh * DTOR / 2);
            tanthetav = Math.Tan(camera.anglev * DTOR / 2);

            /* Do we have a legal camera zoom ? */
            if (camera.zoom < EPSILON)
            {
                return (false);
            }

            /* Are the clipping planes legal ? */
            if (camera.front < 0 || camera.back < 0 || camera.back <= camera.front)
            {
                return (false);
            }

            return true;
        }


        public void Trans_World2Eye(Point3D w, Point3D e)
        {
            /* Translate world so that the camera is at the origin */
            w.x -= camera.pos.x;
            w.y -= camera.pos.y;
            w.z -= camera.pos.z;

            /* Convert to eye coordinates using basis vectors */
            e.x = w.x * basisa.x + w.y * basisa.y + w.z * basisa.z;

            e.y = w.x * viewVec.x + w.y * viewVec.y + w.z * viewVec.z;
            e.z = w.x * basisc.x + w.y * basisc.y + w.z * basisc.z;
        }

        public bool Trans_ClipEye(Point3D e1, Point3D e2)
        {
            double mu;

            /* Is the vector totally in front of the front cutting plane ? */
            if (e1.y <= camera.front && e2.y <= camera.front)
            {

                return (false);
            }

            /* Is the vector totally behind the back cutting plane ? */
            if (e1.y >= camera.back && e2.y >= camera.back)
            {

                return (false);
            }

            /* Is the vector partly in front of the front cutting plane ? */
            if ((e1.y < camera.front && e2.y > camera.front) ||
               (e1.y > camera.front && e2.y < camera.front))
            {

                mu = (camera.front - e1.y) / (e2.y - e1.y);
                if (e1.y < camera.front)
                {
                    e1.x = e1.x + mu * (e2.x - e1.x);
                    e1.z = e1.z + mu * (e2.z - e1.z);
                    e1.y = camera.front;
                }
                else
                {
                    e2.x = e1.x + mu * (e2.x - e1.x);
                    e2.z = e1.z + mu * (e2.z - e1.z);
                    e2.y = camera.front;
                }
            }
            /* Is the vector partly behind the back cutting plane ? */
            if ((e1.y < camera.back && e2.y > camera.back) ||
               (e1.y > camera.back && e2.y < camera.back))
            {
                mu = (camera.back - e1.y) / (e2.y - e1.y);
                if (e1.y < camera.back)
                {
                    e2.x = e1.x + mu * (e2.x - e1.x);
                    e2.z = e1.z + mu * (e2.z - e1.z);
                    e2.y = camera.back;
                }
                else
                {
                    e1.x = e1.x + mu * (e2.x - e1.x);
                    e1.z = e1.z + mu * (e2.z - e1.z);
                    e1.y = camera.back;
                }
            }

            return (true);
        }
        public void Trans_Eye2Norm(Point3D e, Point3D n)
        {
            double d;

            if (camera.projection == 0)
            {

                d = camera.zoom / e.y;
                n.x = d * e.x / tanthetah;
                n.y = e.y;
                n.z = d * e.z / tanthetav;

            }
            else
            {

                n.x = camera.zoom * e.x / tanthetah;
                n.y = e.y;
                n.z = camera.zoom * e.z / tanthetav;
            }
        }
        public bool Trans_ClipNorm(Point3D n1, Point3D n2)
        {
            double mu;

            /* Is the line segment totally right of x = 1 ? */
            if (n1.x >= 1 && n2.x >= 1)
                return (false);


            /* Is the line segment totally left of x = -1 ? */
            if (n1.x <= -1 && n2.x <= -1)
                return (false);


            /* Does the vector cross x = 1 ? */
            if ((n1.x > 1 && n2.x < 1) || (n1.x < 1 && n2.x > 1))
            {

                mu = (1 - n1.x) / (n2.x - n1.x);
                if (n1.x < 1)
                {
                    n2.z = n1.z + mu * (n2.z - n1.z);
                    n2.x = 1;
                }
                else
                {
                    n1.z = n1.z + mu * (n2.z - n1.z);
                    n1.x = 1;
                }
            }

            /* Does the vector cross x = -1 ? */
            if ((n1.x < -1 && n2.x > -1) || (n1.x > -1 && n2.x < -1))
            {

                mu = (-1 - n1.x) / (n2.x - n1.x);
                if (n1.x > -1)
                {
                    n2.z = n1.z + mu * (n2.z - n1.z);
                    n2.x = -1;
                }
                else
                {
                    n1.z = n1.z + mu * (n2.z - n1.z);
                    n1.x = -1;
                }
            }

            /* Is the line segment totally above z = 1 ? */
            if (n1.z >= 1 && n2.z >= 1)
                return (false);


            /* Is the line segment totally below z = -1 ? */
            if (n1.z <= -1 && n2.z <= -1)
                return (false);

            /* Does the vector cross z = 1 ? */
            if ((n1.z > 1 && n2.z < 1) || (n1.z < 1 && n2.z > 1))
            {

                mu = (1 - n1.z) / (n2.z - n1.z);
                if (n1.z < 1)
                {
                    n2.x = n1.x + mu * (n2.x - n1.x);
                    n2.z = 1;
                }
                else
                {
                    n1.x = n1.x + mu * (n2.x - n1.x);
                    n1.z = 1;
                }
            }

            /* Does the vector cross z = -1 ? */
            if ((n1.z < -1 && n2.z > -1) || (n1.z > -1 && n2.z < -1))
            {

                mu = (-1 - n1.z) / (n2.z - n1.z);
                if (n1.z > -1)
                {
                    n2.x = n1.x + mu * (n2.x - n1.x);
                    n2.z = -1;
                }
                else
                {
                    n1.x = n1.x + mu * (n2.x - n1.x);
                    n1.z = -1;
                }

            }

            return (true);
        }
        public void Trans_Norm2Screen(Point3D norm, Point2D projected)
        {
            //MessageBox.Show("the value of  are");
            projected.h = Convert.ToInt32(screen.center.h - screen.size.h * norm.x / 2);
            projected.v = Convert.ToInt32(screen.center.v - screen.size.v * norm.z / 2);

        }
        //public bool Trans_Point();
        public bool Trans_Line(Point3D w1, Point3D w2)
        {



            Trans_World2Eye(w1, e1);
            Trans_World2Eye(w2, e2);
            if (Trans_ClipEye(e1, e2))
            {
                Trans_Eye2Norm(e1, n1);
                Trans_Eye2Norm(e2, n2);
                if (Trans_ClipNorm(n1, n2))
                {
                    Trans_Norm2Screen(n1, p1);
                    Trans_Norm2Screen(n2, p2);
                    return (true);
                }

            }

            return (true);
        }
        public void Normalise(Point3D v)
        {
            double length;

            length = Math.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
            v.x /= length;
            v.y /= length;
            v.z /= length;
        }
        public Point3D CrossProduct(Point3D p1, Point3D p2)
        {
            Point3D p3;
            p3 = new Point3D(0, 0, 0);
            p3.x = p1.y * p2.z - p1.z * p2.y;
            p3.y = p1.z * p2.x - p1.x * p2.z;
            p3.z = p1.x * p2.y - p1.y * p2.x;
            return p3;
        }
        public bool EqualVertex(Point3D p1, Point3D p2)
        {
            if (Math.Abs(p1.x - p2.x) > EPSILON)
                return (false);
            if (Math.Abs(p1.y - p2.y) > EPSILON)
                return (false);
            if (Math.Abs(p1.z - p2.z) > EPSILON)
                return (false);

            return (true);
        }

    }

}