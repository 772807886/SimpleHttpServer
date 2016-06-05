using System;
using System.Windows.Forms;

namespace SimpleHttpServer {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            cbIpAddress.SelectedIndex = 0;
        }

        private void tbPort_KeyPress(object sender, KeyPressEventArgs e) {
            if(!(char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8) {
                e.Handled = true;
            }
        }

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

        private void btnBrowser_Click(object sender, EventArgs e) {
            if(folderBrowser.ShowDialog() == DialogResult.OK) {
                tbFolder.Text = folderBrowser.SelectedPath;
            }
        }
    }
}
