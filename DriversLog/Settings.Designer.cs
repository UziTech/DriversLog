namespace DriversLog
{
    partial class Settings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.browseButton = new System.Windows.Forms.Button();
            this.logFolderTextBox = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.passTextBox1 = new System.Windows.Forms.TextBox();
            this.passTextBox2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.NumLinesDays = new System.Windows.Forms.TextBox();
            this.LinesRB = new System.Windows.Forms.RadioButton();
            this.DaysRB = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Save Folder";
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "Folder to store logs in.";
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(411, 4);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 23);
            this.browseButton.TabIndex = 1;
            this.browseButton.Text = "Browse...";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // logFolderTextBox
            // 
            this.logFolderTextBox.Location = new System.Drawing.Point(79, 6);
            this.logFolderTextBox.Name = "logFolderTextBox";
            this.logFolderTextBox.Size = new System.Drawing.Size(326, 20);
            this.logFolderTextBox.TabIndex = 2;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(182, 84);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(55, 23);
            this.saveButton.TabIndex = 3;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(260, 84);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(55, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 84;
            this.label3.Text = "Settings Password";
            // 
            // passTextBox1
            // 
            this.passTextBox1.Location = new System.Drawing.Point(109, 32);
            this.passTextBox1.Name = "passTextBox1";
            this.passTextBox1.Size = new System.Drawing.Size(102, 20);
            this.passTextBox1.TabIndex = 85;
            this.passTextBox1.UseSystemPasswordChar = true;
            // 
            // passTextBox2
            // 
            this.passTextBox2.Location = new System.Drawing.Point(265, 32);
            this.passTextBox2.Name = "passTextBox2";
            this.passTextBox2.Size = new System.Drawing.Size(102, 20);
            this.passTextBox2.TabIndex = 86;
            this.passTextBox2.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(217, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 87;
            this.label4.Text = "Confirm";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 88;
            this.label2.Text = "Show";
            // 
            // NumLinesDays
            // 
            this.NumLinesDays.Location = new System.Drawing.Point(53, 60);
            this.NumLinesDays.Name = "NumLinesDays";
            this.NumLinesDays.Size = new System.Drawing.Size(50, 20);
            this.NumLinesDays.TabIndex = 89;
            this.NumLinesDays.Text = "0";
            // 
            // LinesRB
            // 
            this.LinesRB.AutoSize = true;
            this.LinesRB.Checked = true;
            this.LinesRB.Location = new System.Drawing.Point(109, 61);
            this.LinesRB.Name = "LinesRB";
            this.LinesRB.Size = new System.Drawing.Size(50, 17);
            this.LinesRB.TabIndex = 90;
            this.LinesRB.TabStop = true;
            this.LinesRB.Text = "Lines";
            this.LinesRB.UseVisualStyleBackColor = true;
            // 
            // DaysRB
            // 
            this.DaysRB.AutoSize = true;
            this.DaysRB.Location = new System.Drawing.Point(165, 61);
            this.DaysRB.Name = "DaysRB";
            this.DaysRB.Size = new System.Drawing.Size(49, 17);
            this.DaysRB.TabIndex = 91;
            this.DaysRB.Text = "Days";
            this.DaysRB.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(220, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 92;
            this.label5.Text = "0 = All";
            // 
            // Settings
            // 
            this.AcceptButton = this.saveButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 116);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.DaysRB);
            this.Controls.Add(this.LinesRB);
            this.Controls.Add(this.NumLinesDays);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.passTextBox2);
            this.Controls.Add(this.passTextBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.logFolderTextBox);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.ShowInTaskbar = false;
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.TextBox logFolderTextBox;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox passTextBox1;
        private System.Windows.Forms.TextBox passTextBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox NumLinesDays;
        private System.Windows.Forms.RadioButton LinesRB;
        private System.Windows.Forms.RadioButton DaysRB;
        private System.Windows.Forms.Label label5;
    }
}