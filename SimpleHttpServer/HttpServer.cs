using System;
using System.Net;
using System.Net.Sockets;

namespace SimpleHttpServer {
    /// <summary>
    /// HTTP服务
    /// </summary>
    class HttpServer : Thread {
        /// <summary>
        /// 监听地址及端口号
        /// </summary>
        private IPEndPoint server;
        /// <summary>
        /// 监听器
        /// </summary>
        private TcpListener listener;
        /// <summary>
        /// 服务器开启状态
        /// </summary>
        public bool running = true;
        /// <summary>
        /// HTTP服务
        /// </summary>
        /// <param name="ip_address">监听地址</param>
        /// <param name="port">监听端口号</param>
        public HttpServer(string ip_address, int port) {
            this.server = new IPEndPoint(IPAddress.Parse(ip_address), port);
        }
        public void listen() {
            listener = new TcpListener(server);
            listener.Start();
            while(running) {
                try {
                    TcpClient client = listener.AcceptTcpClient();
                    new HttpHandler(client).start();
                } catch(Exception) {
                }
                System.Threading.Thread.Sleep(1);
            }
        }
        public void stop() {
            if(running) {
                running = false;
                listener.Stop();
            }
        }
        public override void run() {
            listen();
        }
    }
}
