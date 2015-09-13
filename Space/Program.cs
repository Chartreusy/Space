using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
namespace Space
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Console.WriteLine("Start");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //testTransforms();
            //testCamera();
            MainForm mainForm = new MainForm();

            mainForm.Show();
            mainForm.Running();
        }
    }
}