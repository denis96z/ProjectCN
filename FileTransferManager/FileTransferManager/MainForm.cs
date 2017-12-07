using System;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.IO;
using System.Threading;

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
                updateTimer.Enabled = client.Login != null;
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
                if (client.Login == null)
                {
                    return;
                }

                updateTimer.Enabled = false;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string serverPath = Interaction.InputBox("Server path: ",
                        String.Empty, new FileInfo(openFileDialog.FileName).Name);
                    transferStartTime = DateTime.Now;
                    client.UploadFile(openFileDialog.FileName,
                        serverPath, OnFileTransferProgress);
                    lblTimeLeft.Text = String.Empty;
                }
                progressBar.Value = 0;
                updateTimer.Enabled = true;
            });
        }

        private void btnDownloadFile_Click(object sender, EventArgs e)
        {
            HandleUserAction(() =>
            {
                if (client.Login == null)
                {
                    return;
                }

                updateTimer.Enabled = false;
                foreach (var file in lbFiles.SelectedItems)
                {
                    var fileData = (FileData)file;
                    string fileName = fileData.Path
                        .Substring(fileData.Path.LastIndexOf('\\') + 1);

                    saveFileDialog.FileName = saveFileDialog.InitialDirectory + fileName;
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        transferStartTime = DateTime.Now;
                        client.DownloadFile(fileData.Path, fileData.Owner,
                            saveFileDialog.FileName, OnFileTransferProgress);
                        lblTimeLeft.Text = String.Empty;
                    }

                    progressBar.Value = 0;
                }
                updateTimer.Enabled = true;
            });
        }

        private void btnShare_Click(object sender, EventArgs e)
        {
            HandleUserAction(() =>
            {
                updateTimer.Enabled = false;

                if (client.Login == null)
                {
                    return;
                }
                if (lbFiles.SelectedItems.Count != 1)
                {
                    throw new Exception("Select one file to share.");
                }
                if (lbUsers.SelectedItems.Count != 1)
                {
                    throw new Exception("Select one user to share file with.");
                }

                var fileData = (FileData)lbFiles.SelectedItems[0];
                var userData = (UserData)lbUsers.SelectedItems[0];
                client.Share(fileData.Path, userData.Login);

                updateTimer.Enabled = true;
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
            foreach (var user in client.GetUserList())
            {
                lbUsers.Items.Add(user);
            }

            lbFiles.Items.Clear();
            foreach (var file in client.GetFileList())
            {
                lbFiles.Items.Add(file);
            }

            updateTimer.Enabled = true;
        }

        private DateTime transferStartTime;

        private void OnFileTransferProgress(long progress, long fileSize)
        {
            double p = (double)progress / fileSize;
            progressBar.Value = (int)(progressBar.Maximum * p);

            string timeLeftMsg = String.Empty;
            var timePassed = DateTime.Now - transferStartTime;

            double pLeft = 1.0 - p;
            int leftDays = (int)(timePassed.Days / pLeft);
            int leftHours = (int)(timePassed.Hours / pLeft);
            int leftMinutes = (int)(timePassed.Minutes / pLeft);
            int leftSeconds = (int)(timePassed.Seconds / pLeft);

            leftMinutes += leftSeconds / 60;
            leftHours += leftMinutes / 60;
            leftMinutes %= 60;
            leftSeconds %= 60;


            if (leftDays > 0 || leftHours > 1)
            {
                timeLeftMsg = "Осталось более часа.";
            }
            else
            {
                if (leftMinutes > 0)
                {
                    switch (leftMinutes % 10)
                    {
                        case 1:
                            timeLeftMsg = "Осталась " + leftMinutes + " минута";
                        break;

                        case 2:
                        case 3:
                        case 4:
                            timeLeftMsg = "Осталось " + leftMinutes + " минуты";
                            break;

                        default:
                            timeLeftMsg = "Осталось " + leftMinutes + " минут";
                            break;
                    }

                    switch (leftSeconds % 10)
                    {
                        case 1:
                            timeLeftMsg += " " + leftSeconds + " секунд.";
                            break;

                        case 2:
                        case 3:
                        case 4:
                            timeLeftMsg += " " + leftSeconds + " секунды.";
                            break;

                        default:
                            timeLeftMsg += " " + leftMinutes + " секунд.";
                            break;
                    }
                }
                else
                {
                    switch (leftSeconds % 10)
                    {
                        case 1:
                            timeLeftMsg = "Осталась " + leftSeconds + " секунд.";
                            break;

                        case 2:
                        case 3:
                        case 4:
                            timeLeftMsg = "Осталось " + leftSeconds + " секунды.";
                            break;

                        default:
                            timeLeftMsg = "Осталось " + leftMinutes + " секунд.";
                            break;
                    }
                }
            }

            lblTimeLeft.Text = timeLeftMsg;
            Thread.Sleep(100);
        }

        private void lbFiles_Click(object sender, EventArgs e)
        {
            updateTimer.Enabled =
                lbUsers.SelectedItems.Count == 0 &&
                lbFiles.SelectedItems.Count == 0;
        }
    }
}
