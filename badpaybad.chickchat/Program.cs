using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace badpaybad.chickchat
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ServicesContext.Boot();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var mainForm = new Form1();
            mainForm.Closing += MainForm_Closing;
            Application.Run(mainForm);
            Application.ApplicationExit += Application_ApplicationExit;
        }

        private static void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ServicesContext.Dispose();
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
           ServicesContext.Dispose();
        }
    }
}
