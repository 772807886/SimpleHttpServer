using System;
using System.Net;
using System.Net.NetworkInformation;
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
        /// <summary>
        /// 开始监听
        /// </summary>
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
        /// <summary>
        /// 终止监听线程
        /// </summary>
        public void stop() {
            if(running) {
                running = false;
                listener.Stop();
            }
        }
        /// <summary>
        /// 启动线程
        /// </summary>
        public override void run() {
            listen();
        }
        /// <summary>
        /// 端口是否被占用
        /// </summary>
        /// <param name="port">端口</param>
        /// <returns>检测结果</returns>
        public static bool PortInUse(int port) {
            bool inUse = false;
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();
            foreach(IPEndPoint endPoint in ipEndPoints) {
                if(endPoint.Port == port) {
                    inUse = true;
                    break;
                }
            }
            return inUse;
        }
    }
}
