using System;
using System.Windows.Forms;

namespace SimpleHttpServer {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }
        /// <summary>
        /// 当前对象
        /// </summary>
        public static Form1 _this = null;
        /// <summary>
        /// 根目录
        /// </summary>
        public static string root;
        /// <summary>
        /// 初始化
        /// </summary>
        private void Form1_Load(object sender, EventArgs e) {
            _this = this;
            cbIpAddress.SelectedIndex = 0;
        }
        /// <summary>
        /// 端口输入检测，只允许数字
        /// </summary>
        private void tbPort_KeyPress(object sender, KeyPressEventArgs e) {
            if(!(char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8) {
                e.Handled = true;
            }
        }
        /// <summary>
        /// 端口输入检测，格式检查
        /// </summary>
        private void tbPort_TextChanged(object sender, EventArgs e) {
            string text = tbPort.Text;
            if(tbPort.Text.Length == 0) {
                tbPort.Text = "0";
            } else if(int.Parse(tbPort.Text) > 65535) {
                tbPort.Text = "65535";
            } else {
                tbPort.Text = Math.Abs(int.Parse(tbPort.Text)).ToString();
            }
            if(text != tbPort.Text) {
                tbPort.SelectionStart = tbPort.Text.Length;
            }
        }
        /// <summary>
        /// 浏览文件夹
        /// </summary>
        private void btnBrowser_Click(object sender, EventArgs e) {
            if(folderBrowser.ShowDialog() == DialogResult.OK) {
                root = tbFolder.Text = folderBrowser.SelectedPath;
                if(root.EndsWith("\\")) {
                    root = root.Substring(0, root.Length - 1);
                }
            }
        }
        /// <summary>
        /// 启动服务
        /// </summary>
        private void btnStart_Click(object sender, EventArgs e) {
        }
    }
}
