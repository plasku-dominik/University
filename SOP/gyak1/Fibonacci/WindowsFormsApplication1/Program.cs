using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string []args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string fib1="0", fib2="1";
            if (args.Length == 0)
                fib1 = "0";
            if (args.Length == 1)
                fib2 = "1";
            if (args.Length > 1)
            { fib1 = args[0]; fib2 = args[1]; }

            Application.Run(new Form1(fib1,fib2));
        }
    }
}
