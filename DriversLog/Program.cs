using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DriversLog
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                try
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form1());
                }
                catch (Exception ex)
                {
                    if (ex.Source != "System.Windows.Forms")
                        MessageBox.Show(ex.ToString() + "\n---------\n" + ex.Source);
                }
            }
            else
            {
                try
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form1(args[0]));
                }
                catch (Exception ex)
                {
                    if (ex.Source != "System.Windows.Forms")
                        MessageBox.Show(ex.ToString() + "\n---------\n" + ex.Source);
                }
            }
        }
    }
}
