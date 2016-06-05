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
        TcpListener listener;
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
            while(true) {
                TcpClient client = listener.AcceptTcpClient();
            }
        }
        public override void run() {
            listen();
        }
    }
}
