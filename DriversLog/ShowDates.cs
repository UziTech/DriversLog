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
    public partial class ShowDates : Form
    {
        #region Global Variables
        public DateTime _startDate = DateTime.MinValue, _endDate;
        #endregion
        public ShowDates(DateTime minDate, DateTime maxDate)
        {
            InitializeComponent();
            dateTimePicker1.MinDate = minDate;
            dateTimePicker2.MinDate = minDate;
            dateTimePicker1.MaxDate = maxDate;
            dateTimePicker2.MaxDate = maxDate; 
            dateTimePicker1.Text = minDate.ToString("M/d/yy");
            dateTimePicker2.Text = maxDate.ToString("M/d/yy");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _startDate = dateTimePicker1.Value;
            _endDate = dateTimePicker2.Value;
            Close();
        }
    }
}
