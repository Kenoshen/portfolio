using System;
using System.IO;

namespace Winger.Utils
{
    public static class FileUtils
    {
        public static string FileToString(string filePath)
        {
            FileStream fileStream = File.Open(filePath, FileMode.Open);
            if (fileStream.CanRead)
            {
                byte[] data = new byte[fileStream.Length];
                fileStream.Read(data, 0, data.Length);
                fileStream.Close();
                string str = System.Text.Encoding.Default.GetString(data);
                int weirdChars = 0;
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] > 150)
                    {
                        weirdChars++;
                    }
                    else
                    {
                        break;
                    }
                }
                return str.Substring(weirdChars, str.Length - weirdChars);
            }
            fileStream.Close();
            throw new Exception("The file cannot be read because of its file permissions");
        }


        public static string AbsoluteFilePath(string filePath)
        {
            return Path.GetFullPath(filePath);
        }


        public static bool IsDirectory(string filePath)
        {
            string absolute = Path.GetFullPath(filePath);
            FileAttributes attr = File.GetAttributes(absolute);
            return ((attr & FileAttributes.Directory) == FileAttributes.Directory);
        }


        public static string FilePathToDir(string filePath)
        {
            if (IsDirectory(filePath))
            {
                return filePath;
            }
            else
            {
                return Directory.GetParent(filePath).FullName;
            }
        }


        public static string RemoveExtension(string filePath)
        {
            int period = filePath.IndexOf('.');
            return filePath.Remove(period);
        }
    }
}
