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
    public partial class Calculator : Form
    {
        public Calculator()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            double visaD, discoverD, masterCardD, americanExpressD, otherCreditCardsD, check1D, check2D, check3D, check4D, check5D, check6D, check7D, check8D, otherPaymentD, totalDueFromDriverD;
            int d100I, d50I, d20I, d10I, d5I, d1I, dp25I, dp10I, dp05I, dp01I;
            string change = "";
            try
            {
                visaD = Convert.ToDouble(visaTB.Text);
            }
            catch
            {
                visaD = 0;
                visaTB.Text = "0.00";
            }
            try
            {
                discoverD = Convert.ToDouble(discoverTB.Text);
            }
            catch
            {
                discoverD = 0;
                discoverTB.Text = "0.00";
            }
            try
            {
                masterCardD = Convert.ToDouble(masterCardTB.Text);
            }
            catch
            {
                masterCardD = 0;
                masterCardTB.Text = "0.00";
            }
            try
            {
                americanExpressD = Convert.ToDouble(americanExpressTB.Text);
            }
            catch
            {
                americanExpressD = 0;
                americanExpressTB.Text = "0.00";
            }
            try
            {
                otherCreditCardsD = Convert.ToDouble(otherCreditCardTB.Text);
            }
            catch
            {
                otherCreditCardsD = 0;
                otherCreditCardTB.Text = "0.00";
            }
            try
            {
                check1D = Convert.ToDouble(check1TB.Text);
            }
            catch
            {
                check1D = 0;
                check1TB.Text = "0.00";
            }
            try
            {
                check2D = Convert.ToDouble(check2TB.Text);
            }
            catch
            {
                check2D = 0;
                check2TB.Text = "0.00";
            }
            try
            {
                check3D = Convert.ToDouble(check3TB.Text);
            }
            catch
            {
                check3D = 0;
                check3TB.Text = "0.00";
            }
            try
            {
                check4D = Convert.ToDouble(check4TB.Text);
            }
            catch
            {
                check4D = 0;
                check4TB.Text = "0.00";
            }
            try
            {
                check5D = Convert.ToDouble(check5TB.Text);
            }
            catch
            {
                check5D = 0;
                check5TB.Text = "0.00";
            }
            try
            {
                check6D = Convert.ToDouble(check6TB.Text);
            }
            catch
            {
                check6D = 0;
                check6TB.Text = "0.00";
            }
            try
            {
                check7D = Convert.ToDouble(check7TB.Text);
            }
            catch
            {
                check7D = 0;
                check7TB.Text = "0.00";
            }
            try
            {
                check8D = Convert.ToDouble(check8TB.Text);
            }
            catch
            {
                check8D = 0;
                check8TB.Text = "0.00";
            }
            try
            {
                otherPaymentD = Convert.ToDouble(otherPaymentTB.Text);
            }
            catch
            {
                otherPaymentD = 0;
                otherPaymentTB.Text = "0.00";
            }
            try
            {
                totalDueFromDriverD = Convert.ToDouble(totalDueFromDriverTB.Text);
            }
            catch
            {
                totalDueFromDriverD = 0;
                totalDueFromDriverTB.Text = "0.00";
            }
            try
            {
                d100I = Convert.ToInt32(d100TB.Text);
            }
            catch
            {
                d100I = 0;
                d100TB.Text = "0";
            }
            try
            {
                d50I = Convert.ToInt32(d50TB.Text);
            }
            catch
            {
                d50I = 0;
                d50TB.Text = "0";
            }
            try
            {
                d20I = Convert.ToInt32(d20TB.Text);
            }
            catch
            {
                d20I = 0;
                d20TB.Text = "0";
            }
            try
            {
                d10I = Convert.ToInt32(d10TB.Text);
            }
            catch
            {
                d10I = 0;
                d10TB.Text = "0";
            }
            try
            {
                d5I = Convert.ToInt32(d5TB.Text);
            }
            catch
            {
                d5I = 0;
                d5TB.Text = "0";
            }
            try
            {
                d1I = Convert.ToInt32(d1TB.Text);
            }
            catch
            {
                d1I = 0;
                d1TB.Text = "0";
            }
            try
            {
                dp25I = Convert.ToInt32(dp25TB.Text);
            }
            catch
            {
                dp25I = 0;
                dp25TB.Text = "0";
            }
            try
            {
                dp10I = Convert.ToInt32(dp10TB.Text);
            }
            catch
            {
                dp10I = 0;
                dp10TB.Text = "0";
            }
            try
            {
                dp05I = Convert.ToInt32(dp05TB.Text);
            }
            catch
            {
                dp05I = 0;
                dp05TB.Text = "0";
            }
            try
            {
                dp01I = Convert.ToInt32(dp01TB.Text);
            }
            catch
            {
                dp01I = 0;
                dp01TB.Text = "0";
            }
            double tCardTotalD = visaD + masterCardD + discoverD + americanExpressD + otherCreditCardsD;
            double tChecksTotalD = check1D + check2D + check3D + check4D + check5D + check6D + check7D + check8D;
            double tOtherPaymentD = otherPaymentD;
            double totalTipsD = tCardTotalD + tChecksTotalD + tOtherPaymentD + d100I * 100 + d50I * 50 + d20I * 20 + d10I * 10 + d5I * 5 + d1I + dp25I * .25 + dp10I * .1 + dp05I * .05 + dp01I * .01 - totalDueFromDriverD;
            double totalTips;
            if (tTotalTipsTB.Text != "" && MessageBox.Show("Did you want to calculate the tip?", "Calculate The Tip", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                tTotalTipsTB.Text = "";
            if (tTotalTipsTB.Text == "")
                totalTips = totalTipsD;
            else
            {
                try
                {
                    totalTips = Convert.ToDouble(tTotalTipsTB.Text);
                }
                catch
                {
                    totalTips = totalTipsD;
                }
            }
            int c50I = 0, c20I = 0, c10I = 0, c5I = 0, c1I = 0, cp25I = 0, cp10I = 0, cp05I = 0, cp01I = 0;
            while (totalTips > 0)
            {
                if (d100I <= 0 && d50I <= 0 && d20I <= 0 && d10I <= 0 && d5I <= 0 && d1I <= 0 && dp25I <= 0 && dp10I <= 0 && dp05I <= 0 && dp01I <= 0)
                {
                    MessageBox.Show("Not enough cash for your tip!\r\nExchange a check or credit card reciept for cash.");
                    break;
                }
                while (totalTips >= 100 && d100I > 0)
                {
                    totalTips = Math.Round(totalTips - 100, 2);
                    d100I--;
                }
                while (totalTips >= 50 && d50I > 0)
                {
                    totalTips = Math.Round(totalTips - 50, 2);
                    d50I--;
                }
                while (totalTips >= 20 && d20I > 0)
                {
                    totalTips = Math.Round(totalTips - 20, 2);
                    d20I--;
                }
                while (totalTips >= 10 && d10I > 0)
                {
                    totalTips = Math.Round(totalTips - 10, 2);
                    d10I--;
                }
                while (totalTips >= 5 && d5I > 0)
                {
                    totalTips = Math.Round(totalTips - 5, 2);
                    d5I--;
                }
                while (totalTips >= 1 && d1I > 0)
                {
                    totalTips = Math.Round(totalTips - 1, 2);
                    d1I--;
                }
                while(totalTips >= .25 && dp25I > 0)
                {
                    totalTips = Math.Round(totalTips - .25, 2);
                    dp25I--;
                }
                while(totalTips >= .10 && dp10I > 0)
                {
                    totalTips = Math.Round(totalTips - .10, 2);
                    dp10I--;
                }
                while(totalTips >= .05 && dp05I > 0)
                {
                    totalTips = Math.Round(totalTips - .05, 2);
                    dp05I--;
                }
                while(totalTips >= .01 && dp01I > 0)
                {
                    totalTips = Math.Round(totalTips - .01, 2);
                    dp01I--;
                }
                if (totalTips > 0)
                {
                    if (dp05I > 0)
                    {
                        dp05I--;
                        dp01I += 5;
                        cp01I += 5;
                        if (change == "")
                        {
                            change = "Enchange a Nickle for ";
                        }
                        else
                        {
                            cp05I--;
                        }
                    }
                    else if (dp10I > 0)
                    {
                        dp10I--;
                        dp05I += 2;
                        cp05I += 2;
                        if (change == "")
                        {
                            change = "Enchange a Dime for ";
                        }
                        else
                        {
                            cp10I--;
                        }
                    }
                    else if (dp25I > 0)
                    {
                        dp25I--;
                        dp10I += 2;
                        dp05I += 1;
                        cp10I += 2;
                        cp05I += 1;
                        if (change == "")
                        {
                            change = "Enchange a Quarter for ";
                        }
                        else
                        {
                            cp25I--;
                        }
                    }
                    else if (d1I > 0)
                    {
                        d1I--;
                        dp25I += 4;
                        cp25I += 4;
                        if (change == "")
                        {
                            change = "Enchange a $1 bill for ";
                        }
                        else
                        {
                            c1I--;
                        }
                    }
                    else if (d5I > 0)
                    {
                        d5I--;
                        d1I += 5;
                        c1I += 5;
                        if (change == "")
                        {
                            change = "Enchange a $5 bill for ";
                        }
                        else
                        {
                            c5I--;
                        }
                    }
                    else if (d10I > 0)
                    {
                        d10I--;
                        d5I += 2;
                        c5I += 2;
                        if (change == "")
                        {
                            change = "Enchange a $10 bill for ";
                        }
                        else
                        {
                            c10I--;
                        }
                    }
                    else if (d20I > 0)
                    {
                        d20I--;
                        d10I += 2;
                        c10I += 2;
                        if (change == "")
                        {
                            change = "Enchange a $20 bill for ";
                        }
                        else
                        {
                            c20I--;
                        }
                    }
                    else if (d50I > 0)
                    {
                        d50I--;
                        d20I += 2;
                        d10I += 1;
                        c20I += 2;
                        c10I += 1;
                        if (change == "")
                        {
                            change = "Enchange a $50 bill for ";
                        }
                        else
                        {
                            c50I--;
                        }
                    }
                    else if (d100I > 0)
                    {
                        d100I--;
                        d20I += 5;
                        c20I += 5;
                        if (change == "")
                        {
                            change = "Enchange a $100 bill for ";
                        }
                        else
                        {
                            change = "Error";
                        }
                    }
                }
            }
            double tCashTotalD = d100I * 100 + d50I * 50 + d20I * 20 + d10I * 10 + d5I * 5 + d1I + dp25I * .25 + dp10I * .1 + dp05I * .05 + dp01I * .01;
            double tGrandTotalD = tCardTotalD + tCashTotalD + tChecksTotalD + tOtherPaymentD;
            double tBalanceOSD = tGrandTotalD - totalDueFromDriverD;
            if (tBalanceOSD < 0)
            {
                overL.Font = new Font(FontFamily.GenericSansSerif, 8.25f, FontStyle.Regular);
                shortL.Font = new Font(FontFamily.GenericSansSerif, 8.25f, FontStyle.Bold);
            }
            else if (tBalanceOSD > 0)
            {
                overL.Font = new Font(FontFamily.GenericSansSerif, 8.25f, FontStyle.Bold);
                shortL.Font = new Font(FontFamily.GenericSansSerif, 8.25f, FontStyle.Regular);
            }
            else
            {
                overL.Font = new Font(FontFamily.GenericSansSerif, 8.25f, FontStyle.Regular);
                shortL.Font = new Font(FontFamily.GenericSansSerif, 8.25f, FontStyle.Regular);
            }
            tCashTotalL.Text = tCashTotalD.ToString("0.00");
            tCardTotalL.Text = tCardTotalD.ToString("0.00");
            tCheckTotalL.Text = tChecksTotalD.ToString("0.00");
            tOther2L.Text = tOtherPaymentD.ToString("0.00");
            tGrandTotalL.Text = tGrandTotalD.ToString("0.00");
            tBalanceOSL.Text = tBalanceOSD.ToString("0.00");
            if (tTotalTipsTB.Text == "")
                tTotalTipsTB.Text = totalTipsD.ToString("0.00");
            else
            {
                try
                {
                    tTotalTipsTB.Text = Convert.ToDouble(tTotalTipsTB.Text).ToString("0.00");
                }
                catch
                {
                    tTotalTipsTB.Text = totalTipsD.ToString("0.00");
                }
            }
            n1L.Text = d1I.ToString();
            t1L.Text = (d1I * 1).ToString("0.00");
            n5L.Text = d5I.ToString();
            t5L.Text = (d5I * 5).ToString("0.00");
            n10L.Text = d10I.ToString();
            t10L.Text = (d10I * 10).ToString("0.00");
            n20L.Text = d20I.ToString();
            t20L.Text = (d20I * 20).ToString("0.00");
            n50L.Text = d50I.ToString();
            t50L.Text = (d50I * 50).ToString("0.00");
            nOther1L.Text = d100I.ToString();
            tOther1L.Text = (d100I * 100).ToString("0.00");
            np01L.Text = dp01I.ToString();
            tp01L.Text = (dp01I * .01).ToString("0.00");
            np05L.Text = dp05I.ToString();
            tp05L.Text = (dp05I * .05).ToString("0.00");
            np10L.Text = dp10I.ToString();
            tp10L.Text = (dp10I * .10).ToString("0.00");
            np25L.Text = dp25I.ToString();
            tp25L.Text = (dp25I * .25).ToString("0.00");
            bool changeB = false;
            if (c50I == 1)
            {
                if (changeB)
                {
                    change += ",";
                }
                else
                {
                    changeB = true;
                }
                change += "1 $50 bill ";
            }
            else if (c50I > 1)
            {
                if (changeB)
                {
                    change += ",";
                }
                else
                {
                    changeB = true;
                }
                change += c50I.ToString() + " $50 bills ";
            }
            if (c20I == 1)
            {
                if (changeB)
                {
                    change += ",";
                }
                else
                {
                    changeB = true;
                }
                change += "1 $20 bill ";
            }
            else if (c20I > 1)
            {
                if (changeB)
                {
                    change += ",";
                }
                else
                {
                    changeB = true;
                }
                change += c20I.ToString() + " $20 bills ";
            }
            if (c10I == 1)
            {
                if (changeB)
                {
                    change += ",";
                }
                else
                {
                    changeB = true;
                }
                change += "1 $10 bill ";
            }
            else if (c10I > 1)
            {
                if (changeB)
                {
                    change += ",";
                }
                else
                {
                    changeB = true;
                }
                change += c10I.ToString() + " $10 bills ";
            }
            if (c5I == 1)
            {
                if (changeB)
                {
                    change += ",";
                }
                else
                {
                    changeB = true;
                }
                change += "1 $5 bill ";
            }
            else if (c5I > 1)
            {
                if (changeB)
                {
                    change += ",";
                }
                else
                {
                    changeB = true;
                }
                change += c5I.ToString() + " $5 bills ";
            }
            if (c1I == 1)
            {
                if (changeB)
                {
                    change += ",";
                }
                else
                {
                    changeB = true;
                }
                change += "1 $1 bill ";
            }
            else if (c1I > 1)
            {
                if (changeB)
                {
                    change += ",";
                }
                else
                {
                    changeB = true;
                }
                change += c1I.ToString() + " $1 bills ";
            }
            if (cp25I == 1)
            {
                if (changeB)
                {
                    change += ",";
                }
                else
                {
                    changeB = true;
                }
                change += "1 Quarter ";
            }
            else if (cp25I > 1)
            {
                if (changeB)
                {
                    change += ",";
                }
                else
                {
                    changeB = true;
                }
                change += cp25I.ToString() + " Quarters ";
            }
            if (cp10I == 1)
            {
                if (changeB)
                {
                    change += ",";
                }
                else
                {
                    changeB = true;
                }
                change += "1 Dime ";
            }
            else if (cp10I > 1)
            {
                if (changeB)
                {
                    change += ",";
                }
                else
                {
                    changeB = true;
                }
                change += cp10I.ToString() + " Dimes ";
            }
            if (cp05I == 1)
            {
                if (changeB)
                {
                    change += ",";
                }
                else
                {
                    changeB = true;
                }
                change += "1 Nickle ";
            }
            else if (cp05I > 1)
            {
                if (changeB)
                {
                    change += ",";
                }
                else
                {
                    changeB = true;
                }
                change += cp05I.ToString() + " Nickles ";
            }
            if (cp01I == 1)
            {
                if (changeB)
                {
                    change += ",";
                }
                else
                {
                    changeB = true;
                }
                change += "1 Penny ";
            }
            else if (cp01I > 1)
            {
                if (changeB)
                {
                    change += ",";
                }
                else
                {
                    changeB = true;
                }
                change += cp01I.ToString() + " Pennies ";
            }
            if (change != "")
                MessageBox.Show(change, "Get Change");
        }
    }
}
