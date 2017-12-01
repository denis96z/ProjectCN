using System;
using NLog;
using System.Windows.Forms;

namespace FileTransferManagerServer
{
    static class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            catch (Exception exception)
            {
                logger.Fatal(exception);
            }
        }
    }
}
