using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DriversLog
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            logFolderTextBox.Text = Properties.Settings.Default.dirPath;
            passTextBox1.Text = passTextBox2.Text = Properties.Settings.Default.SettingsPass;
            NumLinesDays.Text = Properties.Settings.Default.NumLinesDays.ToString();
            LinesRB.Checked = Properties.Settings.Default.ShowLines;
            DaysRB.Checked = !Properties.Settings.Default.ShowLines;
        }
        private void browseButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                logFolderTextBox.Text = folderBrowserDialog1.SelectedPath;
            }
        }
        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void saveButton_Click(object sender, EventArgs e)
        {
            if (System.IO.Directory.Exists(logFolderTextBox.Text) && passTextBox1.Text == passTextBox2.Text)
            {
                Properties.Settings.Default.dirPath = logFolderTextBox.Text;
                Properties.Settings.Default.SettingsPass = passTextBox1.Text;
            }
            else
            {
                if (!System.IO.Directory.Exists(logFolderTextBox.Text))
                {
                    MessageBox.Show("Save Folder does not exist!", "Error");
                    logFolderTextBox.Text = Properties.Settings.Default.dirPath;
                    logFolderTextBox.Focus();
                }
                if (passTextBox1.Text != passTextBox2.Text)
                {
                    MessageBox.Show("Passwords do not match!", "Error");
                    passTextBox1.Text = passTextBox2.Text = Properties.Settings.Default.SettingsPass;
                    passTextBox1.Focus();
                }
                return;
            }
            if (Properties.Settings.Default.dirPath.EndsWith("\\"))
                Properties.Settings.Default.dirPath = Properties.Settings.Default.dirPath.Substring(0, Properties.Settings.Default.dirPath.Length - 1);
            Properties.Settings.Default.SettingsViewed = true;
            Properties.Settings.Default.ShowLines = LinesRB.Checked;
            Properties.Settings.Default.NumLinesDays = Convert.ToInt32(NumLinesDays.Text);
            Properties.Settings.Default.Save();
            Close();
        }
    }
}
