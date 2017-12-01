using System;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace FileTransferManager
{
    public partial class MainForm : Form
    {
        private Client client = new Client();

        public MainForm()
        {
            InitializeComponent();
        }

        private void HandleUserAction(Action userAction)
        {
            try
            {
                userAction.Invoke();
                if (updateTimer.Enabled) UpdateLists();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            HandleUserAction(() =>
            {
                if (client.Login == null)
                {
                    string login = Interaction.InputBox("Login: ", "Registration");
                    string password = Interaction.InputBox("Password: ", "Registration");
                    client.Register(login, password);
                    GuiAuthorize();
                }
            });
        }

        private void btnSignInOut_Click(object sender, EventArgs e)
        {
            HandleUserAction(() =>
            {
                if (client.Login == null)
                {
                    string login = Interaction.InputBox("Login: ", "Registration");
                    string password = Interaction.InputBox("Password: ", "Registration");
                    client.SignIn(login, password);
                    GuiAuthorize();
                }
                else
                {
                    client.SignOut();
                    GuiDeauthorize();
                }
            });
        }

        private void btnUploadFile_Click(object sender, EventArgs e)
        {
            HandleUserAction(() =>
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string serverPath = Interaction.InputBox("Server path: ", "Input");
                    client.UploadFile(openFileDialog.FileName, serverPath);
                }
            });
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            HandleUserAction(() =>
            {
                UpdateLists();
            });
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                client.Disconnect();
            }
            catch
            {
                //NOP
            }
        }

        private void GuiAuthorize()
        {
            btnRegister.Enabled = false;
            btnSignInOut.Text = "Sign out";

            lbUsers.Items.Clear();
            lbFiles.Items.Clear();

            updateTimer.Enabled = true;
        }

        private void GuiDeauthorize()
        {
            updateTimer.Enabled = false;

            btnRegister.Enabled = true;
            btnSignInOut.Text = "Sign in";

            lbUsers.Items.Clear();
            lbFiles.Items.Clear();
        }

        private void UpdateLists()
        {
            updateTimer.Enabled = false;

            lbUsers.Items.Clear();
            foreach (string user in client.GetUserList())
            {
                lbUsers.Items.Add(user);
            }

            lbFiles.Items.Clear();
            foreach (string file in client.GetFileList())
            {
                lbFiles.Items.Add(file);
            }

            updateTimer.Enabled = true;
        }
    }
}
