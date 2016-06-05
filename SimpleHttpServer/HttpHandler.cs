using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace SimpleHttpServer {
    /// <summary>
    /// HTTP请求处理线程
    /// </summary>
    class HttpHandler : Thread {
        /// <summary>
        /// TCP接受客户端
        /// </summary>
        private TcpClient client;
        /// <summary>
        /// 输入流
        /// </summary>
        private Stream input;
        /// <summary>
        /// 输出流
        /// </summary>
        private StreamWriter output;
        /// <summary>
        /// 字符串构造器
        /// </summary>
        private StringBuilder sb;
        /// <summary>
        /// HTTP请求处理
        /// </summary>
        /// <param name="client">TCP接受客户端</param>
        public HttpHandler(TcpClient client) {
            this.client = client;
        }
        /// <summary>
        /// 执行
        /// </summary>
        public override void run() {
            //输入输出流
            input = new BufferedStream(client.GetStream());
            output = new StreamWriter(new BufferedStream(client.GetStream()));
            try {
            } catch(Exception e) {
            } finally {
                input.Close();
                output.Close();
                client.GetStream().Close();
            }
            client.Close();
        }
        /// <summary>
        /// 读入一行
        /// </summary>
        /// <returns>一行数据</returns>
        private string readLine() {
            char c;
            sb = new StringBuilder();
            while(true) {
                c = (char)input.ReadByte();
                if(c == '\n') {
                    break;
                } else if(c == '\r') {
                    continue;
                } else if(c == -1) {
                    System.Threading.Thread.Sleep(1);
                    continue;
                }
                sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
