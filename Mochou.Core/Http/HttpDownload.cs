using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Schema;
using Newtonsoft.Json;

namespace Mochou.Core.Http
{
    public class HttpDownload
    {
        public HttpDownload(String url, String localfile, int buffer = 1024 * 1024, String LocalfileWithSuffix = null) {
            this.URL = url;
            BufferSize = buffer;
            this.Localfile = localfile;
            LocalfileReal = localfile;
            this.LocalfileWithSuffix = LocalfileWithSuffix;
        }

        public int BufferSize { get; } = 1024 * 1024;
        public String URL { get; }
        /// <summary>
        /// 请求参数
        /// </summary>
        public object Data;

        /// <summary>
        /// 原始需求本地地址
        /// </summary>
        public string Localfile { get; }
        public String LocalfileReal { get; private set; }
        public String LocalfileWithSuffix { get; private set; } = null;

        public long CurrPostion { get; private set; } = 0;
        public HttpDownLoadFileInfo FileInfo { get; private set; }

        private HttpWebRequest request = null;
        private HttpWebResponse response = null;

        /// <summary>
        /// 下载中的后缀，下载完成去掉
        /// </summary>
        public const string Suffix = ".downloading";

        public event Action<String, String, int> DownloadPercent;
        public event Action<String, String> DownloadOK;

        /// <summary>
        /// Http方式下载文件
        /// </summary>
        /// <param name="url">http地址</param>
        /// <param name="localfile">本地文件</param>
        /// <returns></returns>
        public bool DownloadFile()
        {
            init();
            if (CurrPostion == FileInfo.FileSize) {
                //已经下载完成
                DownloadFileOk();
                return true;
            }
            using (Stream writeStream = File.OpenWrite(LocalfileWithSuffix)) {
                using (Stream readStream = response.GetResponseStream())
                {
                    byte[] btArray = new byte[BufferSize];
                    int contentSize = 0;
                    while ((contentSize = readStream.Read(btArray, 0, btArray.Length)) > 0)
                    {
                        writeStream.Write(btArray, 0, contentSize);
                        CurrPostion += contentSize;
                        //通知进度
                        DownloadPercent?.Invoke(LocalfileReal, LocalfileWithSuffix, (int)(CurrPostion * 100 / FileInfo.FileSize));
                    }
                }
            }
            DownloadFileOk();
            return true;
        }

        /// <summary>
        /// 下载完成
        /// </summary>
        private void DownloadFileOk()
        {
            try
            {
                if (File.Exists(LocalfileReal))
                    File.Delete(LocalfileReal);

                new FileInfo(LocalfileWithSuffix).CopyTo(LocalfileReal);
             
                ////去掉.downloading后缀
                //byte[] btArray = new byte[BufferSize];
                //int contentSize;
                //while ((contentSize = localfileWithSuffixWriteStream.Read(btArray, 0, btArray.Length)) > 0)
                //{
                //    LocalfileRealStream.Write(btArray, 0, contentSize);
                //}
            }
            finally
            {
                //通知完成
                DownloadOK?.Invoke(LocalfileReal, LocalfileWithSuffix);
            }

            //删除临时文件
            if (File.Exists(LocalfileWithSuffix))
            {
                File.Delete(LocalfileWithSuffix);
            }
        }


        private void init() {
            Close();
            vObj();
          
            CreateRequestAndResponse();
            FileInfo = GetHttpDownLoadFileInfo();

            //如果是文件夹，就采用服务器文件名称
            if (Directory.Exists(LocalfileReal))
            {
                LocalfileReal = Path.Combine(LocalfileReal, FileInfo.FileName);
            }

            //如果传了临时文件名，需要设置下载开始点
            if (!String.IsNullOrWhiteSpace(LocalfileWithSuffix))
            {
                if (File.Exists(LocalfileWithSuffix))
                {
                    FileInfo fileInfo = new FileInfo(LocalfileWithSuffix);
                    //文件存在并且大小正确
                    if (fileInfo.Length <= FileInfo.FileSize)
                    {
                        //获取文件开始位置
                        CurrPostion = fileInfo.Length;
                        //重新初始化连接
                        CreateRequestAndResponse();
                    }
                    else
                    {
                        //文件大小有误改文件有错
                        File.Delete(LocalfileWithSuffix);
                        File.Create(LocalfileWithSuffix).Close();
                    }
                }
                else
                {
                    //文件不存在，创建临时文件
                    File.Create(LocalfileWithSuffix).Close();
                }
            }
            else
            {
                LocalfileWithSuffix = LocalfileReal + Suffix;
                if (File.Exists(LocalfileWithSuffix))
                    File.Delete(LocalfileWithSuffix);
                File.Create(LocalfileWithSuffix).Close();
            }
        }

        private void CreateRequestAndResponse() {
            request = HU.Create(URL, Data);
            //从当前位置开始下载
            request.AddRange(CurrPostion);
            response = (HttpWebResponse)request.GetResponse();
            //if (response.StatusCode != HttpStatusCode.OK)//未请求成功
            //    throw new HttpListenerException(T.ToInt(response.StatusCode), HU.GetResponseStreamData(response));
            if (string.IsNullOrWhiteSpace(response.Headers["Content-Disposition"]))
            {
                //不是一个文件
                throw new Exception("this url is not a file");
            }
            if (response.ContentLength == 0)//空文件
                throw new Exception("empty file");
        }

        public HttpDownLoadFileInfo GetHttpDownLoadFileInfo() {
            if (request == null) {
                throw new Exception("not initialized");
            }
            
            String fileName = SU.InTheMiddle(response.Headers["Content-Disposition"], "filename=", "");
            return new HttpDownLoadFileInfo() { FileName = fileName, FileSize=response.ContentLength  };
        }

        public void Close() {
            if (response != null)
            {
                response.Close();
            }
            if (request != null)
            {
                request.Abort();
            }
        }

        /// <summary>
        /// 验证对象
        /// </summary>
        private void vObj() {
            if (string.IsNullOrEmpty(URL) || string.IsNullOrEmpty(Localfile))
                throw new Exception("parameters of the abnormal");
        }

        public static DownloadReslut Download(String url, String fileNameOrDirName, object data = null)
        {
            HttpDownload download = new HttpDownload(url, fileNameOrDirName);
            download.Data = data;
            try {
                download.DownloadFile();
            } catch (Exception e) {
                if (download.response != null)
                    using (StreamReader srReader = new StreamReader(download.response.GetResponseStream(), HttpSetting.DefaultEncoding))
                       return DownloadReslut.Messages(srReader.ReadToEnd());
                else
                    throw new HttpDownloadException(e) ;
            }

            return DownloadReslut.File(download.LocalfileReal);
        }

        public static String DownloadFile(String url, String fileNameOrDirName, object data = null, String tempFileName = null)
        {
            HttpDownload download = new HttpDownload(url, fileNameOrDirName);
            download.LocalfileWithSuffix = tempFileName;
            download.Data = data;
            download.DownloadFile();
            return download.LocalfileReal;
        }
    }

    public class HttpDownLoadFileInfo {
        public String FileName;
        public long FileSize;
    }
    public class DownloadReslut
    {
        public bool IsFile { get; internal set; } = true;
        public string Message { get; internal set; }
        public string FileName { get; internal set; }

        internal static DownloadReslut File(string fileName)
        {
            return new DownloadReslut() { FileName = fileName };
        }
        internal static DownloadReslut Messages(string Message)
        {
            return new DownloadReslut() { IsFile = false, Message = Message };
        }

        public T GetMessage<T>() {
            return JsonConvert.DeserializeObject<T>(Message);
        }
    }
}
