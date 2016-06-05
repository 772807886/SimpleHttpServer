namespace SimpleHttpServer {
    /// <summary>
    /// 线程基类
    /// </summary>
    abstract class Thread {
        /// <summary>
        /// 线程
        /// </summary>
        protected System.Threading.Thread thread = null;
        /// <summary>
        /// 线程函数
        /// </summary>
        abstract public void run();
        /// <summary>
        /// 启动线程
        /// </summary>
        public void start() {
            if(thread == null) {
                thread = new System.Threading.Thread(run);
            }
            thread.Start();
        }
    }
}
