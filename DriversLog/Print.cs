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
    public partial class Print : Form
    {
        public bool _print = false;
        string _name = "";
        public Print(string name)
        {
            _name = name;
            InitializeComponent();
            this.Text = "Print " + name + "'s Log";
            ShownDates.Checked = !AllDates.Checked;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            _print = true;
            Properties.Settings.Default.PLineNum = LineNum.Checked;
            Properties.Settings.Default.PDate = Date.Checked;
            Properties.Settings.Default.PStartMiles = StartMiles.Checked;
            Properties.Settings.Default.PEndMiles = EndMiles.Checked;
            Properties.Settings.Default.PTotalMilesCol = TotalMilesCol.Checked;
            Properties.Settings.Default.PTimeIn = TimeIn.Checked;
            Properties.Settings.Default.PTimeOut = TimeOut.Checked;
            Properties.Settings.Default.PTotalTime = TotalTime.Checked;
            Properties.Settings.Default.PTotalSalesCol = TotalSalesCol.Checked;
            Properties.Settings.Default.PCreditTips = CreditTips.Checked;
            Properties.Settings.Default.PTotalTipsCol = TotalTipsCol.Checked;
            Properties.Settings.Default.PCashTips = CashTips.Checked;
            Properties.Settings.Default.PDeliveries = Deliveries.Checked;
            Properties.Settings.Default.PBasePay = BasePay.Checked;
            Properties.Settings.Default.PCommP = CommP.Checked;
            Properties.Settings.Default.PBasePaySalary = BasePaySalary.Checked;
            Properties.Settings.Default.PCommPaySalary = CommPaySalary.Checked;
            Properties.Settings.Default.PTotalMiles = TotalMiles.Checked;
            Properties.Settings.Default.PMileCredit = MileCredit.Checked;
            Properties.Settings.Default.PEmpTaxP = EmpTaxP.Checked;
            Properties.Settings.Default.PTotalEmpTax = TotalEmpTax.Checked;
            Properties.Settings.Default.PTotalHours = TotalHours.Checked;
            Properties.Settings.Default.PTipPM = TipPM.Checked;
            Properties.Settings.Default.PMilesPH = MilesPH.Checked;
            Properties.Settings.Default.PBasePayPH = BasePayPH.Checked;
            Properties.Settings.Default.PCommPayPH = CommPayPH.Checked;
            Properties.Settings.Default.PSalesPH = SalesPH.Checked;
            Properties.Settings.Default.PTipsPH = TipsPH.Checked;
            Properties.Settings.Default.PDlvsPH = DlvsPH.Checked;
            Properties.Settings.Default.PTotalSales = TotalSales.Checked;
            Properties.Settings.Default.PTotalTips = TotalTips.Checked;
            Properties.Settings.Default.PTotalDlvs = TotalDlvs.Checked;
            Properties.Settings.Default.PSalesPD = SalesPD.Checked;
            Properties.Settings.Default.PTipsPD = TipsPD.Checked;
            Properties.Settings.Default.PTipP = TipP.Checked;
            Properties.Settings.Default.PBSWT = BSWT.Checked;
            Properties.Settings.Default.PCSWT = CSWT.Checked;
            Properties.Settings.Default.PTCTips = TCTips.Checked;
            Properties.Settings.Default.PTCaTips = TCaTips.Checked;
            Properties.Settings.Default.AllDates =  AllDates.Checked;
            Properties.Settings.Default.Save();
            Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
