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

        public Main()
        {
            InitializeComponent();
        }

        public void status_changed(int status, string message)
        {
            string text = string.Format("status:{0},{1}", status, message);
            if (this.txtStatus.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetStatus);
                this.Invoke(d, new object[] { text });
            }

            if (status == 0)
                pbarExc.Value = pbarExc.Value + 1;
        }
        private void SetStatus(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtStatus.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetStatus);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtStatus.Text = text;
            }
        }
        private void btnCzce_Click(object sender, EventArgs e)
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
            txtStatus.Text = "Run...";
            Thread t = new Thread(new ThreadStart(wreq.run_czce));
            t.Start();

            return;
        }

        private void btnDce_Click(object sender, EventArgs e)
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
            }
            pbarExc.Minimum = 0;
            pbarExc.Maximum = run_days;

            txtStatus.Text = "Run...";
            Thread t = new Thread(new ThreadStart(wreq.run_dce));
            t.Start();
        }

        private void btnShfe_Click(object sender, EventArgs e)
        {

        }

        private void btnCffex_Click(object sender, EventArgs e)
        {

        }
    }
}
