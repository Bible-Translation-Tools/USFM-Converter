using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USFMConverter.Core.Util
{
    public static class FileSystem
    {
        public static ICollection<FileInfo> GetFilesInDir(FileInfo dir)
        {
            var dirInfo = new DirectoryInfo(dir.FullName);
            var allFiles = dirInfo.GetFiles("*", SearchOption.AllDirectories);

            return allFiles;
        }

        public static ICollection<FileInfo> GetFilesInDir(FileInfo dir, IEnumerable<string> extensions)
        {
            List<FileInfo> files = new();
            var dirInfo = new DirectoryInfo(dir.FullName);
            var allFiles = dirInfo.GetFiles("*", SearchOption.AllDirectories);
            foreach (FileInfo file in allFiles)
            {
                if (extensions.Contains(file.Extension))
                {
                    files.Add(file);
                }
            }

            return files;
        }
    }
}
