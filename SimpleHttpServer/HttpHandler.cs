using System;
using System.Collections.Generic;
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
        /// 输入数据缓冲区大小
        /// </summary>
        private const int BUFFER_SIZE = 4096;
        /// <summary>
        /// 最大上传数据大小
        /// </summary>
        private const int MAX_POST_SIZE = 5242880;  //5MiBytes
        /// <summary>
        /// 请求记录
        /// </summary>
        private string log;
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
        /// 请求方法
        /// </summary>
        private string request_method;
        /// <summary>
        /// 请求地址
        /// </summary>
        private string request_url;
        /// <summary>
        /// HTTP协议版本
        /// </summary>
        private string request_http_version;
        /// <summary>
        /// 请求头
        /// </summary>
        private Dictionary<string, string> header;
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
                //请求方法
                getMethod();
                //请求头
                getHead();
                switch(request_method) {
                case "POST":
                    break;
                case "GET":
                    break;
                default:
                    log += "Invalid Http Request Method: " + request_method + '\n';
                    throw new Exception("Invalid Http Request Method!");\
                }
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
        /// <summary>
        /// 获取请求方法
        /// </summary>
        private void getMethod() {
            string line = readLine();
            string[] method = line.Split(' ');
            //Method Url Http_version
            if(method.Length != 3) {
                log += "Invalid Http Request Header!\n";
                throw new Exception("Invalid Http Request Header!");
            }
            request_method = method[0];
            request_url = method[1];
            request_http_version = method[2];
            log += line;
        }
        /// <summary>
        /// 获取请求头
        /// </summary>
        private void getHead() {
            string line;
            while((line = readLine()) != string.Empty) {
                int p = line.IndexOf(':');
                if(p < 0) {
                    log += "Invalid Http Header Line: " + line + '\n';
                    throw new Exception("Invalid Http Header Line!");
                }
                string key = line.Substring(0, p);
                while(p < line.Length && line[p] == ' ') {
                    p++;
                }
                string value = line.Substring(p);
                log += key + ' ' + value + '\n';
                header[key] = value;
            }
        }
    }
}
