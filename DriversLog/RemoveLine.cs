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
    public partial class RemoveLine : Form
    {
        public int _lineNum = 0, _maxLines;
        public RemoveLine(int maxLines)
        {
            _maxLines = maxLines;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = 0;
            try
            {
                i = Convert.ToInt32(textBox1.Text);
            }
            finally
            {
                if (i <= _maxLines && i > 0)
                    _lineNum = i;
            }
            Close();
        }
    }
}
