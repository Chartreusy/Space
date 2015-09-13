using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Space
{
    public class Renderer
    {
        State s;
        MainForm f;
        public Renderer(MainForm f, State s)
        {
            this.f = f;
            this.s = s;
        }
        public void Draw(Graphics g, CAMERA camera)
        {
            g.Clear(Color.Black);
            Font drawFont = new Font("Arial", 16);

            g.DrawString("Position: " + camera.pos + " - Facing:" + camera.facing + " - orient:" + camera.up, drawFont, new SolidBrush(Color.AliceBlue), new PointF(0f, 0f));
            g.DrawString("Camera axes: x: " + camera.axes[0] + " - y:" + camera.axes[1] +" - z: " + camera.axes[2], drawFont, new SolidBrush(Color.AliceBlue), new PointF(0f, 20f));
            Figure3D[] cubes = new Figure3D[] { new Figure3D(new Point3D(-100,0,0)),
                                                new Figure3D(new Point3D(100,0,0)),
                                                new Figure3D(new Point3D(0,0,0)),
                                                new Figure3D(new Point3D(0,100,0)),
                                                new Figure3D(new Point3D(0,0, 100))};
            
            foreach(Figure3D cube in cubes)
            {
                DrawFigure(g, camera, cube);
            }
            
            //DrawThing(g);
            // draw all things
        }
        /*
        Will get more complicated once we make it truly 3D
        */
        public void DrawFigure(Graphics g, CAMERA camera, Figure3D fig)
        {
            projection proc = new projection(camera);
            Pen pen = new Pen(Color.Chartreuse);
            Point3D prev, cur;
            for (int i = 1; i< fig.path.Length; i++)
            {
                prev = new Point3D(fig.origin + fig.path[i - 1]);
                cur = new Point3D(fig.origin + fig.path[i]);
                proc.Trans_Line(prev, cur);
                g.DrawEllipse(pen, new RectangleF(proc.p1.h-2.5f, proc.p1.v-2.5f, 5, 5));
                g.DrawEllipse(pen, new RectangleF(proc.p2.h-2.5f, proc.p2.v-2.5f, 5, 5));
                g.DrawLine(pen, proc.p1.h, proc.p1.v, proc.p2.h, proc.p2.v);
            }
            

        }
    }
}
