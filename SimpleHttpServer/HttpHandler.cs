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
        private Dictionary<string, string> header = new Dictionary<string, string>();
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
                    getPostData();
                    break;
                case "GET":
                    break;
                default:
                    Log.log("Invalid Http Request Method: " + request_method, Log.Type.Error);
                    throw new Exception("Invalid Http Request Method!");
                }
                handleRequest();
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
                Log.log("Invalid Http Request Header!", Log.Type.Error);
                throw new Exception("Invalid Http Request Header!");
            }
            request_method = method[0];
            request_url = method[1];
            request_http_version = method[2];
            Log.log(line, Log.Type.Normal);
        }
        /// <summary>
        /// 获取请求头
        /// </summary>
        private void getHead() {
            string line;
            while((line = readLine()) != string.Empty) {
                int p = line.IndexOf(':');
                if(p < 0) {
                    Log.log("Invalid Http Header Line: " + line, Log.Type.Error);
                    throw new Exception("Invalid Http Header Line!");
                }
                string key = line.Substring(0, p);
                while(p < line.Length && line[p] == ' ') {
                    p++;
                }
                string value = line.Substring(p);
                Log.log(key + ' ' + value, Log.Type.Normal);
                header[key] = value;
            }
        }
        /// <summary>
        /// 读取POST上传的数据
        /// </summary>
        /// <returns>数据流</returns>
        private MemoryStream getPostData() {
            MemoryStream ms = new MemoryStream();
            if(header.ContainsKey("Content-Length")) {
                int length = int.Parse(header["Content-Length"]);
                if(length > MAX_POST_SIZE) {
                    Log.log("POST length too big!", Log.Type.Error);
                    throw new Exception("POST length too big!");
                }
                byte[] buffer = new byte[BUFFER_SIZE];
                while(length > 0) {
                    int read = input.Read(buffer, 0, Math.Min(length, BUFFER_SIZE));
                    if(read == 0) {
                        Log.log("Client Disconnected During Post!", Log.Type.Warning);
                        throw new Exception("Client Disconnected During Post!");
                    }
                    ms.Write(buffer, 0, read);
                    length -= read;
                }
                ms.Seek(0, SeekOrigin.Begin);
            }
            Log.log("Got Post Data!", Log.Type.Normal);
            return ms;
        }
        /// <summary>
        /// 处理请求
        /// </summary>
        private void handleRequest() {
            if(Directory.Exists(Form1.root)) {
            } else {  //路径不存在
            }
        }
        /// <summary>
        /// 输出响应头
        /// </summary>
        /// <param name="status">状态码</param>
        /// <param name="header">响应头</param>
        private void printHeader(int status, Dictionary<string, string> header) {
            string status_string = status_name(status);
            //请求状态
            output.Write(request_http_version);
            output.Write(' ');
            output.Write(status);
            output.Write(' ');
            output.WriteLine(status_string);
            output.WriteLine("Server: C# Simple Http Server 1.0");
            output.WriteLine("Author: Jin Liming, jinliming2@gmail.com");
            output.WriteLine("Connection: close");
            foreach(KeyValuePair<string, string> k in header) {
                output.Write(k.Key);
                output.Write(": ");
                output.WriteLine(k.Value);
            }
            output.WriteLine();
        }
        /// <summary>
        /// HTTP Code状态消息查询
        /// </summary>
        /// <param name="status">code</param>
        /// <returns>状态消息</returns>
        private string status_name(int status) {
            switch(status) {
            default:
                return "";
            }
        }
    }
}
