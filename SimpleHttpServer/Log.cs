using System.Drawing;

namespace SimpleHttpServer {
    /// <summary>
    /// 日志记录
    /// </summary>
    class Log {
        public static void log(string text, Type type) {
            Form1._this.Invoke((setTextInvoke)delegate(string s) {
                switch(type) {
                case Type.Normal:
                    Form1._this.rtbConsole.SelectionColor = Color.Black;
                    break;
                case Type.Warning:
                    Form1._this.rtbConsole.SelectionColor = Color.Orange;
                    break;
                case Type.Error:
                    Form1._this.rtbConsole.SelectionColor = Color.Red;
                    break;
                default:
                    break;
                }
                Form1._this.rtbConsole.AppendText(s + "\r\n");
            }, text);
        }
        private delegate void setTextInvoke(string text);
        /// <summary>
        /// 错误类型
        /// </summary>
        public enum Type {
            Normal,
            Warning,
            Error
        }
    }
}
