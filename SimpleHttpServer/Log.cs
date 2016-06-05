namespace SimpleHttpServer {
    /// <summary>
    /// 日志记录
    /// </summary>
    class Log {
        public static void log(string text, Type type) {
            Form1._this.Invoke((setTextInvoke)delegate(string s) {
                Form1._this.tbConsole.Text += s + "\r\n";
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
