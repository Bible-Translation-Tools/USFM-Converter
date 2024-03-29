﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BTTWriterLib;
using USFMConverter.Core.Data;
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
            if (!Directory.Exists(path))
            {
                throw new FileNotFoundException(
                    $"Could not find the specified directory: {path}"
                    );
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start("explorer.exe", @$"/select, {path}");
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", $"-R {path}");
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var processInfo = new ProcessStartInfo("xdg-open", path);
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
        public static async Task<List<(string path, USFMDocument document)>> LoadUSFMsAsync(
            IEnumerable<IProjectItem> files,
            Action<double> progressCallback
        )
        {
            var output = new List<(string path, USFMDocument document)>(files.Count());
            var usfmList = new List<USFMDocument>();
            var fileList = files.ToList();

            var parser = new USFMParser(ignoreUnknownMarkers:true);
            int totalFiles = fileList.Count;

            for (int i = 0; i < totalFiles; i++)
            {
                if (fileList[i] is FileItem)
                {
                    var text = await File.ReadAllTextAsync(fileList[i].Path);
                    output.Add((fileList[i].Path, parser.ParseFromString(text)));
                }
                else if (fileList[i] is WriterProjectItem)
                {
                    var container = new FileSystemResourceContainer(Path.GetDirectoryName(fileList[i].Path));
                    output.Add((fileList[i].Path, BTTWriterLoader.CreateUSFMDocumentFromContainer(container, false)));
                }

                // update progress bar
                var percent = (double)i / totalFiles * 100;
                progressCallback(percent);
            }
            return output;
        }
    }
}
