using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scrapy
{
    public partial class Main : Form
    {
        delegate void SetTextCallback(string text);
        delegate void SetValueCallback(int value);

        public Main()
        {
            InitializeComponent();
        }

        public void status_changed(int status, string message)
        {
            string text = string.Format("status:{0},{1}", status, message);
            if (this.txtStatus.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetStatusText);
                this.Invoke(d, new object[] { text });
            }

            if (status == 0)
            {
                if (this.pbarExc.InvokeRequired)
                {
                    SetValueCallback d = new SetValueCallback(SetStatusProgress);
                    this.Invoke(d, new object[] { pbarExc.Value + 1 });
                }
            }

            using (StreamWriter sr = new StreamWriter("scrapy.log", true, Encoding.UTF8)) 
            {
                sr.WriteLine(DateTime.Now.ToString("yyyymmdd HH:MM:ss") + " " + message);
            }
        }
        private void SetStatusText(string text) 
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtStatus.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetStatusText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtStatus.Text = text;
            }
        }
        private void SetStatusProgress(int value)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtStatus.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetStatusText);
                this.Invoke(d, new object[] { value });
            }
            else
            {
                this.pbarExc.Value = value;
            }
        }
        void do_work(string exchange)
        {
            web_request wreq = new web_request();
            wreq.handler_ = this;
            int run_days = 0;
            while (dtpFrom.Value.Date <= dtpEnd.Value.Date)
            {
                int y = dtpFrom.Value.Date.Year;
                int m = dtpFrom.Value.Date.Month;
                int d = dtpFrom.Value.Day;
                dtpFrom.Value = dtpFrom.Value.Date.AddDays(1);

                DateTime do_date = new DateTime(y, m, d);
                if (do_date.DayOfWeek == DayOfWeek.Saturday || do_date.DayOfWeek == DayOfWeek.Sunday)
                    continue;
                wreq.lst_request_date_.Add(do_date);
                run_days++;
            }
            pbarExc.Minimum = 0;
            pbarExc.Maximum = run_days;
            pbarExc.Value = 0;

            txtStatus.Text = "Run...";
            ThreadStart start = null;
            if (exchange == "dce")
                start = new ThreadStart(wreq.download_dce);
            else if (exchange == "czce")
                start = new ThreadStart(wreq.download_czce);
            else if (exchange == "shfe")
                start = new ThreadStart(wreq.download_shfe);
            else if (exchange == "cffex")
                start = new ThreadStart(wreq.download_cffex);
            Thread t = new Thread(start);
            t.Start();
        }
        private void btnCzce_Click(object sender, EventArgs e)
        {
            do_work("czce");
        }

        private void btnDce_Click(object sender, EventArgs e)
        {
            do_work("dce");
        }

        private void btnShfe_Click(object sender, EventArgs e)
        {
            do_work("shfe");
        }

        private void btnCffex_Click(object sender, EventArgs e)
        {
            do_work("cffex");
        }
    }
}
