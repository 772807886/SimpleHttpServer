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
                printHeader(500);  //服务器错误响应
            } finally {
                output.Flush();
                output.BaseStream.Flush();
                input.Close();
                output.Close();
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
                request_url = request_url.Replace('/', '\\');
                if(request_url.EndsWith("\\")) {
                    if(File.Exists(Form1.root + request_url + "index.html")) {
                        success(Form1.root + request_url + "index.html");
                    } else if(File.Exists(Form1.root + request_url + "index.htm")) {
                        success(Form1.root + request_url + "index.htm");
                    } else {
                        printHeader(403);
                    }
                } else if(File.Exists(Form1.root + request_url)) {
                    success(Form1.root + request_url);
                }
            } else {  //路径不存在
                printHeader(404);
            }
        }
        private void success(string file) {
            string type = "text/html";
            int p = file.LastIndexOf('\\');
            if(p > 0) {
                p = file.IndexOf('.', p);
                if(p > 0) {
                    type = file_mime(file.Substring(p + 1));
                }
            }
            Dictionary<string, string> head = new Dictionary<string, string>();
            head["Content-Type"] = type;
            printHeader(200, head);
            FileStream fs = File.Open(file, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[BUFFER_SIZE];
            int count;
            while((count = fs.Read(buffer, 0, BUFFER_SIZE)) != 0) {
                output.BaseStream.Write(buffer, 0, count);
            }
            output.BaseStream.Flush();
        }
        /// <summary>
        /// 输出响应头
        /// </summary>
        /// <param name="status">状态码</param>
        /// <param name="header">响应头</param>
        private void printHeader(int status, Dictionary<string, string> header = null) {
            string status_string = status_name(status);
            //请求状态
            output.Write(request_http_version);
            output.Write(' ');
            output.Write(status);
            output.Write(' ');
            output.WriteLine(status_string);
            //响应头
            output.WriteLine("Server: C# Simple Http Server 1.0");
            output.WriteLine("Author: Jin Liming, jinliming2@gmail.com");
            output.WriteLine("Connection: close");
            if(header != null) {
                foreach(KeyValuePair<string, string> k in header) {
                    output.Write(k.Key);
                    output.Write(": ");
                    output.WriteLine(k.Value);
                }
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
            case 100:
                return "Continue";
            case 101:
                return "Switching Protocols";
            case 102:
                return "Processing";
            case 200:
                return "OK";
            case 201:
                return "Created";
            case 202:
                return "Accepted";
            case 203:
                return "Non-Authoritative Information";
            case 204:
                return "No Content";
            case 205:
                return "Reset Content";
            case 206:
                return "Partial Content";
            case 207:
                return "Multi-Status";
            case 300:
                return "Multiple Choices";
            case 301:
                return "Moved Permanently";
            case 302:
                return "Move temporarily";
            case 303:
                return "See Other";
            case 304:
                return "Not Modified";
            case 305:
                return "Use Proxy";
            case 306:
                return "Switch Proxy";
            case 307:
                return "Temporary Redirect";
            case 400:
                return "Bad Request";
            case 401:
                return "Unauthorized";
            case 402:
                return "Payment Required";
            case 403:
                return "Forbidden";
            case 404:
                return "Not Found";
            case 405:
                return "Method Not Allowed";
            case 406:
                return "Not Acceptable";
            case 407:
                return "Proxy Authentication Required";
            case 408:
                return "Request Timeout";
            case 409:
                return "Conflict";
            case 410:
                return "Gone";
            case 411:
                return "Length Required";
            case 412:
                return "Precondition Failed";
            case 413:
                return "Request Entity Too Large";
            case 414:
                return "Request-URI Too Long";
            case 415:
                return "Unsupported Media Type";
            case 416:
                return "Requested Range Not Satisfiable";
            case 417:
                return "Expectation Failed";
            case 421:
                return "There are too many connections from your internet address";
            case 422:
                return "Unprocessable Entity";
            case 423:
                return "Locked";
            case 424:
                return "Failed Dependency";
            case 425:
                return "Unordered Collection";
            case 426:
                return "Upgrade Required";
            case 449:
                return "Retry With";
            case 500:
                return "Internal Server Error";
            case 501:
                return "Not Implemented";
            case 502:
                return "Bad Gateway";
            case 503:
                return "Service Unavailable";
            case 504:
                return "Gateway Timeout";
            case 505:
                return "HTTP Version Not Supported";
            case 506:
                return "Variant Also Negotiates";
            case 507:
                return "Insufficient Storage";
            case 509:
                return "Bandwidth Limit Exceeded";
            case 510:
                return "Not Extended";
            case 600:
                return "Unparseable Response Headers";
            default:
                return "";
            }
        }
        private string file_mime(string extension) {
            switch(extension) {
            case "323":
                return "text/h323";
            case "acx":
                return "application/internet-property-stream";
            case "ai":
            case "eps":
            case "ps":
                return "application/postscript";
            case "aif":
            case "aifc":
            case "aiff":
                return "audio/x-aiff";
            case "asf":
            case "asr":
            case "asx":
                return "video/x-ms-asf";
            case "au":
                return "audio/basic";
            case "avi":
                return "video/x-msvideo";
            case "axs":
                return "application/olescript";
            case "bas":
            case "c":
            case "cpp":
            case "h":
            case "txt":
                return "text/plain";
            case "bcpio":
                return "application/x-bcpio";
            case "bmp":
                return "image/bmp";
            case "cat":
                return "application/vnd.ms-pkiseccat";
            case "cdf":
                return "application/x-cdf";
            case "cer":
            case "crt":
            case "der":
                return "application/x-x509-ca-cert";
            case "clp":
                return "application/x-msclip";
            case "cmx":
                return "image/x-cmx";
            case "cod":
                return "image/cis-cod";
            case "cpio":
                return "application/x-cpio";
            case "crd":
                return "application/x-mscardfile";
            case "crl":
                return "application/pkix-crl";
            case "csh":
                return "application/x-csh";
            case "css":
                return "text/css";
            case "dcr":
            case "dir":
            case "dxr":
                return "application/x-director";
            case "dll":
                return "application/x-msdownload";
            case "doc":
            case "dot":
                return "application/msword";
            case "docx":
                return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            case "dvi":
                return "application/x-dvi";
            case "etx":
                return "text/x-setext";
            case "evy":
                return "application/envoy";
            case "fif":
                return "application/fractals";
            case "flr":
                return "x-world/x-vrml";
            case "gif":
                return "image/gif";
            case "gtar":
                return "application/x-gtar";
            case "gz":
                return "application/x-gzip";
            case "hdf":
                return "application/x-hdf";
            case "hlp":
                return "application/winhlp";
            case "hqx":
                return "application/mac-binhex40";
            case "hta":
                return "application/hta";
            case "htc":
                return "text/x-component";
            case "htm":
            case "html":
            case "stm":
                return "text/html";
            case "htt":
                return "text/webviewhtml";
            case "ico":
                return "image/x-icon";
            case "ief":
                return "image/ief";
            case "iii":
                return "application/x-iphone";
            case "ins":
            case "isp":
                return "application/x-internet-signup";
            case "jfif":
                return "image/pipeg";
            case "jpe":
            case "jpeg":
            case "jpg":
                return "image/jpeg";
            case "js":
                return "application/x-javascript";
            case "latex":
                return "application/x-latex";
            case "lsf":
            case "lsx":
                return "video/x-la-asf";
            case "m13":
            case "m14":
            case "mvb":
                return "application/x-msmediaview";
            case "m3u":
                return "audio/x-mpegurl";
            case "man":
                return "application/x-troff-man";
            case "mdb":
                return "application/x-msaccess";
            case "me":
                return "application/x-troff-me";
            case "mht":
            case "mhtml":
            case "nws":
                return "message/rfc822";
            case "mid":
            case "midi":
            case "rmi":
                return "audio/mid";
            case "mny":
                return "application/x-msmoney";
            case "mov":
            case "qt":
                return "video/quicktime";
            case "movie":
                return "video/x-sgi-movie";
            case "mp2":
            case "mpa":
            case "mpe":
            case "mpeg":
            case "mpg":
            case "mpv2":
                return "video/mpeg";
            case "mp3":
                return "audio/mpeg";
            case "mpp":
                return "application/vnd.ms-project";
            case "ms":
                return "application/x-troff-ms";
            case "oda":
                return "application/oda";
            case "p10":
                return "application/pkcs10";
            case "p12":
            case "pfx":
                return "application/x-pkcs12";
            case "p7b":
            case "spc":
                return "application/x-pkcs7-certificates";
            case "p7c":
            case "p7m":
                return "application/x-pkcs7-mime";
            case "p7r":
                return "application/x-pkcs7-certreqresp";
            case "p7s":
                return "application/x-pkcs7-signature";
            case "pbm":
                return "image/x-portable-bitmap";
            case "pdf":
                return "application/pdf";
            case "pgm":
                return "image/x-portable-graymap";
            case "pko":
                return "application/ynd.ms-pkipko";
            case "pma":
            case "pmc":
            case "pml":
            case "pmr":
            case "pmw":
                return "application/x-perfmon";
            case "png":
                return "image/png";
            case "pnm":
                return "image/x-portable-anymap";
            case "pot,":
            case "pps":
            case "ppt":
                return "application/vnd.ms-powerpoint";
            case "pptx":
                return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
            case "ppsx":
                return "application/vnd.openxmlformats-officedocument.presentationml.slideshow";
            case "ppm":
                return "image/x-portable-pixmap";
            case "prf":
                return "application/pics-rules";
            case "pub":
                return "application/x-mspublisher";
            case "ra":
            case "ram":
                return "audio/x-pn-realaudio";
            case "ras":
                return "image/x-cmu-raster";
            case "rgb":
                return "image/x-rgb";
            case "rm":
                return "application/vnd.rn-realmedia";
            case "roff":
            case "t":
            case "tr":
                return "application/x-troff";
            case "rtf":
                return "application/rtf";
            case "rtx":
                return "text/richtext";
            case "scd":
                return "application/x-msschedule";
            case "sct":
                return "text/scriptlet";
            case "setpay":
                return "application/set-payment-initiation";
            case "setreg":
                return "application/set-registration-initiation";
            case "sh":
                return "application/x-sh";
            case "shar":
                return "application/x-shar";
            case "sit":
                return "application/x-stuffit";
            case "snd":
                return "audio/basic";
            case "spl":
                return "application/futuresplash";
            case "src":
                return "application/x-wais-source";
            case "sst":
                return "application/vnd.ms-pkicertstore";
            case "stl":
                return "application/vnd.ms-pkistl";
            case "svg":
                return "image/svg+xml";
            case "sv4cpio":
                return "application/x-sv4cpio";
            case "sv4crc":
                return "application/x-sv4crc";
            case "swf":
                return "application/x-shockwave-flash";
            case "tar":
                return "application/x-tar";
            case "tcl":
                return "application/x-tcl";
            case "tex":
                return "application/x-tex";
            case "texi":
            case "texinfo":
                return "application/x-texinfo";
            case "tgz":
                return "application/x-compressed";
            case "tif":
            case "tiff":
                return "image/tiff";
            case "trm":
                return "application/x-msterminal";
            case "tsv":
                return "text/tab-separated-values";
            case "uls":
                return "text/iuls";
            case "ustar":
                return "application/x-ustar";
            case "vcf":
                return "text/x-vcard";
            case "vrml":
            case "wrl":
            case "wrz":
            case "xaf":
            case "xof":
                return "x-world/x-vrml";
            case "wav":
                return "audio/x-wav";
            case "wcm":
            case "wdb":
            case "wks":
            case "wps":
                return "application/vnd.ms-works";
            case "wmf":
                return "application/x-msmetafile";
            case "wri":
                return "application/x-mswrite";
            case "xbm":
                return "image/x-xbitmap";
            case "xla":
            case "xlc":
            case "xlm":
            case "xls":
            case "xlt":
            case "xlw":
                return "application/vnd.ms-excel";
            case "xlsx":
                return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            case "xpm":
                return "image/x-xpixmap";
            case "xwd":
                return "image/x-xwindowdump";
            case "z":
                return "application/x-compress";
            case "zip":
                return "application/x-zip-compressed";
            case "bin":
            case "cab":
            case "chm":
            case "class":
            case "dms":
            case "exe":
            case "msi":
            case "lha":
            case "lzh":
            case "ocx":
            case "rar":
            default:
                return "application/octet-stream";
            }
        }
    }
}
