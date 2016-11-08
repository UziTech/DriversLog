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
    public partial class NewDriver : Form
    {
        public bool _open = false;
        public string _driverName, _pass, _driverList;
        public NewDriver(string driverList)
        {
            _driverList = driverList;
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Driver name can not be blank.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Focus();
                textBox1.SelectAll();
                return;
            }
            if(_driverList.Contains("|" + textBox1.Text + "|"))
            {
                MessageBox.Show("That name already exists.","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Focus();
                textBox1.SelectAll();
                return;
            }
            if (textBox1.Text != "")
            {
                Password p = new Password(true, textBox1.Text, _driverName + @"'s New Password");
                p.ShowDialog();
                if (p._open)
                {
                    _open = true;
                    _pass = p._password;
                    _driverName = textBox1.Text;
                    _driverList += "|" + textBox1.Text;
                    Properties.Settings.Default.Save();
                    if (!Directory.Exists(Properties.Settings.Default.dirPath))
                    {
                        Directory.CreateDirectory(Properties.Settings.Default.dirPath);
                    }
                    StreamWriter fileW = new StreamWriter(Properties.Settings.Default.dirPath + "\\" + textBox1.Text + ".Dlog", false);
                    //Encryption en = new Encryption(p._password + "~Line#|Date|StartMiles|EndMiles|TotalMiles|TimeIn|TimeOut|TotalTime|TotalSales|CreditTips|TotalTips|CashTips|Deliveries|BasePay|Commission%|MileCredit|EmpTax%");
                    //fileW.WriteLine(en.EncryptedText);
                    fileW.WriteLine(Encrypt(p._password + "~Line#|Date|StartMiles|EndMiles|TotalMiles|TimeIn|TimeOut|TotalTime|TotalSales|CreditTips|TotalTips|CashTips|Deliveries|BasePay|Commission%|MileCredit|EmpTax%"));
                    fileW.Close();
                }
            }
            Close();
        }
        private string Encrypt(string s)
        {
            //return s;///
            byte[] b = ASCIIEncoding.Default.GetBytes(s);
            char[] c = new char[b.Length];
            for (int i = 0; i < b.Length; i++)
            {
                if (b[i] % 2 == 1)
                {
                    c[i] = Convert.ToChar((b[i] >> 1) + 64);
                }
                else
                {
                    c[i] = Convert.ToChar(b[i] >> 1);
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
