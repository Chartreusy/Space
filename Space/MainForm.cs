using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Space
{
    public partial class MainForm : Form
    {
        Timer timer = new Timer();
        BufferedGraphicsContext curCtxt;
        BufferedGraphics buffer;
        Renderer r;
        State s;
        CAMERA camera;
        public MainForm()
        {
            this.Width = 1000;
            this.Height = 800;
            InitializeComponent();
            CenterToScreen();
            this.Click += new EventHandler(this.MainForm_Click);
            this.KeyPress += new KeyPressEventHandler(this.MainForm_KeyPress);
            curCtxt = BufferedGraphicsManager.Current;
            buffer = curCtxt.Allocate(this.CreateGraphics(), this.DisplayRectangle);
            s = new State();
            r = new Renderer(this, s);
            camera = new CAMERA();
        }
        public void Running()
        {
            while (this.Created)
            {
                timer.Reset();
                CatchMouse();
                //s.UpdateState();
                r.Draw(buffer.Graphics, camera);
                buffer.Render();
                Application.DoEvents();
                while (timer.GetTicks() < 100) ;
            }
        }
        private void CatchMouse()
        {

            camera.rotateCamera((MousePosition.Y - ClientRectangle.Height / 2) / 50, (MousePosition.X - ClientRectangle.Width / 2) / 50, 0);
            //camera.anglev = camera.anglev + (new_mx - mx);// azimuthal - x
            //camera.angleh = camera.angleh + (new_my - my); // elevation - y
        }
        private void MainForm_Click(object sender, EventArgs e)
        {
            Console.WriteLine(MousePosition.X + ", " + MousePosition.Y);
        }
        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            double magnitude = 20.0;
            Console.WriteLine(e.KeyChar + " pressed");
            switch (e.KeyChar)
            {
                case 'w':
                    camera.pos += (camera.viewVec().normalize() * magnitude);

                    break;
                case 's':
                    camera.pos += (camera.viewVec().normalize() * -magnitude);

                    break;
                case 'a':
                    camera.rotateCamera(0, 0, 0.5);
                    break;
                case 'd':
                    camera.rotateCamera(0, 0, 0.5);
                    break;
                case 'e':
                    camera.pos += (camera.sideVec().normalize() * magnitude);
                    break;
                case 'q':
                    camera.pos += (camera.sideVec().normalize() * -magnitude);
                    break;
            }
        }
        

    }
}