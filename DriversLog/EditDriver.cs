using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DriversLog
{
    public partial class EditDriver : Form
    {
        public string _name, _driverList;
        public EditDriver(string name, string driverList)
        {
            _driverList = driverList;
            InitializeComponent();
            textBox1.Text = _name = name;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == _name || (!_driverList.Contains("|" + textBox1.Text + "|") && !_driverList.EndsWith("|" + textBox1.Text)))
            {
                _driverList = _driverList.Replace(_name, textBox1.Text);
                Properties.Settings.Default.Save();
                File.Move(Properties.Settings.Default.dirPath + "\\" + _name + ".Dlog", Properties.Settings.Default.dirPath + "\\" + textBox1.Text + ".Dlog");
                _name = textBox1.Text;
                Close();
            }
            else
            {
                MessageBox.Show("Driver Already Exists!","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Focus();
                textBox1.SelectAll();
            }
        }
    }
}
