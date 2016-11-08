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
    public partial class Password : Form
    {
        
        bool _confirm;
        public bool _open = false;
        string _driversName;
        public string _password;
        string _title;
        public Password(bool confirm, string driversName, string title)
        {
            _title = title;
            _driversName = driversName;
            _confirm = confirm;
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (_confirm)
            {
                if (textBox1.Text == textBox2.Text)
                {
                    _password = textBox1.Text;
                    _open = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Passwords don't match.", "Error");
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox1.Focus();
                }
            }
            else
            {
                if (_driversName == null)
                {
                    if (textBox1.Text == Properties.Settings.Default.SettingsPass)
                    {
                        _password = textBox1.Text;
                        _open = true;
                        Close();
                    }
                }
                else
                {
                    System.IO.StreamReader fileR = new System.IO.StreamReader(Properties.Settings.Default.dirPath + "\\" + _driversName + ".Dlog");
                    //Encryption en = new Encryption(fileR.ReadLine());
                    //string s = en.DecryptedText;
                    string s = Decrypt(fileR.ReadLine());
                    try
                    {
                        if (s.IndexOf('~') > 0)
                            s = s.Substring(0, s.IndexOf('~'));
                        else
                            s = "";
                    }
                    catch
                    {
                        s = "";
                    }
                    if (s == textBox1.Text || textBox1.Text == Properties.Settings.Default.SettingsPass)
                    {
                        fileR.Close();
                        _password = s;
                        _open = true;
                        Close();
                    }
                    else
                    {
                        textBox1.Text = "";
                        textBox1.Focus();
                    }
                    fileR.Close();
                }
            }
        }
        private string Decrypt(string s)
        {
            //return s;///
            byte[] b = ASCIIEncoding.Default.GetBytes(s);
            char[] c = new char[b.Length];
            for (int i = 0; i < b.Length; i++)
            {
                if (b[i] > 63)
                {
                    c[i] = Convert.ToChar((b[i] << 1) - 127);
                }
                else
                {
                    c[i] = Convert.ToChar(b[i] << 1);
                }
            }
            s = "";
            foreach (char ch in c)
            {
                s += ch;
            }
            return s;
        }
    }
}
