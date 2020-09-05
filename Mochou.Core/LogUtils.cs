using Mochou.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Mochou.Core
{
    public class LogUtils
    {
        public static String LOG_DIR = "log";
        public static String LOG_FILE = "log";

        private static bool del_flag = false;
        private static String getPath()
        {
            if (!del_flag)
            {
                del_flag = !del_flag;

                DirectoryInfo log = new DirectoryInfo(LOG_DIR);
                if (log.Exists)
                {
                    foreach (var file in log.GetFiles())
                    {
                        //只保留一个月时间的日志
                        if (file.LastWriteTime < DateTime.Now.AddMonths(-1))
                        {
                            file.Delete();
                        }
                    }
                }
            }

            return LOG_DIR + "/" + LOG_FILE + DateTime.Now.ToString("d").Replace("/", "") + ".log";
        }

        public static void log(String Msg)
        {
            //using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"C:\Interfacelog\test.txt", true))
            //{
            //    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + Msg);
            //}
            StringBuilder sb = new StringBuilder("\n\r时间：").Append(DateTime.Now).Append("\t 消息：\t ").Append(Msg).Append("\r\n");
            FileUtils.AppendString(getPath(), sb.ToString());
        }
    }

    public class Log : LogUtils { }
}
