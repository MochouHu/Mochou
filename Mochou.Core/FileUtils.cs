using Mochou.Core.Extension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Mochou.Core
{
    public class FileUtils
    {
        public static void Delete(string path, Func<FileInfo, FileInfo> condition = null)
        {
            if (File.Exists(path))
                File.Delete(path);
            if (Directory.Exists(path))
                Delete(new DirectoryInfo(path), condition);
        }
        public static void Delete(DirectoryInfo path, Func<FileInfo, FileInfo> condition = null)
        {
            foreach (var file in path.GetFiles()) {
                var delet = file;
                if (condition != null) delet = condition.Invoke(file);
                if (delet != null) delet.Delete();
            }
        }
        /// <summary>
        /// 读取二进制对象 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static T ReadBinObjet<T>(string fileName, T def = default(T))
        {
            T t;
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    t = (T)bf.Deserialize(fs);
                }
            }
            catch (Exception)
            {
                return def;
            }

            if (t == null)
            {
                return def;
            }
            return t;
        }
        /// <summary>
        /// 写二进制对象
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        public static void WriteBinObjet(string fileName, object data)
        {
            FileInfo file = new FileInfo(fileName);
            if (!File.Exists(fileName))
            {
                if (!file.Directory.Exists)
                {
                    file.Directory.Create();
                }
                file.Create().Dispose();
            }
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, data);
            }
        }

        /// <summary>
        /// 读取json格式对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="def"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public static T ReadJson<T>(string fileName, T def = default(T), bool create = false)
        {
            String strJson = ReadString(fileName, default(String), create);

            if (strJson == default(String))
            {
                return def;
            }

            if (!String.IsNullOrWhiteSpace(strJson))
            {
                return JsonConvert.DeserializeObject<T>(strJson);
            }
            return default(T);
        }
        /// <summary>
        /// 读取json格式对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="def"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public static object ReadJson(string fileName, string def = null, bool create = false)
        {
            String strJson = ReadString(fileName, def, create);

            if (String.IsNullOrWhiteSpace(strJson))
            {
                return JsonConvert.DeserializeObject(strJson);
            }
            return null;
        }
        public static void WriteJson(String fileName, Object data, bool isFormatting = false) {
            if (isFormatting)
            {
                WriteString(fileName, FormatUtils.FormatJsonString(JsonConvert.SerializeObject(data)));
            }
            else {
                WriteString(fileName, JsonConvert.SerializeObject(data));
            }
        }
        public static void WriteString(String fileName, String data)
        {
            FileInfo file = new FileInfo(fileName);

            StreamWriter reader = null;

            try
            {
                if (!File.Exists(fileName))
                {
                    if (!file.Directory.Exists)
                    {
                        file.Directory.Create();
                    }
                    reader = new StreamWriter(file.Create());
                }
                else
                {
                    reader = new StreamWriter(fileName);
                }

                reader.Write(data);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
        public static string ReadString(string fileName, string def = null, bool create = false)
        {
            FileInfo file = new FileInfo(fileName);

            StreamReader reader = null;

            try
            {
                if (!File.Exists(fileName))
                {
                    if (!create)
                    {
                        return def;
                    }
                    if (!file.Directory.Exists)
                    {
                        file.Directory.Create();
                    }
                    reader = new StreamReader(file.Create());
                }
                else
                {
                    reader = new StreamReader(fileName);
                }

                string res = reader.ReadToEnd();
                return string.IsNullOrWhiteSpace(res) ? def : res;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public static void AppendString(String fileName, String data)
        {
            FileInfo file = new FileInfo(fileName);

            StreamWriter reader = null;

            try
            {
                if (!File.Exists(fileName))
                {
                    if (!file.Directory.Exists)
                    {
                        file.Directory.Create();
                    }
                    reader = new StreamWriter(file.Create());
                }
                else
                {
                    reader = new StreamWriter(fileName, true);
                }
                reader.Write(data);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
        /// <summary>
        ///  获取过期的文件
        /// </summary>
        /// <param name="Dir"></param>
        /// <param name="scretchTime"></param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        public static List<FileInfo> GetScratchFile(String Dir, DateTime scretchTime, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return GetScratchFile(new DirectoryInfo(Dir), "*.*", scretchTime, searchOption);
        }
        /// <summary>
        ///  获取过期的文件
        /// </summary>
        /// <param name="File"></param>
        /// <param name="scretchTime"></param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        public static List<FileInfo> GetScratchFile(FileInfo File, DateTime scretchTime, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return GetScratchFile(File.Directory, "*.*", scretchTime, searchOption);
        }
        /// <summary>
        ///  获取过期的文件
        /// </summary>
        /// <param name="Dir"></param>
        /// <param name="scretchTime"></param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        public static List<FileInfo> GetScratchFile(DirectoryInfo Dir, DateTime scretchTime, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return GetScratchFile(Dir, "*.*", scretchTime, searchOption);
        }
        /// <summary>
        ///  获取过期的文件
        /// </summary>
        /// <param name="Dir"></param>
        /// <param name="searchPattern"></param>
        /// <param name="scretchTime"></param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        public static List<FileInfo> GetScratchFile(String Dir, String searchPattern, DateTime scretchTime, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return GetScratchFile(new DirectoryInfo(Dir), searchPattern, scretchTime, searchOption);
        }
        /// <summary>
        ///  获取过期的文件
        /// </summary>
        /// <param name="File"></param>
        /// <param name="searchPattern"></param>
        /// <param name="scretchTime"></param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        public static List<FileInfo> GetScratchFile(FileInfo File, String searchPattern, DateTime scretchTime, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return GetScratchFile(File.Directory, searchPattern, scretchTime, searchOption);
        }

        /// <summary>
        /// 获取过期的文件
        /// </summary>
        /// <param name="Dir">文件夹</param>
        /// <param name="searchPattern">筛选条件</param>
        /// <param name="scretchTime">过期时间，在这个时间之前的时间为过期文件</param>
        /// <param name="searchOption">是否包括子文件夹</param>
        /// <returns></returns>
        public static List<FileInfo> GetScratchFile(DirectoryInfo Dir, String searchPattern, DateTime scretchTime, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            List<FileInfo> res = new List<FileInfo>();
            FileInfo[] Files = Dir.GetFiles(searchPattern, searchOption);
            foreach (FileInfo file in Files) {
                if (file.LastAccessTime < scretchTime) {
                    res.Add(file);
                }
            }
            return res;
        }
        public string FileName; //INI文件名  
        //声明读写INI文件的API函数  
        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);
        public static O ReadIniFile<O>(String file) where O : new()
        {
            if (!File.Exists(file)) throw new IOException("文件：" + file + "不存在！");
            FileInfo fileInfo = new FileInfo(file);
            O o = new O();
            var oProp = typeof(O).GetProperties();
            var map = new Dictionary<string, object>();
            var defGroup = typeof(O).Name;
            foreach (var para in oProp)
            {
                IniConfigMaping paraAtt = (IniConfigMaping)Attribute.GetCustomAttribute(para, typeof(IniConfigMaping));
                var group = defGroup;
                var name = para.Name;
                if (paraAtt == null)
                    continue;

                if (!string.IsNullOrWhiteSpace(paraAtt.Name))
                    name = paraAtt.Name;
                if (!string.IsNullOrWhiteSpace(paraAtt.Group))
                    group = paraAtt.Group;

                Byte[] Buffer = new Byte[1024 * 2];
                int bufLen = GetPrivateProfileString(group, name, T.ToString(para.GetValue(o, null)), Buffer, Buffer.GetUpperBound(0), fileInfo.FullName);
                string s = Encoding.GetEncoding(0).GetString(Buffer, 0, bufLen);
                map.Add(para.Name, s);
            }

            return TypeUtils.DicToObjct<O>(map);
        }
        public static void WriteIniFile(String file, object o) 
        {
            if (!File.Exists(file)) throw new IOException("文件：" + file + "不存在！");
            FileInfo fileInfo = new FileInfo(file);
            var oProp = o.GetType().GetProperties();
            var map = new Dictionary<string, object>();
            var defGroup = o.GetType().Name;
            foreach (var para in oProp)
            {
                IniConfigMaping paraAtt = (IniConfigMaping)Attribute.GetCustomAttribute(para, typeof(IniConfigMaping));
                var group = defGroup;
                var name = para.Name;
                if (paraAtt == null)
                    continue;

                if (!string.IsNullOrWhiteSpace(paraAtt.Name))
                    name = paraAtt.Name;
                if (!string.IsNullOrWhiteSpace(paraAtt.Group))
                    group = paraAtt.Group;

                WritePrivateProfileString(group, name, T.ToString(para.GetValue(o, null)), fileInfo.FullName);
            }
        }
    }

    public class FU : FileUtils { }
    /// <summary>
    /// 读取，写入Ini文件类中的Maping设置
    /// </summary>
    public class IniConfigMaping : Attribute
    {
        public string Group;
        public string Name;
    }
}
