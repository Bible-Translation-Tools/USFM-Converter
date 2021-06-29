using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="DirectoryNotFoundException"/>
        public static void CheckWritePermission(string path)
        {
            File.OpenWrite(path).Close();
        }

        public static void OpenFileLocation(string path)
        {
            var file = new FileInfo(path);
            if (!file.Exists)
            {
                throw new FileNotFoundException(
                    "Could not find the specified path: " + path, 
                    file.Name
                );
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start("explorer.exe", @"/select," + file.FullName);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", "-R " + file.FullName);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var processInfo = new ProcessStartInfo("xdg-open", file.DirectoryName);
                var process = new Process { StartInfo = processInfo };
                process.Start();
            }
        }
    }
}
