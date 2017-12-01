using System;
using System.Windows.Forms;

namespace FileTransferManagerServer
{
    public partial class MainForm : Form
    {
        private Server server = new Server();

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            server.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            server.Stop();
        }
    }
}
