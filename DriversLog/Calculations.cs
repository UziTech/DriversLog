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
    public partial class Calculations : Form
    {
        public Calculations(TextBox[] TotalMilesTB, TextBox[] TotalTimeTB, TextBox[] TotalSalesTB, TextBox[] CreditTipsTB, TextBox[] TotalTipsTB, TextBox[] CashTipsTB, TextBox[] DeliveriesTB, string BasePay, string CommPay, string CashPDelivery, string MileCredit, string EmpTax)
        {
            InitializeComponent();
                int totalI = 0;
                for (int i = 0; i < TotalMilesTB.Length; i++)
                {
                    try
                    {
                        if (TotalMilesTB[i].Visible)
                        {
                            totalI += Convert.ToInt32(TotalMilesTB[i].Text);
                        }
                    }
                    catch { }
                }
                TMTB.Text = totalI.ToString();
            try
            {
                bool show0 = false;
                int totalHours = 0, totalMins = 0;
                for (int i = 0; i < TotalTimeTB.Length; i++)
                {
                    if (TotalTimeTB[i].Visible)
                    {
                        try
                        {
                            totalHours += Convert.ToInt32(TotalTimeTB[i].Text.Substring(0, TotalTimeTB[i].Text.IndexOf(':')));
                            totalMins += Convert.ToInt32(TotalTimeTB[i].Text.Substring(TotalTimeTB[i].Text.IndexOf(':') + 1, 2));
                            show0 = true;
                        }
                        catch { }
                    }
                }
                totalHours += totalMins / 60;
                totalMins %= 60;
                if (show0)
                    THTB.Text = totalHours.ToString() + ((totalMins < 10) ? ":0" : ":") + totalMins.ToString();
                else
                    THTB.Text = "";
            }
            catch { }
                bool show1 = false;
                double totalD = 0;
                for (int i = 0; i < TotalSalesTB.Length; i++)
                {
                    if (TotalSalesTB[i].Visible)
                    {
                        try
                        {
                            totalD += Convert.ToDouble(TotalSalesTB[i].Text);
                            show1 = true;
                        }
                        catch { }
                    }
                }
                if (show1)
                    TSTB.Text = totalD.ToString("0.00");
                else
                    TSTB.Text = "";
                show1 = false;
                totalD = 0;
                for (int i = 0; i < CreditTipsTB.Length; i++)
                {
                    if (CreditTipsTB[i].Visible)
                    {
                        try
                        {
                            totalD += Convert.ToDouble(CreditTipsTB[i].Text);
                            show1 = true;
                        }
                        catch { }
                    }
                }
                if (show1)
                    TCTTB.Text = totalD.ToString("0.00");
                else
                    TCTTB.Text = "";
                show1 = false;
                totalD = 0;
                for (int i = 0; i < CashTipsTB.Length; i++)
                {
                    if (CashTipsTB[i].Visible)
                    {
                        try
                        {
                            totalD += Convert.ToDouble(CashTipsTB[i].Text);
                            show1 = true;
                        }
                        catch { }
                    }
                }
                if (show1)
                    TCaTTB.Text = totalD.ToString("0.00");
                else
                    TCaTTB.Text = "";
                show1 = false;
                totalD = 0;
                for (int i = 0; i < TotalTipsTB.Length; i++)
                {
                    if (TotalTipsTB[i].Visible)
                    {
                        try
                        {
                            totalD += Convert.ToDouble(TotalTipsTB[i].Text);
                            show1 = true;
                        }
                        catch { }
                    }
                }
                if (show1)
                    TTTB.Text = totalD.ToString();
                else
                    TTTB.Text = "";
                show1 = false;
                totalI = 0;
                for (int i = 0; i < DeliveriesTB.Length; i++)
                {
                    if (DeliveriesTB[i].Visible)
                    {
                        try
                        {
                            totalI += Convert.ToInt32(DeliveriesTB[i].Text);
                            show1 = true;
                        }
                        catch { }
                    }
                }
                if (show1)
                    TDTB.Text = totalI.ToString();
                else
                    TDTB.Text = "";
            try
            {
                BPSTB.Text = (Convert.ToDouble(TDTB.Text) * Convert.ToDouble(CashPDelivery) + Convert.ToDouble(BasePay) * (Convert.ToDouble(THTB.Text.Substring(0, THTB.Text.IndexOf(':'))) + Convert.ToDouble(THTB.Text.Substring(THTB.Text.IndexOf(':') + 1, 2)) / 60)).ToString("0.00");
            }
            catch { BPSTB.Text = ""; }
            try
            {
                BPPHTB.Text = (Convert.ToDouble(BPSTB.Text) / (Convert.ToInt32(THTB.Text.Substring(0, THTB.Text.IndexOf(':'))) + Convert.ToDouble(THTB.Text.Substring(THTB.Text.IndexOf(':') + 1, 2)) / 60)).ToString("0.00");
            }
            catch { BPPHTB.Text = ""; }
            try
            {
                CSTB.Text = (Convert.ToInt32(TDTB.Text) * Convert.ToDouble(CashPDelivery) + Convert.ToDouble(CommPay) / 100 * Convert.ToDouble(TSTB.Text)).ToString("0.00");
            }
            catch { }
            try
            {
                CPPHTB.Text = (Convert.ToDouble(CSTB.Text) / (Convert.ToInt32(THTB.Text.Substring(0, THTB.Text.IndexOf(':'))) + Convert.ToDouble(THTB.Text.Substring(THTB.Text.IndexOf(':') + 1, 2)) / 60)).ToString("0.00");
            }
            catch { CPPHTB.Text = ""; }
            try
            {
                MPHTB.Text = (Convert.ToDouble(TMTB.Text) / (Convert.ToInt32(THTB.Text.Substring(0, THTB.Text.IndexOf(':'))) + Convert.ToDouble(THTB.Text.Substring(THTB.Text.IndexOf(':') + 1, 2)) / 60)).ToString("0.00000");
            }
            catch { MPHTB.Text = ""; }
            try
            {
                SPHTB.Text = (Convert.ToDouble(TSTB.Text) / (Convert.ToInt32(THTB.Text.Substring(0, THTB.Text.IndexOf(':'))) + Convert.ToDouble(THTB.Text.Substring(THTB.Text.IndexOf(':') + 1, 2)) / 60)).ToString("0.00");
            }
            catch { SPHTB.Text = ""; }
            try
            {
                TPHTB.Text = (Convert.ToDouble(TTTB.Text) / (Convert.ToInt32(THTB.Text.Substring(0, THTB.Text.IndexOf(':'))) + Convert.ToDouble(THTB.Text.Substring(THTB.Text.IndexOf(':') + 1, 2)) / 60)).ToString("0.00");
            }
            catch { TPHTB.Text = ""; }
            try
            {
                DPHTB.Text = (Convert.ToDouble(TDTB.Text) / (Convert.ToInt32(THTB.Text.Substring(0, THTB.Text.IndexOf(':'))) + Convert.ToDouble(THTB.Text.Substring(THTB.Text.IndexOf(':') + 1, 2)) / 60)).ToString("0.00000");
            }
            catch { DPHTB.Text = ""; }
            try
            {
                BSWTTB.Text = (Convert.ToDouble(BPSTB.Text) + Convert.ToDouble(TTTB.Text)).ToString("0.00");
            }
            catch { BSWTTB.Text = ""; }
            try
            {
                CSWTTB.Text = (Convert.ToDouble(CSTB.Text) + Convert.ToDouble(TTTB.Text)).ToString("0.00");
            }
            catch { CSWTTB.Text = ""; }
            try
            {
                TPDTB.Text = (Convert.ToDouble(TTTB.Text) / Convert.ToInt32(TDTB.Text)).ToString("0.00");
            }
            catch { TPDTB.Text = ""; }
            try
            {
                TPMTB.Text = (Convert.ToDouble(TTTB.Text) / Convert.ToDouble(TMTB.Text)).ToString("0.00");
            }
            catch { TPMTB.Text = ""; }
            try
            {
                TPTB.Text = (Convert.ToDouble(TTTB.Text) / Convert.ToDouble(TSTB.Text) * 100).ToString("0.00000");
            }
            catch { TPTB.Text = ""; }
            try
            {
                SPDTB.Text = (Convert.ToDouble(TSTB.Text) / Convert.ToInt32(TDTB.Text)).ToString("0.00");
            }
            catch { SPDTB.Text = ""; }
            try
            {
                MPDTB.Text = (Convert.ToDouble(TMTB.Text) / Convert.ToInt32(TDTB.Text)).ToString("0.00000");
            }
            catch { MPDTB.Text = ""; }
            try
            {
                ETTB.Text = ((((Convert.ToDouble(BPSTB.Text) > Convert.ToDouble(CSTB.Text)) ? Convert.ToDouble(BPSTB.Text) : Convert.ToDouble(CSTB.Text)) - Convert.ToDouble(MileCredit) * Convert.ToInt32(TMTB.Text)) * (Convert.ToDouble(EmpTax) / 100)).ToString("0.00");
            }
            catch { ETTB.Text = ""; }
        }

        private void TB_Focus(object sender, EventArgs e)
        {
            button1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
