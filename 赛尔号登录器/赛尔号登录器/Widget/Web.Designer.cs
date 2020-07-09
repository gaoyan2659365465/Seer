namespace 赛尔号登录器
{
    partial class Web
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceController1 = new System.ServiceProcess.ServiceController();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // serviceController1
            // 
            this.serviceController1.ServiceName = "Flash Helper Service";
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.Margin = new System.Windows.Forms.Padding(0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(830, 401);
            this.webBrowser1.TabIndex = 0;
            // 
            // Web
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkRed;
            this.Controls.Add(this.webBrowser1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Web";
            this.Size = new System.Drawing.Size(830, 401);
            this.ResumeLayout(false);

        }

        #endregion
        private System.ServiceProcess.ServiceController serviceController1;
        private System.Windows.Forms.WebBrowser webBrowser1;
    }
}
