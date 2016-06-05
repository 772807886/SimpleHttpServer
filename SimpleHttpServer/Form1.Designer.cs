namespace SimpleHttpServer {
    partial class Form1 {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.cbIpAddress = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnBrowser = new System.Windows.Forms.Button();
            this.tbFolder = new System.Windows.Forms.TextBox();
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.btnStart = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tbConsole = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "监听地址：";
            // 
            // cbIpAddress
            // 
            this.cbIpAddress.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIpAddress.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cbIpAddress.Items.AddRange(new object[] {
            "127.0.0.1",
            "0.0.0.0"});
            this.cbIpAddress.Location = new System.Drawing.Point(83, 12);
            this.cbIpAddress.Name = "cbIpAddress";
            this.cbIpAddress.Size = new System.Drawing.Size(121, 20);
            this.cbIpAddress.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(210, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "监听端口：";
            // 
            // tbPort
            // 
            this.tbPort.AutoCompleteCustomSource.AddRange(new string[] {
            "80",
            "8080",
            "8081",
            "8082",
            "8083",
            "8084",
            "8085",
            "8086",
            "8087",
            "8088",
            "8089"});
            this.tbPort.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tbPort.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbPort.Location = new System.Drawing.Point(281, 12);
            this.tbPort.MaxLength = 5;
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(111, 21);
            this.tbPort.TabIndex = 3;
            this.tbPort.Text = "80";
            this.tbPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbPort.WordWrap = false;
            this.tbPort.TextChanged += new System.EventHandler(this.tbPort_TextChanged);
            this.tbPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbPort_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "访问目录：";
            // 
            // btnBrowser
            // 
            this.btnBrowser.Location = new System.Drawing.Point(342, 39);
            this.btnBrowser.Name = "btnBrowser";
            this.btnBrowser.Size = new System.Drawing.Size(50, 23);
            this.btnBrowser.TabIndex = 5;
            this.btnBrowser.Text = "浏览";
            this.btnBrowser.UseVisualStyleBackColor = true;
            this.btnBrowser.Click += new System.EventHandler(this.btnBrowser_Click);
            // 
            // tbFolder
            // 
            this.tbFolder.Location = new System.Drawing.Point(83, 41);
            this.tbFolder.MaxLength = 256;
            this.tbFolder.Name = "tbFolder";
            this.tbFolder.ReadOnly = true;
            this.tbFolder.Size = new System.Drawing.Size(253, 21);
            this.tbFolder.TabIndex = 6;
            this.tbFolder.TabStop = false;
            this.tbFolder.WordWrap = false;
            // 
            // folderBrowser
            // 
            this.folderBrowser.Description = "选择网站根目录";
            this.folderBrowser.ShowNewFolderButton = false;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(317, 68);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 7;
            this.btnStart.Text = "启动服务";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "Console:";
            // 
            // tbConsole
            // 
            this.tbConsole.AcceptsReturn = true;
            this.tbConsole.AcceptsTab = true;
            this.tbConsole.Location = new System.Drawing.Point(12, 97);
            this.tbConsole.Multiline = true;
            this.tbConsole.Name = "tbConsole";
            this.tbConsole.ReadOnly = true;
            this.tbConsole.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbConsole.Size = new System.Drawing.Size(380, 352);
            this.tbConsole.TabIndex = 9;
            this.tbConsole.TabStop = false;
            this.tbConsole.WordWrap = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 461);
            this.Controls.Add(this.tbConsole);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.tbFolder);
            this.Controls.Add(this.btnBrowser);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbIpAddress);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Simple Http Server";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbIpAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnBrowser;
        private System.Windows.Forms.TextBox tbFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowser;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label4;
        internal System.Windows.Forms.TextBox tbConsole;
    }
}

