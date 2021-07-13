using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using USFMToolsSharp;
using USFMToolsSharp.Models.Markers;

namespace USFMConverter.Core.Util
{
    public static class FileSystem
    {
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
        public static bool CheckWritePermission(string path)
        {
            File.OpenWrite(path).Close();
            return true;
        }

        public static void OpenFileLocation(string path)
        {
            var file = new FileInfo(path);
            if (!file.Exists)
            {
                throw new FileNotFoundException(
                    "Could not find the specified path: " + path
                    );
            }

            var filePath = $"\"{file.FullName}\""; // preserve spaces with wrapping double quotes
            var dir = $"\"{file.DirectoryName}\"";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start("explorer.exe", @"/select," + filePath);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", "-R " + filePath);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var processInfo = new ProcessStartInfo("xdg-open", dir);
                var process = new Process { StartInfo = processInfo };
                process.Start();
            }
        }

        public static void OpenFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(
                    "Could not find the specified file at: " + path
                    );
            }

            path = $"\"{path}\""; // preserve spaces with wrapping double quotes

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo(path)
                {
                    UseShellExecute = true
                });
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", path);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var processInfo = new ProcessStartInfo("xdg-open", path);
                var process = new Process { StartInfo = processInfo };
                process.Start();
            }
        }

        /// <summary>
        /// Parses the given text files into one USFM Document asynchronously.
        /// </summary>
        /// <param name="files">Text files with USFM format.</param>
        /// <param name="progressCallback">Call back for progress bar update.</param>
        /// <returns>A USFM Document</returns>
        public static async Task<USFMDocument> LoadUSFMsAsync(
            IEnumerable<string> files,
            Action<double> progressCallback
        )
        {
            var usfmList = new List<USFMDocument>();
            List<string> fileList = files.ToList();

            var parser = new USFMParser(new List<string> { "s5" });
            int totalFiles = fileList.Count;

            for (int i = 0; i < totalFiles; i++)
            {
                await Task.Run(() => {
                    var text = File.ReadAllText(fileList[i]);
                    usfmList.Add(parser.ParseFromString(text));
                });

                // update progress bar
                var percent = (double)i / totalFiles * 100;
                progressCallback(percent);
            }

            // sort by biblical order of books
            usfmList.Sort(new BooksComparison());

            var usfmDoc = new USFMDocument();
            foreach (var usfm in usfmList)
            {
                usfmDoc.Insert(usfm);
            }
            return usfmDoc;
        }
    }
}
