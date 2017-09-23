namespace Scrapy
{
    partial class Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCzce = new System.Windows.Forms.Button();
            this.btnShfe = new System.Windows.Forms.Button();
            this.btnDce = new System.Windows.Forms.Button();
            this.btnCffex = new System.Windows.Forms.Button();
            this.tab = new System.Windows.Forms.TabControl();
            this.tpExcWebMds = new System.Windows.Forms.TabPage();
            this.pbarExc = new System.Windows.Forms.ProgressBar();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tab.SuspendLayout();
            this.tpExcWebMds.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCzce
            // 
            this.btnCzce.Location = new System.Drawing.Point(54, 146);
            this.btnCzce.Name = "btnCzce";
            this.btnCzce.Size = new System.Drawing.Size(75, 23);
            this.btnCzce.TabIndex = 0;
            this.btnCzce.Text = "Czce";
            this.btnCzce.UseVisualStyleBackColor = true;
            this.btnCzce.Click += new System.EventHandler(this.btnCzce_Click);
            // 
            // btnShfe
            // 
            this.btnShfe.Location = new System.Drawing.Point(150, 146);
            this.btnShfe.Name = "btnShfe";
            this.btnShfe.Size = new System.Drawing.Size(75, 23);
            this.btnShfe.TabIndex = 1;
            this.btnShfe.Text = "Shfe";
            this.btnShfe.UseVisualStyleBackColor = true;
            this.btnShfe.Click += new System.EventHandler(this.btnShfe_Click);
            // 
            // btnDce
            // 
            this.btnDce.Location = new System.Drawing.Point(248, 145);
            this.btnDce.Name = "btnDce";
            this.btnDce.Size = new System.Drawing.Size(75, 23);
            this.btnDce.TabIndex = 2;
            this.btnDce.Text = "Dce";
            this.btnDce.UseVisualStyleBackColor = true;
            this.btnDce.Click += new System.EventHandler(this.btnDce_Click);
            // 
            // btnCffex
            // 
            this.btnCffex.Location = new System.Drawing.Point(349, 144);
            this.btnCffex.Name = "btnCffex";
            this.btnCffex.Size = new System.Drawing.Size(75, 23);
            this.btnCffex.TabIndex = 3;
            this.btnCffex.Text = "Cffex";
            this.btnCffex.UseVisualStyleBackColor = true;
            this.btnCffex.Click += new System.EventHandler(this.btnCffex_Click);
            // 
            // tab
            // 
            this.tab.Controls.Add(this.tpExcWebMds);
            this.tab.Controls.Add(this.tabPage2);
            this.tab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab.Location = new System.Drawing.Point(0, 0);
            this.tab.Name = "tab";
            this.tab.SelectedIndex = 0;
            this.tab.Size = new System.Drawing.Size(754, 448);
            this.tab.TabIndex = 4;
            // 
            // tpExcWebMds
            // 
            this.tpExcWebMds.Controls.Add(this.pbarExc);
            this.tpExcWebMds.Controls.Add(this.dtpEnd);
            this.tpExcWebMds.Controls.Add(this.dtpFrom);
            this.tpExcWebMds.Controls.Add(this.txtStatus);
            this.tpExcWebMds.Controls.Add(this.btnShfe);
            this.tpExcWebMds.Controls.Add(this.btnCffex);
            this.tpExcWebMds.Controls.Add(this.btnCzce);
            this.tpExcWebMds.Controls.Add(this.btnDce);
            this.tpExcWebMds.Location = new System.Drawing.Point(4, 22);
            this.tpExcWebMds.Name = "tpExcWebMds";
            this.tpExcWebMds.Padding = new System.Windows.Forms.Padding(3);
            this.tpExcWebMds.Size = new System.Drawing.Size(746, 422);
            this.tpExcWebMds.TabIndex = 0;
            this.tpExcWebMds.Text = "交易所网页日线数据";
            this.tpExcWebMds.UseVisualStyleBackColor = true;
            // 
            // pbarExc
            // 
            this.pbarExc.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pbarExc.Location = new System.Drawing.Point(3, 375);
            this.pbarExc.Name = "pbarExc";
            this.pbarExc.Size = new System.Drawing.Size(740, 23);
            this.pbarExc.TabIndex = 19;
            // 
            // dtpEnd
            // 
            this.dtpEnd.Location = new System.Drawing.Point(232, 66);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(118, 21);
            this.dtpEnd.TabIndex = 18;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Location = new System.Drawing.Point(90, 66);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(118, 21);
            this.dtpFrom.TabIndex = 17;
            // 
            // txtStatus
            // 
            this.txtStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtStatus.Location = new System.Drawing.Point(3, 398);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(740, 21);
            this.txtStatus.TabIndex = 16;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(746, 422);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 448);
            this.Controls.Add(this.tab);
            this.Name = "Main";
            this.Text = "Scrapy";
            this.tab.ResumeLayout(false);
            this.tpExcWebMds.ResumeLayout(false);
            this.tpExcWebMds.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCzce;
        private System.Windows.Forms.Button btnShfe;
        private System.Windows.Forms.Button btnDce;
        private System.Windows.Forms.Button btnCffex;
        private System.Windows.Forms.TabControl tab;
        private System.Windows.Forms.TabPage tpExcWebMds;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.ProgressBar pbarExc;
    }
}

