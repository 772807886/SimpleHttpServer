namespace SimpleHttpServer {
    /// <summary>
    /// 日志记录
    /// </summary>
    class Log {
        public static void log(string text, Type type) {
            Form1._this.Invoke((setTextInvoke)delegate(string s, Type t) {
                Form1._this.tbConsole.Text += s + '\n';
            }, text);
        }
        private delegate void setTextInvoke(string text, Type type);
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
