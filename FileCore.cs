using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace VkBotApi
{
    public static class FileCore
    {
        public static string PathAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                              @"\" + "VKChatBot";

        public static string PathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        public static string PathCurrent = Directory.GetCurrentDirectory();
        public static string PathSettings = PathAppData + @"\" + "Settings.dat";

        public static string PathLogs = PathAppData + @"\" + "Logs";
        public static string PathImages = PathAppData + @"\" + "Images";

        public static string PathLog => PathLogs + @"\" + "Log_" + DateTime.Now.ToString("dd-MM-yy") + ".txt";
        public static string PathMailData => PathAppData + @"\" + "MailData.json";

        public static void CreateHidFolder()
        {
            try
            {
                var dataDir = Directory.CreateDirectory(PathAppData);
                dataDir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

                Directory.CreateDirectory(PathLogs);
                Directory.CreateDirectory(PathImages);
            }
            catch (Exception ex)
            {
                ex.Error();
            }
        }

        public static void SaveBinary(string path, object o)
        {
            try
            {
                if (o == null) return;
                using (Stream stream = File.Create(path))
                {
                    var bin = new BinaryFormatter();
                    bin.Serialize(stream, o);
                }
            }
            catch (Exception e)
            {
                e.Error();
            }
        }

        public static object ReadBinary(string path)
        {
            try
            {
                using (Stream stream = File.Open(path, FileMode.Open))
                {
                    var bin = new BinaryFormatter();
                    if (stream.Length > 0)
                    {
                        var res = bin.Deserialize(stream);
                        return res;
                    }
                    return null;
                }
            }
            catch (Exception e)
            {
                e.Error();
                return null;
            }
        }

        public static bool IfFileExist(this string path)
        {
            try
            {
                return File.Exists(path);
            }
            catch (Exception e)
            {
                e.Error();
                return false;
            }
        }

        public static void SaveToFile(this string path, string output, bool add = false)
        {
            try
            {
                using (var writer = new StreamWriter(path, add))
                {
                    writer.WriteLine(output);
                }
            }
            catch (Exception e)
            {
                e.Error();
            }
        }

        public static string ReadFromFile(this string path, string defaulValue = null)
        {
            try
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    StringBuilder sb = new StringBuilder();

                    string s;
                    while ((s = sr.ReadLine()) != null)
                    {
                        sb.Append(s);
                    }
                    string tempReturn = sb.ToString();
                    sb.Clear();
                    return tempReturn;
                }
            }
            catch (Exception e)
            {
                e.Error();
                return defaulValue;
            }
        }

        public static string[] ReadFromFileAllLines(this string path)
        {
            try
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    List<string> array = new List<string>();
                    string s;
                    while ((s = sr.ReadLine()) != null)
                    {
                        array.Add(s);
                    }
                    return array.ToArray();
                }
            }
            catch (Exception e)
            {
                e.Error();
                return null;
            }
        }

        public static void ClearFile(this string path)
        {
            try
            {
                using (new StreamWriter(path, false))
                {
                }
            }
            catch (Exception e)
            {
                e.Error();
            }
        }

        public static void RemoveNull(this string path)
        {
            try
            {
                var dir = new DirectoryInfo(path);
                foreach (var file in dir.GetFiles())
                {
                    if (file.Length <= 1)
                    {
                        DeleteFile(file.FullName);
                    }
                }
            }
            catch (Exception e)
            {
                e.Error();
            }
        }

        public static void DeleteFile(this string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception e)
            {
                e.Error();
            }
        }
    }
}