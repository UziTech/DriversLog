using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;

namespace DriversLog
{
    public partial class Form1 : Form
    {
        #region Global Variables
        Calculator _calculator = new Calculator();
        bool _calc = false, _saved = true, _print = false;
        DateTime _minDate, _maxDate;
        string _driverName = "", _pass = "", _driverList = "", _tempDirPath = "";
        int _lines = 0, _firstLineIndex = 0, _lastLineIndex = 0, _pageNum, _numDrivers = 0;
        Font _printFont;
        #endregion
        public Form1()
        {
            Form1_Start();
        }
        public Form1(string s)
        {
            if (s.Substring(0, s.LastIndexOf('\\')).ToLower() != Properties.Settings.Default.dirPath.ToLower())
            {
                _tempDirPath = Properties.Settings.Default.dirPath;
                Properties.Settings.Default.dirPath = s.Substring(0, s.LastIndexOf('\\'));
                Properties.Settings.Default.Save();
            }
            Form1_Start();
            Names_Click(s.Substring(s.LastIndexOf("\\") + 1, s.Length - s.LastIndexOf("\\") - 6), null);
        }
        private void Form1_Start()
        {
            InitializeComponent();
            _calculator.FormClosed += new FormClosedEventHandler(_calculator_FormClosed);
            if (!Properties.Settings.Default.SettingsViewed)
            {
                settingsToolStripMenuItem_Click(null, null);
                if (!Properties.Settings.Default.SettingsViewed)
                {
                    Application.Exit();
                    return;
                }
            }
            if (!Directory.Exists(Properties.Settings.Default.dirPath))
            {
                Properties.Settings.Default.dirPath = "";
                Properties.Settings.Default.Save();
                settingsToolStripMenuItem_Click(null, null);
                if (!Directory.Exists(Properties.Settings.Default.dirPath))
                {
                    Application.Exit();
                    return;
                }
            }
            LoadDrivers();
            if (Properties.Settings.Default.Maximize)
                this.WindowState = FormWindowState.Maximized;
            Form1_Resize(null, null);
            if (_numDrivers != 0)
            {
                driversToolStripMenuItem.Visible = true;
                editDriverToolStripMenuItem.Visible = true;
                removeDriverToolStripMenuItem.Visible = true;
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_driverName != "" && !_saved)
            {
                DialogResult r = MessageBox.Show("You have not saved your work.\n Save your work now?", "Save?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (r == DialogResult.Cancel)
                    e.Cancel = true;
                else if (r == DialogResult.Yes)
                    Save(Properties.Settings.Default.dirPath + "\\" + _driverName + ".Dlog", true);
            }
            if (this.WindowState == FormWindowState.Maximized)
                Properties.Settings.Default.Maximize = true;
            else
                Properties.Settings.Default.Maximize = false;
            if (_tempDirPath != "")
                Properties.Settings.Default.dirPath = _tempDirPath;
            Properties.Settings.Default.Save();
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            ETPTB.Location = new Point(ETPTB.Location.X, ClientSize.Height - 23);
            MCTB.Location = new Point(MCTB.Location.X, ClientSize.Height - 23);
            CPTB.Location = new Point(CPTB.Location.X, ClientSize.Height - 23);
            BPTB.Location = new Point(BPTB.Location.X, ClientSize.Height - 23);
            PPDlvTB.Location = new Point(PPDlvTB.Location.X, ClientSize.Height - 23);
            label6.Location = new Point(label6.Location.X, ClientSize.Height - 20);
            label7.Location = new Point(label7.Location.X, ClientSize.Height - 20);
            label8.Location = new Point(label8.Location.X, ClientSize.Height - 20);
            label9.Location = new Point(label9.Location.X, ClientSize.Height - 20);
            label10.Location = new Point(label10.Location.X, ClientSize.Height - 20);
            panel1.Size = new Size(ClientSize.Width, ClientSize.Height - 80);
            Properties.Settings.Default.ClientSize = ClientSize;
            Properties.Settings.Default.Save();
        }
        private void _calculator_FormClosed(object sender, FormClosedEventArgs e)
        {
            _calculator = new Calculator();
            _calculator.FormClosed += new FormClosedEventHandler(_calculator_FormClosed);
        }
        private void ReadOnly_Focus(object sender, EventArgs e)
        {
            BPTB.Focus();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BPTB.Focus();
            AboutBox a = new AboutBox();
            a.ShowDialog();
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BPTB.Focus();
            Save(Properties.Settings.Default.dirPath + "\\" + _driverName + ".Dlog", true);
        }
        private void newDriverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BPTB.Focus();
            NewDriver n = new NewDriver(_driverList);
            n.ShowDialog();
            if (n._open)
            {
                _driverList = n._driverList;
                _numDrivers++;
                AddDrivers();
                if (!_saved)
                {
                    DialogResult r = MessageBox.Show("You have not saved your work.\n Save your work now?", "Save?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (r == DialogResult.Cancel)
                        return;
                    else if (r == DialogResult.Yes)
                        Save(Properties.Settings.Default.dirPath + "\\" + _driverName + ".Dlog", true);
                }
                _driverName = n._driverName;
                _pass = n._pass;
                Names_Click(_driverName, e);
                driversToolStripMenuItem.Visible = true;
                editDriverToolStripMenuItem.Visible = true;
                removeDriverToolStripMenuItem.Visible = true;
            }
        }
        private void showDatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BPTB.Focus();
            if (_driverName != "" && _lines > 1)
            {
                ShowDates sd = new ShowDates(_minDate, _maxDate);
                sd.ShowDialog();
                if (sd._startDate != DateTime.MinValue)
                {
                    BPTB.Focus();
                    Show(sd._startDate, sd._endDate);
                }
            }
        }
        private void newLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BPTB.Focus();
            if (_driverName != "")
            {
                _calc = false;
                Save(Properties.Settings.Default.dirPath + "\\" + _driverName + ".Dlog.tmp", false);
                AddComponents(++_lines);
                _lastLineIndex = _lines - 1;
                DateTB[0].Text = System.DateTime.Today.ToString("M/d/yy");
                Open(Properties.Settings.Default.dirPath + "\\" + _driverName + ".Dlog.tmp", true);
                File.Delete(Properties.Settings.Default.dirPath + "\\" + _driverName + ".Dlog.tmp");
                _saved = false;
                if (this.Text == "Driver's Log : " + _driverName)
                    this.Text += "*";
                SMilesTB[0].Focus();
            }
        }
        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BPTB.Focus();
            if (_driverName != "")
            {
                Password p = new Password(true, _driverName, _driverName + @"'s New Password");
                p.ShowDialog();
                if (p._open)
                {
                    _pass = p._password;
                    _saved = false;
                    if (this.Text == "Driver's Log : " + _driverName)
                        this.Text += "*";
                }
            }
        }
        private void removeLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BPTB.Focus();
            RemoveLine r = new RemoveLine(_lines);
            if (_driverName != "" && _lines > 0)
            {
                r.ShowDialog();
                if (r._lineNum != 0)
                {
                    _calc = false;
                    Save(Properties.Settings.Default.dirPath + "\\" + _driverName + ".tmp.log", false, r._lineNum);
                    AddComponents(--_lines);
                    _firstLineIndex = 0;
                    _lastLineIndex = _lines - 1;
                    for (int i = 0; i < _lines; i++)
                    {
                        DateTB[i].Visible = true;
                        SMilesTB[i].Visible = true;
                        EMilesTB[i].Visible = true;
                        TMilesTB[i].Visible = true;
                        TimeITB[i].Visible = true;
                        TimeOTB[i].Visible = true;
                        TTimeTB[i].Visible = true;
                        TSalesTB[i].Visible = true;
                        CTipsTB[i].Visible = true;
                        TTipsTB[i].Visible = true;
                        CaTipsTB[i].Visible = true;
                        DlvsTB[i].Visible = true;
                    }
                    Open(Properties.Settings.Default.dirPath + "\\" + _driverName + ".tmp.log", false);
                    File.Delete(Properties.Settings.Default.dirPath + "\\" + _driverName + ".tmp.log");
                    _saved = false;
                    if (this.Text == "Driver's Log : " + _driverName)
                        this.Text += "*";
                }
            }
        }
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_driverName != "" && _lines > 0)
            {
                Print p = new Print(_driverName);
                p.ShowDialog();
                if (p._print)
                {
                    _printFont = new System.Drawing.Font(FontFamily.GenericMonospace, 12, FontStyle.Bold);
                    _pageNum = 1;
                    printDocument1.DefaultPageSettings.Landscape = false;
                    if (printDialog1.ShowDialog() == DialogResult.OK)
                    {
                        _print = true;
                        printPreviewDialog1.PrintPreviewControl.Zoom = 1;
                        printPreviewDialog1.PrintPreviewControl.AutoZoom = false;
                        printPreviewDialog1.ShowDialog();
                        _print = false;
                    }
                }
            }
        }
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Password p = new Password(!Properties.Settings.Default.SettingsViewed, null, "Settings Password");
            p.ShowDialog();
            if (p._open)
            {
                string tempDir;
                int tempNumLinesDays;
                bool tempShowLines;
                Properties.Settings.Default.SettingsPass = p._password;
                Properties.Settings.Default.Save();
                tempDir = Properties.Settings.Default.dirPath;
                tempNumLinesDays = Properties.Settings.Default.NumLinesDays;
                tempShowLines = Properties.Settings.Default.ShowLines;
                Settings s = new Settings();
                s.ShowDialog();
                if (_driverName != "" && tempDir != Properties.Settings.Default.dirPath)
                {
                    if (!_saved)
                    {
                        DialogResult r = MessageBox.Show("You have not saved your work.\n Save your work now?", "Save?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        if (r == DialogResult.Yes)
                            Save(tempDir + "\\" + _driverName + ".Dlog", true);
                    }
                    _calc = false;
                    this.Text = "Driver's Log";
                    panel1.Controls.Clear();
                    _driverName = "";
                    saveToolStripMenuItem.Visible = false;
                    printToolStripMenuItem.Visible = false;
                    changePasswordToolStripMenuItem.Visible = false;
                    showDatesToolStripMenuItem.Visible = false;
                    newLineToolStripMenuItem.Visible = false;
                    removeLineToolStripMenuItem.Visible = false;
                }
                if (_driverName != "" && (tempNumLinesDays != Properties.Settings.Default.NumLinesDays || tempShowLines != Properties.Settings.Default.ShowLines))
                {
                    if (!_saved)
                    {
                        DialogResult r = MessageBox.Show("You have not saved your work.\n Save your work now?", "Save?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        if (r == DialogResult.Yes)
                            Save(tempDir + "\\" + _driverName + ".Dlog", true);
                    }
                    if (Properties.Settings.Default.NumLinesDays != 0)
                    {
                        if (Properties.Settings.Default.ShowLines)
                        {
                            Show(1, (_lines < Properties.Settings.Default.NumLinesDays ? _lines : Properties.Settings.Default.NumLinesDays));
                        }
                        else
                        {
                            Show(DateTime.Today.Subtract(TimeSpan.FromDays(Properties.Settings.Default.NumLinesDays - 1)), DateTime.Today);
                        }
                    }
                    else
                    {
                        Show(1, _lines);
                    }
                }
                LoadDrivers();
            }
        }
        private void calculatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _calculator.Show();
            if (_calculator.WindowState == FormWindowState.Minimized)
                _calculator.WindowState = FormWindowState.Normal;
            _calculator.BringToFront();
        }
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            if (!_print)
            {
                e.Cancel = true;
                return;
            }
            float yPos = 0f;
            float leftMargin = e.MarginBounds.Left;
            float topMargin = e.MarginBounds.Top;
            float rightMargin = e.MarginBounds.Right;
            string line = null;
            string[] lines = WriteFile().Split('\n');
            float stringWidth = 0f;
            int longestString = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                float sw = e.Graphics.MeasureString(lines[i], _printFont).Width;
                if (sw > stringWidth)
                {
                    stringWidth = sw;
                    longestString = i;
                }
            }
            while (e.Graphics.MeasureString(lines[longestString], _printFont).Width > rightMargin - leftMargin)
            {
                _printFont = new Font(FontFamily.GenericMonospace, _printFont.SizeInPoints - .1f, FontStyle.Bold);
                if (!printDocument1.DefaultPageSettings.Landscape && _printFont.SizeInPoints < 8)
                {
                    printDocument1.DefaultPageSettings.Landscape = true;
                    _printFont = new Font(FontFamily.GenericMonospace, 12, FontStyle.Bold);
                    printDocument1_PrintPage(sender, e);
                    return;
                }
                if (_printFont.SizeInPoints < 4)
                {
                    MessageBox.Show("Page too small or margins too big.");
                    e.Cancel = true;
                    return;
                }
            }
            float linesPerPage = e.MarginBounds.Height / _printFont.GetHeight(e.Graphics);
            int count = _pageNum / (int)Math.Floor(linesPerPage);
            while (count < linesPerPage && count < lines.Length)
            {
                line = lines[count];
                if (line == null)
                {
                    break;
                }
                yPos = topMargin + count * _printFont.GetHeight(e.Graphics);
                e.Graphics.DrawString(line, _printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
                count++;
            }
            if (count < lines.Length)
            {
                e.HasMorePages = true;
                _pageNum++;
            }
        }
        private void LoadDrivers()
        {
            _numDrivers = 0;
            _driverList = "";
            string[] s = Directory.GetFiles(Properties.Settings.Default.dirPath);
            foreach (string st in s)
            {
                if (st.ToLower().EndsWith(".dlog"))
                {
                    _numDrivers++;
                    _driverList += "|" + st.Substring(st.LastIndexOf("\\") + 1, st.Length - st.LastIndexOf("\\") - 6) + "|";
                }
            }
            Properties.Settings.Default.Save();
            if (_numDrivers > 0)
                AddDrivers();
        }
        private void Names_Click(object sender, EventArgs e)
        {
            BPTB.Focus();
            if (sender.ToString() != _driverName)
            {
                if (_driverName != "" && !_saved)
                {
                    DialogResult r = MessageBox.Show("You have not saved your work.\n Save your work now?", "Save?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (r == DialogResult.Cancel)
                        return;
                    else if (r == DialogResult.Yes)
                        Save(Properties.Settings.Default.dirPath + "\\" + _driverName + ".Dlog", true);
                }
                string s = _driverName;
                for (int i = 0; i < _numDrivers; i++)
                {
                    if (sender == namesToolStripMenuItem[i] || sender.ToString() == namesToolStripMenuItem[i].Text)
                    {
                        _driverName = namesToolStripMenuItem[i].Text;
                        break;
                    }
                }
                Password p = new Password(false, _driverName, _driverName + @"'s Password");
                p.ShowDialog();
                if (!p._open)
                {
                    _driverName = s;
                    return;
                }
                _pass = p._password;
            }
            _calc = false;
            BPTB.Text = "";
            CPTB.Text = "";
            MCTB.Text = "";
            ETPTB.Text = "";
            StreamReader fileR = new StreamReader(Properties.Settings.Default.dirPath + "\\" + _driverName + ".Dlog");
            _lines = -1;
            while (!fileR.EndOfStream)
            {
                fileR.ReadLine();
                _lines++;
            }
            fileR.Close();
            _firstLineIndex = 0;
            _lastLineIndex = _lines - 1;
            AddComponents(_lines);
            Open(Properties.Settings.Default.dirPath + "\\" + _driverName + ".Dlog", false);
            if (_lines > 0)
            {
                if (SMilesTB[0].Text == "")
                    SMilesTB[0].Focus();
                else if (EMilesTB[0].Text == "")
                    EMilesTB[0].Focus();
                else if (TimeITB[0].Text == "")
                    TimeITB[0].Focus();
                else if (TimeOTB[0].Text == "")
                    TimeOTB[0].Focus();
                else if (TSalesTB[0].Text == "")
                    TSalesTB[0].Focus();
                else if (CTipsTB[0].Text == "")
                    CTipsTB[0].Focus();
                else if (TTipsTB[0].Text == "")
                    TTipsTB[0].Focus();
                else if (DlvsTB[0].Text == "")
                    DlvsTB[0].Focus();
                else
                    DateTB[0].Focus();
            }
            _saved = true;
            this.Text = "Driver's Log : " + _driverName;
        }
        private void RemoveNames_Click(object sender, EventArgs e)
        {
            BPTB.Focus();
            for (int i = 0; i < _numDrivers; i++)
            {
                if (sender == removeNamesToolStripMenuItem[i])
                {
                    if (MessageBox.Show("Are you sure you want to remove " + removeNamesToolStripMenuItem[i].Text + " completely?", "Remove " + removeNamesToolStripMenuItem[i].Text + "?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        if (removeNamesToolStripMenuItem[i].Text == _driverName)
                        {
                            _calc = false;
                            this.Text = "Driver's Log";
                            panel1.Controls.Clear();
                            _driverName = "";
                            saveToolStripMenuItem.Visible = false;
                            printToolStripMenuItem.Visible = false;
                            changePasswordToolStripMenuItem.Visible = false;
                            showDatesToolStripMenuItem.Visible = false;
                            newLineToolStripMenuItem.Visible = false;
                            removeLineToolStripMenuItem.Visible = false;
                        }
                        _driverList = _driverList.Replace("|" + removeNamesToolStripMenuItem[i].Text + "|", "");
                        _numDrivers--;
                        Properties.Settings.Default.Save();
                        if (_numDrivers == 0)
                        {
                            editDriverToolStripMenuItem.Visible = false;
                            removeDriverToolStripMenuItem.Visible = false;
                            driversToolStripMenuItem.Visible = false;
                        }
                        File.Delete(Properties.Settings.Default.dirPath + "\\" + removeNamesToolStripMenuItem[i].Text + ".Dlog");
                        AddDrivers();
                    }
                    break;
                }
            }
        }
        private void EditNames_Click(object sender, EventArgs e)
        {
            BPTB.Focus();
            for(int i = 0; i < _numDrivers; i++)
            {
                if (sender == editNamesToolStripMenuItem[i])
                {
                    EditDriver ed = new EditDriver(editNamesToolStripMenuItem[i].Text, _driverList);
                    ed.ShowDialog();
                    _driverList = ed._driverList;
                    if (editNamesToolStripMenuItem[i].Text == _driverName)
                    {
                        _driverName = ed._name;
                        this.Text = "Driver's Log : " + _driverName;
                    }
                    AddDrivers();
                    break;
                }
            }
        }
        private void DateTB_TextChanged(object sender, EventArgs e)
        {
            _saved = false;
            if (this.Text == "Driver's Log : " + _driverName)
                this.Text += "*";
        }
        private void SMilesTB_TextChanged(object sender, EventArgs e)
        {
            _saved = false;
            if (this.Text == "Driver's Log : " + _driverName)
                this.Text += "*";
            if (_calc)
            {
                for (int i = _firstLineIndex; i <= _lastLineIndex; i++)
                {
                    if (sender == SMilesTB[i])
                    {
                        MessageBox.Show("before try\r\n 2");
                        try
                        {

                            MessageBox.Show("inside try\r\n 3");
                            if (Convert.ToInt32(SMilesTB[i].Text) <= Convert.ToInt32(EMilesTB[i].Text))
                            {
                                MessageBox.Show("inside if\r\n 4");
                                TMilesTB[i].Text = (Convert.ToInt32(EMilesTB[i].Text) - Convert.ToInt32(SMilesTB[i].Text)).ToString();

                            }
                            else
                            {
                                MessageBox.Show("inside else\r\n 5");
                                TMilesTB[i].Text = "";
                            }
                        }
                        catch
                        {
                            MessageBox.Show("inside catch\r\n 6");
                            TMilesTB[i].Text = ""; }
                        break;
                    }
                }
            }
        }
        private void EMilesTB_TextChanged(object sender, EventArgs e)
        {
            _saved = false;
            if (this.Text == "Driver's Log : " + _driverName)
                this.Text += "*";
            if (_calc)
            {
                for (int i = _firstLineIndex; i <= _lastLineIndex; i++)
                {
                    if (sender == EMilesTB[i])
                    {
                        MessageBox.Show("before try\r\n 2");
                        try
                        {
                            MessageBox.Show("inside try\r\n 3");
                            if (Convert.ToInt32(SMilesTB[i].Text) <= Convert.ToInt32(EMilesTB[i].Text))
                            {
                                MessageBox.Show("inside if\r\n 4");
                                TMilesTB[i].Text = (Convert.ToInt32(EMilesTB[i].Text) - Convert.ToInt32(SMilesTB[i].Text)).ToString();
                            }
                            else
                            {
                                MessageBox.Show("inside else\r\n 5");
                                TMilesTB[i].Text = "";
                            }
                        }
                        catch
                        {
                            MessageBox.Show("inside catch\r\n 6");
                            TMilesTB[i].Text = ""; }
                        break;
                    }
                }
            }
        }
        private void TimeITB_TextChanged(object sender, EventArgs e)
        {
            _saved = false;
            if (this.Text == "Driver's Log : " + _driverName)
                this.Text += "*";
            if (_calc)
            {
                for (int i = _firstLineIndex; i <= _lastLineIndex; i++)
                {
                    if (sender == TimeITB[i])
                    {
                        try
                        {
                            if (((TimeITB[i].Text.IndexOf(':') == 1 && TimeITB[i].Text.Length == 4) || (TimeITB[i].Text.IndexOf(':') == 2 && TimeITB[i].Text.Length == 5)) && ((TimeOTB[i].Text.IndexOf(':') == 1 && TimeOTB[i].Text.Length == 4) || (TimeOTB[i].Text.IndexOf(':') == 2 && TimeOTB[i].Text.Length == 5)))
                            {
                                int hours = Convert.ToInt32(TimeOTB[i].Text.Substring(0, TimeOTB[i].Text.IndexOf(':'))) - Convert.ToInt32(TimeITB[i].Text.Substring(0, TimeITB[i].Text.IndexOf(':')));
                                if (hours < 0)
                                    hours += 12;
                                int minutes = Convert.ToInt32(TimeOTB[i].Text.Substring(TimeOTB[i].Text.IndexOf(':') + 1, 2)) - Convert.ToInt32(TimeITB[i].Text.Substring(TimeITB[i].Text.IndexOf(':') + 1, 2));
                                if (minutes < 0)
                                {
                                    hours--;
                                    minutes += 60;
                                }
                                TTimeTB[i].Text = hours.ToString() + ":" + ((minutes < 10) ? "0" : "") + minutes.ToString();
                            }
                        }
                        catch { TTimeTB[i].Text = ""; }
                        break;
                    }
                }
            }
        }
        private void TimeOTB_TextChanged(object sender, EventArgs e)
        {
            _saved = false;
            if (this.Text == "Driver's Log : " + _driverName)
                this.Text += "*";
            if (_calc)
            {
                for (int i = _firstLineIndex; i <= _lastLineIndex; i++)
                {
                    if (sender == TimeOTB[i])
                    {
                        try
                        {
                            if (((TimeITB[i].Text.IndexOf(':') == 1 && TimeITB[i].Text.Length == 4) || (TimeITB[i].Text.IndexOf(':') == 2 && TimeITB[i].Text.Length == 5)) && ((TimeOTB[i].Text.IndexOf(':') == 1 && TimeOTB[i].Text.Length == 4) || (TimeOTB[i].Text.IndexOf(':') == 2 && TimeOTB[i].Text.Length == 5)))
                            {
                                int hours = Convert.ToInt32(TimeOTB[i].Text.Substring(0, TimeOTB[i].Text.IndexOf(':'))) - Convert.ToInt32(TimeITB[i].Text.Substring(0, TimeITB[i].Text.IndexOf(':')));
                                if (hours < 0)
                                    hours += 12;
                                int minutes = Convert.ToInt32(TimeOTB[i].Text.Substring(TimeOTB[i].Text.IndexOf(':') + 1, 2)) - Convert.ToInt32(TimeITB[i].Text.Substring(TimeITB[i].Text.IndexOf(':') + 1, 2));
                                if (minutes < 0)
                                {
                                    hours--;
                                    minutes += 60;
                                }
                                TTimeTB[i].Text = hours.ToString() + ":" + ((minutes < 10) ? "0" : "") + minutes.ToString();
                            }
                        }
                        catch { TTimeTB[i].Text = ""; }
                        break;
                    }
                }
            }
        }
        private void TSalesTB_TextChanged(object sender, EventArgs e)
        {
            _saved = false;
            if (this.Text == "Driver's Log : " + _driverName)
                this.Text += "*";
        }
        private void CTipsTB_TextChanged(object sender, EventArgs e)
        {
            _saved = false;
            if (this.Text == "Driver's Log : " + _driverName)
                this.Text += "*";
        }
        private void TTipsTB_TextChanged(object sender, EventArgs e)
        {
            _saved = false;
            if (this.Text == "Driver's Log : " + _driverName)
                this.Text += "*";
            if (_calc)
            {
                for (int i = _firstLineIndex; i <= _lastLineIndex; i++)
                {
                    if (sender == TTipsTB[i])
                    {
                        try
                        {
                            CaTipsTB[i].Text = (Convert.ToDouble(TTipsTB[i].Text) - Convert.ToDouble(CTipsTB[i].Text)).ToString("0.00");
                        }
                        catch { CaTipsTB[i].Text = ""; }
                        break;
                    }
                }
            }
        }
        private void DlvsTB_TextChanged(object sender, EventArgs e)
        {
            _saved = false;
            if (this.Text == "Driver's Log : " + _driverName)
                this.Text += "*";
        }
        private void BPTB_TextChanged(object sender, EventArgs e)
        {
            _saved = false;
            if (this.Text == "Driver's Log : " + _driverName)
                this.Text += "*";
        }
        private void PPDlvTB_TextChanged(object sender, EventArgs e)
        {
            _saved = false;
            if (this.Text == "Driver's Log : " + _driverName)
                this.Text += "*";
        }
        private void CPTB_TextChanged(object sender, EventArgs e)
        {
            _saved = false;
            if (this.Text == "Driver's Log : " + _driverName)
                this.Text += "*";
        }
        private void MCTB_TextChanged(object sender, EventArgs e)
        {
            _saved = false;
            if (this.Text == "Driver's Log : " + _driverName)
                this.Text += "*";
        }
        private void ETPTB_TextChanged(object sender, EventArgs e)
        {
            _saved = false;
            if (this.Text == "Driver's Log : " + _driverName)
                this.Text += "*";
        }
        private void TSalesTB_Leave(object sender, EventArgs e)
        {
            for (int i = _firstLineIndex; i <= _lastLineIndex; i++)
            {
                if (sender == TSalesTB[i])
                {
                    if (TSalesTB[i].Text != "")
                        TSalesTB[i].Text = Convert.ToDecimal(TSalesTB[i].Text).ToString("0.00");
                    break;
                }
            }
        }
        private void CTipsTB_Leave(object sender, EventArgs e)
        {
            for (int i = _firstLineIndex; i <= _lastLineIndex; i++)
            {
                if (sender == CTipsTB[i])
                {
                    if(CTipsTB[i].Text != "")
                        CTipsTB[i].Text = Convert.ToDouble(CTipsTB[i].Text).ToString("0.00");
                    break;
                }
            }
        }
        private void TTipsTB_Leave(object sender, EventArgs e)
        {
            for (int i = _firstLineIndex; i <= _lastLineIndex; i++)
            {
                if (sender == TTipsTB[i])
                {
                    if(TTipsTB[i].Text != "")
                        TTipsTB[i].Text = Convert.ToDecimal(TTipsTB[i].Text).ToString("0.00");
                    break;
                }
            }
        }
        private void TimeITB_Leave(object sender, EventArgs e)
        {
            for (int i = _firstLineIndex; i <= _lastLineIndex; i++)
            {
                if (sender == TimeITB[i])
                {
                    try
                    {
                        if (TimeITB[i].Text != "" && TimeITB[i].Text.Length < 4)
                            TimeITB[i].AppendText(":00");
                        break;
                    }
                    catch { }
                    if (TimeITB[i].Text == "")
                        TTimeTB[i].Text = "";
                }
            }
        }
        private void TimeOTB_Leave(object sender, EventArgs e)
        {
            for (int i = _firstLineIndex; i <= _lastLineIndex; i++)
            {
                if (sender == TimeOTB[i])
                {
                    try
                    {
                        if (TimeOTB[i].Text != "" && TimeOTB[i].Text.Length < 4)
                            TimeOTB[i].AppendText(":00");
                        break;
                    }
                    catch { }
                    if (TimeOTB[i].Text == "")
                        TTimeTB[i].Text = "";
                }
            }
        }
        private void DateTB_Leave(object sender, EventArgs e)
        {
            for (int i = _firstLineIndex; i <= _lastLineIndex; i++)
            {
                if (sender == DateTB[i])
                {
                    try
                    {
                        _minDate = DateTime.MaxValue;
                        _maxDate = DateTime.MinValue;
                        for (int j = 0; j < _lines; j++)
                        {
                            if (Convert.ToDateTime(DateTB[j].Text) < _minDate)
                                _minDate = Convert.ToDateTime(DateTB[j].Text);
                            if (Convert.ToDateTime(DateTB[j].Text) > _maxDate)
                                _maxDate = Convert.ToDateTime(DateTB[j].Text);
                        }
                        DateTB[i].Text = Convert.ToDateTime(DateTB[i].Text).ToString("M/d/yy");
                    }
                    catch { }
                    break;
                }
            }
        }
        private void BPTB_Leave(object sender, EventArgs e)
        {
            try
            {
                BPTB.Text = Convert.ToDouble(BPTB.Text).ToString("0.00");
            }
            catch { }
        }
        private void PPDlvTB_Leave(object sender, EventArgs e)
        {
            try
            {
                PPDlvTB.Text = Convert.ToDouble(PPDlvTB.Text).ToString("0.00");
            }
            catch { PPDlvTB.Text = ""; }
        }
        private void DateTB_KeyDown(object sender, KeyEventArgs e)
        {
            for (int i = _firstLineIndex; i <= _lastLineIndex; i++)
            {
                if (sender == DateTB[i])
                {
                    if (e.KeyCode.ToString() == "Left")
                    {
                        if (DateTB[i].SelectionStart == 0)
                        {
                            DlvsTB[i].Focus();
                            DlvsTB[i].SelectAll();
                        }
                    }
                    else if (e.KeyCode.ToString() == "Right")
                    {
                        if (DateTB[i].SelectionStart == DateTB[i].Text.Length || DateTB[i].SelectionLength == DateTB[i].Text.Length)
                        {
                            SMilesTB[i].Focus();
                            SMilesTB[i].SelectAll();
                        }
                    }
                    else if (e.KeyCode.ToString() == "Up")
                    {
                        if (i == _firstLineIndex)
                        {
                            DateTB[_lastLineIndex].Focus();
                            DateTB[_lastLineIndex].SelectAll();
                        }
                        else
                        {
                            for (int j = i; j > _firstLineIndex; j--)
                            {
                                if (DateTB[j - 1].Visible)
                                {
                                    DateTB[j - 1].Focus();
                                    DateTB[j - 1].SelectAll();
                                    break;
                                }
                            }
                        }
                    }
                    else if (e.KeyCode.ToString() == "Down")
                    {
                        if (i == _lastLineIndex)
                        {
                            BPTB.Focus();
                            BPTB.SelectAll();
                        }
                        else
                        {
                            for (int j = i; j < _lastLineIndex; j++)
                            {
                                if (DateTB[j + 1].Visible)
                                {
                                    DateTB[j + 1].Focus();
                                    DateTB[j + 1].SelectAll();
                                    break;
                                }
                            }
                        }
                    }
                    break;
                }
            }
        }
        private void SMilesTB_KeyDown(object sender, KeyEventArgs e)
        {
            for (int i = _firstLineIndex; i <= _lastLineIndex; i++)
            {
                if (sender == SMilesTB[i])
                {
                    if (e.KeyCode.ToString() == "Left")
                    {
                        if (SMilesTB[i].SelectionStart == 0)
                        {
                            DateTB[i].Focus();
                            DateTB[i].SelectAll();
                        }
                    }
                    else if (e.KeyCode.ToString() == "Right")
                    {
                        if (SMilesTB[i].SelectionStart == SMilesTB[i].Text.Length || SMilesTB[i].SelectionLength == SMilesTB[i].Text.Length)
                        {
                            EMilesTB[i].Focus();
                            EMilesTB[i].SelectAll();
                        }
                    }
                    else if (e.KeyCode.ToString() == "Up")
                    {
                        if (i == _firstLineIndex)
                        {
                            SMilesTB[_lastLineIndex].Focus();
                            SMilesTB[_lastLineIndex].SelectAll();
                        }
                        else
                        {
                            for (int j = i; j > _firstLineIndex; j--)
                            {
                                if (SMilesTB[j - 1].Visible)
                                {
                                    SMilesTB[j - 1].Focus();
                                    SMilesTB[j - 1].SelectAll();
                                    break;
                                }
                            }
                        }
                    }
                    else if (e.KeyCode.ToString() == "Down")
                    {
                        if (i == _lastLineIndex)
                        {
                            BPTB.Focus();
                            BPTB.SelectAll();
                        }
                        else
                        {
                            for (int j = i; j < _lastLineIndex; j++)
                            {
                                if (SMilesTB[j + 1].Visible)
                                {
                                    SMilesTB[j + 1].Focus();
                                    SMilesTB[j + 1].SelectAll();
                                    break;
                                }
                            }
                        }
                    }
                    break;
                }
            }
        }
        private void EMilesTB_KeyDown(object sender, KeyEventArgs e)
        {
            for (int i = _firstLineIndex; i <= _lastLineIndex; i++)
            {
                if (sender == EMilesTB[i])
                {
                    if (e.KeyCode.ToString() == "Left")
                    {
                        if (EMilesTB[i].SelectionStart == 0)
                        {
                            SMilesTB[i].Focus();
                            SMilesTB[i].SelectAll();
                        }
                    }
                    else if (e.KeyCode.ToString() == "Right")
                    {
                        if (EMilesTB[i].SelectionStart == EMilesTB[i].Text.Length || EMilesTB[i].SelectionLength == EMilesTB[i].Text.Length)
                        {
                            TimeITB[i].Focus();
                            TimeITB[i].SelectAll();
                        }
                    }
                    else if (e.KeyCode.ToString() == "Up")
                    {
                        if (i == _firstLineIndex)
                        {
                            EMilesTB[_lastLineIndex].Focus();
                            EMilesTB[_lastLineIndex].SelectAll();
                        }
                        else
                        {
                            for (int j = i; j > _firstLineIndex; j--)
                            {
                                if (EMilesTB[j - 1].Visible)
                                {
                                    EMilesTB[j - 1].Focus();
                                    EMilesTB[j - 1].SelectAll();
                                    break;
                                }
                            }
                        }
                    }
                    else if (e.KeyCode.ToString() == "Down")
                    {
                        if (i == _lastLineIndex)
                        {
                            BPTB.Focus();
                            BPTB.SelectAll();
                        }
                        else
                        {
                            for (int j = i; j < _lastLineIndex; j++)
                            {
                                if (EMilesTB[j + 1].Visible)
                                {
                                    EMilesTB[j + 1].Focus();
                                    EMilesTB[j + 1].SelectAll();
                                    break;
                                }
                            }
                        }
                    }
                    break;
                }
            }
        }
        private void TimeITB_KeyDown(object sender, KeyEventArgs e)
        {
            for (int i = _firstLineIndex; i <= _lastLineIndex; i++)
            {
                if (sender == TimeITB[i])
                {
                    if (e.KeyCode.ToString() == "Left")
                    {
                        if (TimeITB[i].SelectionStart == 0)
                        {
                            EMilesTB[i].Focus();
                            EMilesTB[i].SelectAll();
                        }
                    }
                    else if (e.KeyCode.ToString() == "Right")
                    {
                        if (TimeITB[i].SelectionStart == TimeITB[i].Text.Length || TimeITB[i].SelectionLength == TimeITB[i].Text.Length)
                        {
                            TimeOTB[i].Focus();
                            TimeOTB[i].SelectAll();
                        }
                    }
                    else if (e.KeyCode.ToString() == "Up")
                    {
                        if (i == _firstLineIndex)
                        {
                            TimeITB[_lastLineIndex].Focus();
                            TimeITB[_lastLineIndex].SelectAll();
                        }
                        else
                        {
                            for (int j = i; j > _firstLineIndex; j--)
                            {
                                if (TimeITB[j - 1].Visible)
                                {
                                    TimeITB[j - 1].Focus();
                                    TimeITB[j - 1].SelectAll();
                                    break;
                                }
                            }
                        }
                    }
                    else if (e.KeyCode.ToString() == "Down")
                    {
                        if (i == _lastLineIndex)
                        {
                            BPTB.Focus();
                            BPTB.SelectAll();
                        }
                        else
                        {
                            for (int j = i; j < _lastLineIndex; j++)
                            {
                                if (TimeITB[j + 1].Visible)
                                {
                                    TimeITB[j + 1].Focus();
                                    TimeITB[j + 1].SelectAll();
                                    break;
                                }
                            }
                        }
                    }
                    break;
                }
            }
        }
        private void TimeOTB_KeyDown(object sender, KeyEventArgs e)
        {
            for (int i = _firstLineIndex; i <= _lastLineIndex; i++)
            {
                if (sender == TimeOTB[i])
                {
                    if (e.KeyCode.ToString() == "Left")
                    {
                        if (TimeOTB[i].SelectionStart == 0)
                        {
                            TimeITB[i].Focus();
                            TimeITB[i].SelectAll();
                        }
                    }
                    else if (e.KeyCode.ToString() == "Right")
                    {
                        if (TimeOTB[i].SelectionStart == TimeOTB[i].Text.Length || TimeOTB[i].SelectionLength == TimeOTB[i].Text.Length)
                        {
                            TSalesTB[i].Focus();
                            TSalesTB[i].SelectAll();
                        }
                    }
                    else if (e.KeyCode.ToString() == "Up")
                    {
                        if (i == _firstLineIndex)
                        {
                            TimeOTB[_lastLineIndex].Focus();
                            TimeOTB[_lastLineIndex].SelectAll();
                        }
                        else
                        {
                            for (int j = i; j > _firstLineIndex; j--)
                            {
                                if (TimeOTB[j - 1].Visible)
                                {
                                    TimeOTB[j - 1].Focus();
                                    TimeOTB[j - 1].SelectAll();
                                    break;
                                }
                            }
                        }
                    }
                    else if (e.KeyCode.ToString() == "Down")
                    {
                        if (i == _lastLineIndex)
                        {
                            BPTB.Focus();
                            BPTB.SelectAll();
                        }
                        else
                        {
                            for (int j = i; j < _lastLineIndex; j++)
                            {
                                if (TimeOTB[j + 1].Visible)
                                {
                                    TimeOTB[j + 1].Focus();
                                    TimeOTB[j + 1].SelectAll();
                                    break;
                                }
                            }
                        }
                    }
                    break;
                }
            }
        }
        private void TSalesTB_KeyDown(object sender, KeyEventArgs e)
        {
            for (int i = _firstLineIndex; i <= _lastLineIndex; i++)
            {
                if (sender == TSalesTB[i])
                {
                    if (e.KeyCode.ToString() == "Left")
                    {
                        if (TSalesTB[i].SelectionStart == 0)
                        {
                            TimeOTB[i].Focus();
                            TimeOTB[i].SelectAll();
                        }
                    }
                    else if (e.KeyCode.ToString() == "Right")
                    {
                        if (TSalesTB[i].SelectionStart == TSalesTB[i].Text.Length || TSalesTB[i].SelectionLength == TSalesTB[i].Text.Length)
                        {
                            CTipsTB[i].Focus();
                            CTipsTB[i].SelectAll();
                        }
                    }
                    else if (e.KeyCode.ToString() == "Up")
                    {
                        if (i == _firstLineIndex)
                        {
                            TSalesTB[_lastLineIndex].Focus();
                            TSalesTB[_lastLineIndex].SelectAll();
                        }
                        else
                        {
                            for (int j = i; j > _firstLineIndex; j--)
                            {
                                if (TSalesTB[j - 1].Visible)
                                {
                                    TSalesTB[j - 1].Focus();
                                    TSalesTB[j - 1].SelectAll();
                                    break;
                                }
                            }
                        }
                    }
                    else if (e.KeyCode.ToString() == "Down")
                    {
                        if (i == _lastLineIndex)
                        {
                            BPTB.Focus();
                            BPTB.SelectAll();
                        }
                        else
                        {
                            for (int j = i; j < _lastLineIndex; j++)
                            {
                                if (TSalesTB[j + 1].Visible)
                                {
                                    TSalesTB[j + 1].Focus();
                                    TSalesTB[j + 1].SelectAll();
                                    break;
                                }
                            }
                        }
                    }
                    break;
                }
            }
        }
        private void CTipsTB_KeyDown(object sender, KeyEventArgs e)
        {
            for (int i = _firstLineIndex; i <= _lastLineIndex; i++)
            {
                if (sender == CTipsTB[i])
                {
                    if (e.KeyCode.ToString() == "Left")
                    {
                        if (CTipsTB[i].SelectionStart == 0)
                        {
                            TSalesTB[i].Focus();
                            TSalesTB[i].SelectAll();
                        }
                    }
                    else if (e.KeyCode.ToString() == "Right")
                    {
                        if (CTipsTB[i].SelectionStart == CTipsTB[i].Text.Length || CTipsTB[i].SelectionLength == CTipsTB[i].Text.Length)
                        {
                            TTipsTB[i].Focus();
                            TTipsTB[i].SelectAll();
                        }
                    }
                    else if (e.KeyCode.ToString() == "Up")
                    {
                        if (i == _firstLineIndex)
                        {
                            CTipsTB[_lastLineIndex].Focus();
                            CTipsTB[_lastLineIndex].SelectAll();
                        }
                        else
                        {
                            for (int j = i; j > _firstLineIndex; j--)
                            {
                                if (CTipsTB[j - 1].Visible)
                                {
                                    CTipsTB[j - 1].Focus();
                                    CTipsTB[j - 1].SelectAll();
                                    break;
                                }
                            }
                        }
                    }
                    else if (e.KeyCode.ToString() == "Down")
                    {
                        if (i == _lastLineIndex)
                        {
                            BPTB.Focus();
                            BPTB.SelectAll();
                        }
                        else
                        {
                            for (int j = i; j < _lastLineIndex; j++)
                            {
                                if (CTipsTB[j + 1].Visible)
                                {
                                    CTipsTB[j + 1].Focus();
                                    CTipsTB[j + 1].SelectAll();
                                    break;
                                }
                            }
                        }
                    }
                    break;
                }
            }
        }
        private void TTipsTB_KeyDown(object sender, KeyEventArgs e)
        {
            for (int i = _firstLineIndex; i <= _lastLineIndex; i++)
            {
                if (sender == TTipsTB[i])
                {
                    if (e.KeyCode.ToString() == "Left")
                    {
                        if (TTipsTB[i].SelectionStart == 0)
                        {
                            CTipsTB[i].Focus();
                            CTipsTB[i].SelectAll();
                        }
                
                    }
                    else if (e.KeyCode.ToString() == "Right")
                    {
                        if (TTipsTB[i].SelectionStart == TTipsTB[i].Text.Length || TTipsTB[i].SelectionLength == TTipsTB[i].Text.Length)
                        {
                            DlvsTB[i].Focus();
                            DlvsTB[i].SelectAll();
                        }
                
                    }
                    else if (e.KeyCode.ToString() == "Up")
                    {
                        if (i == _firstLineIndex)
                        {
                            TTipsTB[_lastLineIndex].Focus();
                            TTipsTB[_lastLineIndex].SelectAll();
                        }
                        else
                        {
                            for (int j = i; j > _firstLineIndex; j--)
                            {
                                if (TTipsTB[j - 1].Visible)
                                {
                                    TTipsTB[j - 1].Focus();
                                    TTipsTB[j - 1].SelectAll();
                                    break;
                                }
                            }
                        }
                    }
                    else if (e.KeyCode.ToString() == "Down")
                    {
                        if (i == _lastLineIndex)
                        {
                            BPTB.Focus();
                            BPTB.SelectAll();
                        }
                        else
                        {
                            for (int j = i; j < _lastLineIndex; j++)
                            {
                                if (TTipsTB[j + 1].Visible)
                                {
                                    TTipsTB[j + 1].Focus();
                                    TTipsTB[j + 1].SelectAll();
                                    break;
                                }
                            }
                        }
                    }
                    break;
                }
            }
        }
        private void DlvsTB_KeyDown(object sender, KeyEventArgs e)
        {
            for (int i = _firstLineIndex; i <= _lastLineIndex; i++)
            {
                if (sender == DlvsTB[i])
                {
                    if (e.KeyCode.ToString() == "Left")
                    {
                        if (DlvsTB[i].SelectionStart == 0)
                        {
                            TTipsTB[i].Focus();
                            TTipsTB[i].SelectAll();
                        }
                    }
                    else if (e.KeyCode.ToString() == "Right")
                    {
                        if (DlvsTB[i].SelectionStart == DlvsTB[i].Text.Length || DlvsTB[i].SelectionLength == DlvsTB[i].Text.Length)
                        {
                            DateTB[i].Focus();
                            DateTB[i].SelectAll();
                        }
                    }
                    else if (e.KeyCode.ToString() == "Up")
                    {
                        if (i == _firstLineIndex)
                        {
                            DlvsTB[_lastLineIndex].Focus();
                            DlvsTB[_lastLineIndex].SelectAll();
                        }
                        else
                        {
                            for (int j = i; j > _firstLineIndex; j--)
                            {
                                if (DlvsTB[j - 1].Visible)
                                {
                                    DlvsTB[j - 1].Focus();
                                    DlvsTB[j - 1].SelectAll();
                                    break;
                                }
                            }
                        }
                    }
                    else if (e.KeyCode.ToString() == "Down")
                    {
                        if (i == _lastLineIndex)
                        {
                            BPTB.Focus();
                            BPTB.SelectAll();
                        }
                        else
                        {
                            for (int j = i; j < _lastLineIndex; j++)
                            {
                                if (DlvsTB[j + 1].Visible)
                                {
                                    DlvsTB[j + 1].Focus();
                                    DlvsTB[j + 1].SelectAll();
                                    break;
                                }
                            }
                        }
                    }
                    break;
                }
            }
        }
        private void BPTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Left")
            {
                if (BPTB.SelectionStart == 0)
                {
                    ETPTB.Focus();
                    ETPTB.SelectAll();
                }
            }
            else if (e.KeyCode.ToString() == "Right")
            {
                if (BPTB.SelectionStart == BPTB.Text.Length || BPTB.SelectionLength == BPTB.Text.Length)
                {
                    CPTB.Focus();
                    CPTB.SelectAll();
                }
            }
            else if (e.KeyCode.ToString() == "Up")
            {
                try
                {
                    DateTB[_lastLineIndex].Focus();
                    DateTB[_lastLineIndex].SelectAll();
                }
                catch
                {
                }
            }
            else if (e.KeyCode.ToString() == "Down")
            {
                try
                {
                    DateTB[_firstLineIndex].Focus();
                    DateTB[_firstLineIndex].SelectAll();
                }
                catch
                {
                }
            }
        }
        private void CPTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Left")
            {
                if (CPTB.SelectionStart == 0)
                {
                    BPTB.Focus();
                    BPTB.SelectAll();
                }
            }
            else if (e.KeyCode.ToString() == "Right")
            {
                if (CPTB.SelectionStart == CPTB.Text.Length || CPTB.SelectionLength == CPTB.Text.Length)
                {
                    PPDlvTB.Focus();
                    PPDlvTB.SelectAll();
                }
            }
            else if (e.KeyCode.ToString() == "Up")
            {
                try
                {
                    DateTB[_lastLineIndex].Focus();
                    DateTB[_lastLineIndex].SelectAll();
                }
                catch
                {
                }
            }
            else if (e.KeyCode.ToString() == "Down")
            {
                try
                {
                    DateTB[_firstLineIndex].Focus();
                    DateTB[_firstLineIndex].SelectAll();
                }
                catch
                {
                }
            }
        }
        private void MCTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Left")
            {
                if (MCTB.SelectionStart == 0)
                {
                    PPDlvTB.Focus();
                    PPDlvTB.SelectAll();
                }
            }
            else if (e.KeyCode.ToString() == "Right")
            {
                if (MCTB.SelectionStart == MCTB.Text.Length || MCTB.SelectionLength == MCTB.Text.Length)
                {
                    ETPTB.Focus();
                    ETPTB.SelectAll();
                }
            }
            else if (e.KeyCode.ToString() == "Up")
            {
                try
                {
                    DateTB[_lastLineIndex].Focus();
                    DateTB[_lastLineIndex].SelectAll();
                }
                catch
                {
                }
            }
            else if (e.KeyCode.ToString() == "Down")
            {
                try
                {
                    DateTB[_firstLineIndex].Focus();
                    DateTB[_firstLineIndex].SelectAll();
                }
                catch
                {
                }
            }
        }
        private void ETPTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Left")
            {
                if (ETPTB.SelectionStart == 0)
                {
                    MCTB.Focus();
                    MCTB.SelectAll();
                }
            }
            else if (e.KeyCode.ToString() == "Right")
            {
                if (ETPTB.SelectionStart == ETPTB.Text.Length || ETPTB.SelectionLength == ETPTB.Text.Length)
                {
                    BPTB.Focus();
                    BPTB.SelectAll();
                }
            }
            else if (e.KeyCode.ToString() == "Up")
            {
                try
                {
                    DateTB[_lastLineIndex].Focus();
                    DateTB[_lastLineIndex].SelectAll();
                }
                catch
                {
                }
            }
            else if (e.KeyCode.ToString() == "Down")
            {
                try
                {
                    DateTB[_firstLineIndex].Focus();
                    DateTB[_firstLineIndex].SelectAll();
                }
                catch
                {
                }
            }
        }
        private void PPDlvTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Left")
            {
                if (PPDlvTB.SelectionStart == 0)
                {
                    CPTB.Focus();
                    CPTB.SelectAll();
                }
            }
            else if (e.KeyCode.ToString() == "Right")
            {
                if (PPDlvTB.SelectionStart == PPDlvTB.Text.Length || PPDlvTB.SelectionLength == PPDlvTB.Text.Length)
                {
                    MCTB.Focus();
                    MCTB.SelectAll();
                }
            }
            else if (e.KeyCode.ToString() == "Up")
            {
                try
                {
                    DateTB[_lastLineIndex].Focus();
                    DateTB[_lastLineIndex].SelectAll();
                }
                catch
                {
                }
            }
            else if (e.KeyCode.ToString() == "Down")
            {
                try
                {
                    DateTB[_firstLineIndex].Focus();
                    DateTB[_firstLineIndex].SelectAll();
                }
                catch
                {
                }
            }
        }
        private void Save(string filePath, bool backUp)
        {
            if (backUp)
                File.Copy(filePath, filePath + ".bak", true);
            StreamWriter fileW = new StreamWriter(filePath, false);
            fileW.WriteLine(Encrypt(_pass + "~Line#|Date|StartMiles|EndMiles|TotalMiles|TimeIn|TimeOut|TotalTime|TotalSales|CreditTips|TotalTips|CashTips|Deliveries|BasePay" + BPTB.Text + "|Commission%" + CPTB.Text + "|Pay/Delivery" + PPDlvTB.Text + "|MileCredit" + MCTB.Text + "|EmpTax%" + ETPTB.Text));
            for (int i = 0; i < _lines; i++)
            {
                fileW.WriteLine(Encrypt(NumL[i].Text + "|" + DateTB[i].Text + "|" + SMilesTB[i].Text + "|" + EMilesTB[i].Text + "|" + TMilesTB[i].Text + "|" + TimeITB[i].Text + "|" + TimeOTB[i].Text + "|" + TTimeTB[i].Text + "|" + TSalesTB[i].Text + "|" + CTipsTB[i].Text + "|" + TTipsTB[i].Text + "|" + CaTipsTB[i].Text + "|" + DlvsTB[i].Text));
            }
            fileW.Close();
            _saved = true;
            if (this.Text == "Driver's Log : " + _driverName + "*")
                this.Text = "Driver's Log : " + _driverName;
        }
        private void Save(string filePath, bool backUp,int removeLine)
        {
            if (backUp)
                File.Copy(filePath, filePath + ".bak", true);
            StreamWriter fileW = new StreamWriter(filePath, false);
            fileW.WriteLine(Encrypt(_pass + "~Line#|Date|StartMiles|EndMiles|TotalMiles|TimeIn|TimeOut|TotalTime|TotalSales|CreditTips|TotalTips|CashTips|Deliveries|BasePay" + BPTB.Text + "|Commission%" + CPTB.Text + "|Pay/Delivery" + PPDlvTB.Text + "|MileCredit" + MCTB.Text + "|EmpTax%" + ETPTB.Text));
            for (int i = 0; i < _lines; i++)
            {
                if (i != removeLine - 1)
                {
                    fileW.WriteLine(Encrypt(NumL[i].Text + "|" + DateTB[i].Text + "|" + SMilesTB[i].Text + "|" + EMilesTB[i].Text + "|" + TMilesTB[i].Text + "|" + TimeITB[i].Text + "|" + TimeOTB[i].Text + "|" + TTimeTB[i].Text + "|" + TSalesTB[i].Text + "|" + CTipsTB[i].Text + "|" + TTipsTB[i].Text + "|" + CaTipsTB[i].Text + "|" + DlvsTB[i].Text));
                }
            }
            fileW.Close();
        }
        private void Open(string filePath, bool newLine)
        {
            if (File.Exists(filePath))
            {
                StreamReader fileR = new StreamReader(filePath);
                try
                {
                    _calc = false;
                    foreach (string s in Decrypt(fileR.ReadLine()).Split('|'))
                    {
                        if (s.Contains("BasePay") && s.Length > 7)
                            BPTB.Text = Convert.ToDouble(s.Substring(7)).ToString("0.00");
                        else if (s.Contains("Commission%") && s.Length > 11)
                            CPTB.Text = s.Substring(11);
                        else if (s.Contains("Pay/Delivery") && s.Length > 12)
                            PPDlvTB.Text = s.Substring(12);
                        else if (s.Contains("MileCredit") && s.Length > 10)
                            MCTB.Text = s.Substring(10);
                        else if (s.Contains("EmpTax%") && s.Length > 7)
                            ETPTB.Text = s.Substring(7);
                    }
                    int j = 0;
                    if (newLine)
                        j++;
                    while (!fileR.EndOfStream)
                    {
                        string[] s = new string[13];
                        s = Decrypt(fileR.ReadLine()).Split('|');
                        DateTB[j].Text = s[1];
                        SMilesTB[j].Text = s[2];
                        EMilesTB[j].Text = s[3];
                        TMilesTB[j].Text = s[4];
                        TimeITB[j].Text = s[5];
                        TimeOTB[j].Text = s[6];
                        TTimeTB[j].Text = s[7];
                        TSalesTB[j].Text = s[8];
                        CTipsTB[j].Text = s[9];
                        TTipsTB[j].Text = s[10];
                        CaTipsTB[j].Text = s[11];
                        DlvsTB[j++].Text = s[12];

                    }
                    fileR.Close();
                    _minDate = DateTime.MaxValue;
                    _maxDate = DateTime.MinValue;
                    if (_lines > 0)
                    {
                        for (int i = 0; i < _lines; i++)
                        {
                            if (Convert.ToDateTime(DateTB[i].Text) < _minDate)
                                _minDate = Convert.ToDateTime(DateTB[i].Text);
                            if (Convert.ToDateTime(DateTB[i].Text) > _maxDate)
                                _maxDate = Convert.ToDateTime(DateTB[i].Text);
                        }
                    }
                    _calc = true;
                    saveToolStripMenuItem.Visible = true;
                    printToolStripMenuItem.Visible = true;
                    changePasswordToolStripMenuItem.Visible = true;
                    newLineToolStripMenuItem.Visible = true;
                    if (_lines > 0)
                    {
                        calculationsToolStripMenuItem.Visible = true;
                        showDatesToolStripMenuItem.Visible = true;
                        removeLineToolStripMenuItem.Visible = true;
                    }
                    else
                    {
                        calculationsToolStripMenuItem.Visible = false;
                        showDatesToolStripMenuItem.Visible = false;
                        removeLineToolStripMenuItem.Visible = false;
                    }
                    if (Properties.Settings.Default.NumLinesDays != 0)
                    {
                        if (Properties.Settings.Default.ShowLines)
                        {
                            Show(1, (_lines < Properties.Settings.Default.NumLinesDays?_lines:Properties.Settings.Default.NumLinesDays));
                        }
                        else
                        {
                            Show(DateTime.Today.Subtract(TimeSpan.FromDays(Properties.Settings.Default.NumLinesDays - 1)), DateTime.Today);
                        }
                    }
                    else
                    {
                        Show(1, _lines);
                    }
                }
                catch (Exception ex)
                {
                    fileR.Close();
                    if (File.Exists(filePath + ".bak") && MessageBox.Show("The save file for this user seems to be corrupt!\n\rDo you want to convert to the last known uncorrupt file?", "Corrupt File!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        File.Copy(filePath, filePath + ".corrupt", true);
                        File.Copy(filePath + ".bak", filePath, true);
                        File.Delete(filePath + ".bak");
                        Open(filePath, false);
                    }
                    else
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
            else
            {
                MessageBox.Show("File does not exist!");
            }
        }
        private void Show(DateTime startDate, DateTime endDate)
        {
            _firstLineIndex = -1;
            _lastLineIndex = -1;
            DateTime firstDate = DateTime.MaxValue, lastDate = DateTime.MinValue;
            bool visible = true;
            for (int i = 0; i < _lines; i++)
            {
                try
                {
                    if (Convert.ToDateTime(DateTB[i].Text) <= endDate && Convert.ToDateTime(DateTB[i].Text) >= startDate)
                    {
                        visible = true;
                        if (Convert.ToDateTime(DateTB[i].Text) < firstDate)
                        {
                            firstDate = Convert.ToDateTime(DateTB[i].Text);
                            _firstLineIndex = i;
                        }
                        if (Convert.ToDateTime(DateTB[i].Text) >= lastDate)
                        {
                            lastDate = Convert.ToDateTime(DateTB[i].Text);
                            _lastLineIndex = i;
                        }
                    }
                    else
                    {
                        visible = false;
                    }
                    DateTB[i].Visible = visible;
                    SMilesTB[i].Visible = visible;
                    EMilesTB[i].Visible = visible;
                    TMilesTB[i].Visible = visible;
                    TimeITB[i].Visible = visible;
                    TimeOTB[i].Visible = visible;
                    TTimeTB[i].Visible = visible;
                    TSalesTB[i].Visible = visible;
                    CTipsTB[i].Visible = visible;
                    TTipsTB[i].Visible = visible;
                    CaTipsTB[i].Visible = visible;
                    DlvsTB[i].Visible = visible;
                }
                catch { }
            }
        }
        private void Show(int startLine, int endLine)
        {
            for (int i = 0; i < _lines; i++)
            {
                bool visible = false;
                if (i >= startLine - 1 && i <= endLine - 1)
                {
                    visible = true;
                }
                DateTB[i].Visible = visible;
                SMilesTB[i].Visible = visible;
                EMilesTB[i].Visible = visible;
                TMilesTB[i].Visible = visible;
                TimeITB[i].Visible = visible;
                TimeOTB[i].Visible = visible;
                TTimeTB[i].Visible = visible;
                TSalesTB[i].Visible = visible;
                CTipsTB[i].Visible = visible;
                TTipsTB[i].Visible = visible;
                CaTipsTB[i].Visible = visible;
                DlvsTB[i].Visible = visible;
            }
        }
        private string WriteFile()
        {
            string s = "";
            if (Properties.Settings.Default.PBasePay)
            {
                s += "Base Pay:             " + BPTB.Text + "\n";
            }
            if (Properties.Settings.Default.PCommP)
            {
                s += "Comm. %:              " + CPTB.Text + "\n";
            }
            if (Properties.Settings.Default.PBasePaySalary)
            {
                s += "Base Pay Salary:      " + "" + "\n";////
            }
            if (Properties.Settings.Default.PCommPaySalary)
            {
                s += "Comm. Pay Salary:     " + "" + "\n";////
            }
            if (Properties.Settings.Default.PPPDlv)
            {
                s += "Pay / Delivery:       " + "" + "\n";////
            }
            if (Properties.Settings.Default.PTotalMiles)
            {
                s += "Total Miles:          " + "" + "\n";////
            }
            if (Properties.Settings.Default.PMileCredit)
            {
                s += "Mile Credit:          " + MCTB.Text + "\n";
            }
            if (Properties.Settings.Default.PEmpTaxP)
            {
                s += "Emp. Tax %:           " + ETPTB.Text + "\n";
            }
            if (Properties.Settings.Default.PTotalEmpTax)
            {
                s += "Total Emp. Tax:       " + "" + "\n";////
            }
            if (Properties.Settings.Default.PTotalHours)
            {
                s += "Total Hours:          " + "" + "\n";////
            }
            if (Properties.Settings.Default.PTipPM)
            {
                s += "Tip / Mile:           " + "" + "\n";////
            }
            if (Properties.Settings.Default.PMilesPH)
            {
                s += "Miles/Hour:           " + "" + "\n";////
            }
            if (Properties.Settings.Default.PBasePayPH)
            {
                s += "Base Pay/Hour:        " + "" + "\n";////
            }
            if (Properties.Settings.Default.PCommPayPH)
            {
                s += "Comm. Pay/Hour:       " + "" + "\n";////
            }
            if (Properties.Settings.Default.PSalesPH)
            {
                s += "Sales/Hour:           " + "" + "\n";////
            }
            if (Properties.Settings.Default.PTipsPH)
            {
                s += "Tips/Hour:            " + "" + "\n";////
            }
            if (Properties.Settings.Default.PDlvsPH)
            {
                s += "Dlvs/Hour:            " + "" + "\n";////
            }
            if (Properties.Settings.Default.PTotalSales)
            {
                s += "Total Sales:          " + "" + "\n";////
            }
            if (Properties.Settings.Default.PTotalTips)
            {
                s += "Total Tips:           " + "" + "\n";////
            }
            if (Properties.Settings.Default.PTotalDlvs)
            {
                s += "Total Dlvs:           " + "" + "\n";////
            }
            if (Properties.Settings.Default.PSalesPD)
            {
                s += "Sales/Dlv:            " + "" + "\n";////
            }
            if (Properties.Settings.Default.PTipsPD)
            {
                s += "Tip/Dlv:              " + "" + "\n";////
            }
            if (Properties.Settings.Default.PTipP)
            {
                s += "Tip %:                " + "" + "\n";////
            }
            if (Properties.Settings.Default.PCSWT)
            {
                s += "Comm. Salary w/ Tips: " + "" + "\n";////
            }
            if (Properties.Settings.Default.PBSWT)
            {
                s += "Base Salary w/ Tips:  " + "" + "\n";////
            }
            if (Properties.Settings.Default.PTCTips)
            {
                s += "Total Credit Tips:    " + "" + "\n";////
            }
            if (Properties.Settings.Default.PTCaTips)
            {
                s += "Total Cash Tips:      " + "" + "\n";////
            }
            int i;
            if (Properties.Settings.Default.PLineNum)
            {
                s += String.Format("{0,-7}", "Line #");
            }
            if (Properties.Settings.Default.PDate)
            {
                s += String.Format("{0,-9}", "Date");
            }
            if (Properties.Settings.Default.PStartMiles)
            {
                s += String.Format("{0,-12}", "Start Miles");
            }
            if (Properties.Settings.Default.PEndMiles)
            {
                s += String.Format("{0,-10}", "End Miles");
            }
            if (Properties.Settings.Default.PTotalMilesCol)
            {
                s += String.Format("{0,-12}", "Total Miles");
            }
            if (Properties.Settings.Default.PTimeIn)
            {
                s += String.Format("{0,-8}", "Time In");
            }
            if (Properties.Settings.Default.PTimeOut)
            {
                s += String.Format("{0,-9}", "Time Out");
            }
            if (Properties.Settings.Default.PTotalTime)
            {
                s += String.Format("{0,-11}", "Total Time");
            }
            if (Properties.Settings.Default.PTotalSalesCol)
            {
                s += String.Format("{0,-12}", "Total Sales");
            }
            if (Properties.Settings.Default.PCreditTips)
            {
                s += String.Format("{0,-12}", "Credit Tips");
            }
            if (Properties.Settings.Default.PTotalTipsCol)
            {
                s += String.Format("{0,-11}", "Total Tips");
            }
            if (Properties.Settings.Default.PCashTips)
            {
                s += String.Format("{0,-10}", "Cash Tips");
            }
            if (Properties.Settings.Default.PDeliveries)
            {
                s += String.Format("{0,-11}", "Deliveries");
            }
            if (Properties.Settings.Default.AllDates)
            {
                for (i = 0; i < _lines; i++)
                {
                    s += "\n";
                    if (Properties.Settings.Default.PLineNum)
                    {
                        s += String.Format("{0,-7}", NumL[i].Text);
                    }
                    if (Properties.Settings.Default.PDate)
                    {
                        s += String.Format("{0,-9}", DateTB[i].Text);
                    }
                    if (Properties.Settings.Default.PStartMiles)
                    {
                        s += String.Format("{0,-12}", SMilesTB[i].Text);
                    }
                    if (Properties.Settings.Default.PEndMiles)
                    {
                        s += String.Format("{0,-10}", EMilesTB[i].Text);
                    }
                    if (Properties.Settings.Default.PTotalMilesCol)
                    {
                        s += String.Format("{0,-12}", TMilesTB[i].Text);
                    }
                    if (Properties.Settings.Default.PTimeIn)
                    {
                        s += String.Format("{0,-8}", TimeITB[i].Text);
                    }
                    if (Properties.Settings.Default.PTimeOut)
                    {
                        s += String.Format("{0,-9}", TimeOTB[i].Text);
                    }
                    if (Properties.Settings.Default.PTotalTime)
                    {
                        s += String.Format("{0,-11}", TTimeTB[i].Text);
                    }
                    if (Properties.Settings.Default.PTotalSalesCol)
                    {
                        s += String.Format("{0,-12}", TSalesTB[i].Text);
                    }
                    if (Properties.Settings.Default.PCreditTips)
                    {
                        s += String.Format("{0,-12}", CTipsTB[i].Text);
                    }
                    if (Properties.Settings.Default.PTotalTipsCol)
                    {
                        s += String.Format("{0,-11}", TTipsTB[i].Text);
                    }
                    if (Properties.Settings.Default.PCashTips)
                    {
                        s += String.Format("{0,-10}", CaTipsTB[i].Text);
                    }
                    if (Properties.Settings.Default.PDeliveries)
                    {
                        s += String.Format("{0,-11}", DlvsTB[i].Text);
                    }
                }
            }
            else
            {
                for (i = _firstLineIndex; i <= _lastLineIndex; i++)
                {
                    if (DateTB[i].Visible)
                    {
                        s += "\n";
                        if (Properties.Settings.Default.PLineNum)
                        {
                            s += String.Format("{0,-7}", NumL[i].Text);
                        }
                        if (Properties.Settings.Default.PDate)
                        {
                            s += String.Format("{0,-9}", DateTB[i].Text);
                        }
                        if (Properties.Settings.Default.PStartMiles)
                        {
                            s += String.Format("{0,-12}", SMilesTB[i].Text);
                        }
                        if (Properties.Settings.Default.PEndMiles)
                        {
                            s += String.Format("{0,-10}", EMilesTB[i].Text);
                        }
                        if (Properties.Settings.Default.PTotalMilesCol)
                        {
                            s += String.Format("{0,-12}", TMilesTB[i].Text);
                        }
                        if (Properties.Settings.Default.PTimeIn)
                        {
                            s += String.Format("{0,-8}", TimeITB[i].Text);
                        }
                        if (Properties.Settings.Default.PTimeOut)
                        {
                            s += String.Format("{0,-9}", TimeOTB[i].Text);
                        }
                        if (Properties.Settings.Default.PTotalTime)
                        {
                            s += String.Format("{0,-11}", TTimeTB[i].Text);
                        }
                        if (Properties.Settings.Default.PTotalSalesCol)
                        {
                            s += String.Format("{0,-12}", TSalesTB[i].Text);
                        }
                        if (Properties.Settings.Default.PCreditTips)
                        {
                            s += String.Format("{0,-12}", CTipsTB[i].Text);
                        }
                        if (Properties.Settings.Default.PTotalTipsCol)
                        {
                            s += String.Format("{0,-11}", TTipsTB[i].Text);
                        }
                        if (Properties.Settings.Default.PCashTips)
                        {
                            s += String.Format("{0,-10}", CaTipsTB[i].Text);
                        }
                        if (Properties.Settings.Default.PDeliveries)
                        {
                            s += String.Format("{0,-11}", DlvsTB[i].Text);
                        }
                    }
                }
            }
            return s;
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
        private void calculationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Calculations c = new Calculations(TMilesTB, TTimeTB, TSalesTB, CTipsTB, TTipsTB, CaTipsTB, DlvsTB, BPTB.Text, CPTB.Text, PPDlvTB.Text, MCTB.Text, ETPTB.Text);
            c.ShowDialog();
        }
    }
}
