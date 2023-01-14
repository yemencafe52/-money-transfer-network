using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    static class clsEntryPoint
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            MyConnection.Start();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            frmLogin fl = new frmLogin();
            fl.ShowDialog();

            if (fl.Sucess)
            {   
                Application.Run(new frmDashBoard());
            }

            MyConnection.Stop();
        }
    }
}
