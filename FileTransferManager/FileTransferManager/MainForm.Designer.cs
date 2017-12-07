namespace FileTransferManager
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gbFileList = new System.Windows.Forms.GroupBox();
            this.lbFiles = new System.Windows.Forms.CheckedListBox();
            this.btnUploadFile = new System.Windows.Forms.Button();
            this.btnDownloadFile = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnSignInOut = new System.Windows.Forms.Button();
            this.gbUserList = new System.Windows.Forms.GroupBox();
            this.lbUsers = new System.Windows.Forms.CheckedListBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.btnShare = new System.Windows.Forms.Button();
            this.gbFileList.SuspendLayout();
            this.gbUserList.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbFileList
            // 
            this.gbFileList.Controls.Add(this.lbFiles);
            this.gbFileList.Location = new System.Drawing.Point(12, 35);
            this.gbFileList.Name = "gbFileList";
            this.gbFileList.Size = new System.Drawing.Size(326, 257);
            this.gbFileList.TabIndex = 0;
            this.gbFileList.TabStop = false;
            this.gbFileList.Text = "Files on server:";
            // 
            // lbFiles
            // 
            this.lbFiles.FormattingEnabled = true;
            this.lbFiles.Location = new System.Drawing.Point(6, 19);
            this.lbFiles.Name = "lbFiles";
            this.lbFiles.Size = new System.Drawing.Size(314, 229);
            this.lbFiles.TabIndex = 8;
            this.lbFiles.Click += new System.EventHandler(this.lbFiles_Click);
            // 
            // btnUploadFile
            // 
            this.btnUploadFile.Location = new System.Drawing.Point(13, 316);
            this.btnUploadFile.Name = "btnUploadFile";
            this.btnUploadFile.Size = new System.Drawing.Size(93, 23);
            this.btnUploadFile.TabIndex = 1;
            this.btnUploadFile.Text = "Upload";
            this.btnUploadFile.UseVisualStyleBackColor = true;
            this.btnUploadFile.Click += new System.EventHandler(this.btnUploadFile_Click);
            // 
            // btnDownloadFile
            // 
            this.btnDownloadFile.Location = new System.Drawing.Point(118, 316);
            this.btnDownloadFile.Name = "btnDownloadFile";
            this.btnDownloadFile.Size = new System.Drawing.Size(94, 23);
            this.btnDownloadFile.TabIndex = 2;
            this.btnDownloadFile.Text = "Download";
            this.btnDownloadFile.UseVisualStyleBackColor = true;
            this.btnDownloadFile.Click += new System.EventHandler(this.btnDownloadFile_Click);
            // 
            // btnRegister
            // 
            this.btnRegister.Location = new System.Drawing.Point(13, 6);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(93, 23);
            this.btnRegister.TabIndex = 3;
            this.btnRegister.Text = "Register";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // btnSignInOut
            // 
            this.btnSignInOut.Location = new System.Drawing.Point(118, 6);
            this.btnSignInOut.Name = "btnSignInOut";
            this.btnSignInOut.Size = new System.Drawing.Size(94, 23);
            this.btnSignInOut.TabIndex = 4;
            this.btnSignInOut.Text = "Sign In";
            this.btnSignInOut.UseVisualStyleBackColor = true;
            this.btnSignInOut.Click += new System.EventHandler(this.btnSignInOut_Click);
            // 
            // gbUserList
            // 
            this.gbUserList.Controls.Add(this.lbUsers);
            this.gbUserList.Location = new System.Drawing.Point(344, 36);
            this.gbUserList.Name = "gbUserList";
            this.gbUserList.Size = new System.Drawing.Size(199, 257);
            this.gbUserList.TabIndex = 6;
            this.gbUserList.TabStop = false;
            this.gbUserList.Text = "Users:";
            // 
            // lbUsers
            // 
            this.lbUsers.FormattingEnabled = true;
            this.lbUsers.Location = new System.Drawing.Point(6, 22);
            this.lbUsers.Name = "lbUsers";
            this.lbUsers.Size = new System.Drawing.Size(187, 229);
            this.lbUsers.TabIndex = 0;
            this.lbUsers.Click += new System.EventHandler(this.lbFiles_Click);
            // 
            // updateTimer
            // 
            this.updateTimer.Interval = 5000;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(344, 316);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(199, 23);
            this.progressBar.TabIndex = 7;
            // 
            // btnShare
            // 
            this.btnShare.Location = new System.Drawing.Point(218, 316);
            this.btnShare.Name = "btnShare";
            this.btnShare.Size = new System.Drawing.Size(114, 23);
            this.btnShare.TabIndex = 8;
            this.btnShare.Text = "Share";
            this.btnShare.UseVisualStyleBackColor = true;
            this.btnShare.Click += new System.EventHandler(this.btnShare_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 351);
            this.Controls.Add(this.btnShare);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.gbUserList);
            this.Controls.Add(this.btnSignInOut);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.btnDownloadFile);
            this.Controls.Add(this.btnUploadFile);
            this.Controls.Add(this.gbFileList);
            this.Name = "MainForm";
            this.Text = "Ftm2017";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.gbFileList.ResumeLayout(false);
            this.gbUserList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbFileList;
        private System.Windows.Forms.Button btnUploadFile;
        private System.Windows.Forms.Button btnDownloadFile;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnSignInOut;
        private System.Windows.Forms.GroupBox gbUserList;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.CheckedListBox lbFiles;
        private System.Windows.Forms.CheckedListBox lbUsers;
        private System.Windows.Forms.Button btnShare;
    }
}

