using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using USFMConverter.Core.ConstantValue;
using USFMConverter.Core.Data;
using USFMToolsSharp;
using USFMToolsSharp.Models.Markers;

namespace USFMConverter.Core.Render
{
    public abstract class RenderDocument
    {
        public RenderDocument()
        {
            
        }

        /// <summary>
        /// Parses the given text files into one USFM Document.
        /// </summary>
        /// <param name="files">Text files with USFM format.</param>
        /// <returns>A USFM Document</returns>
        public USFMDocument LoadUSFMs(IEnumerable<FileInfo> files)
        {
            var usfmDoc = new USFMDocument();
            var parser = new USFMParser(new List<string> { "s5" });

            foreach(var file in files)
            {
                var text = File.ReadAllText(file.FullName);
                usfmDoc.Insert(parser.ParseFromString(text));
            }

            return usfmDoc;
        }

        /// <summary>
        /// Parses the given text files into one USFM Document asynchronously.
        /// </summary>
        /// <param name="files">Text files with USFM format.</param>
        /// <param name="progressCallback">Call back for progress bar update.</param>
        /// <returns>A USFM Document</returns>
        public async Task<USFMDocument> LoadUSFMsAsync(
            IEnumerable<FileInfo> files, 
            Action<double> progressCallback
        )
        {
            var usfmDoc = new USFMDocument();
            List<FileInfo> fileList = files.ToList();

            var parser = new USFMParser(new List<string> { "s5" });
            int totalFiles = fileList.Count;

            for (int i = 0; i < totalFiles; i++)
            {
                await Task.Run(() => {
                    var text = File.ReadAllText(fileList[i].FullName);
                    usfmDoc.Insert(parser.ParseFromString(text));
                });
                
                // update progress bar
                var percent = (double)i / totalFiles * 100;
                progressCallback(percent);
            }

            return usfmDoc;
        }

        public void Render(Project project)
        {
            Render(project, LoadUSFMs(project.Files));
        }

        public abstract void Render(Project project, USFMDocument usfmDoc);

        public static RenderDocument GetInstance(FileFormat fileFormat)
        {
            switch (fileFormat)
            {
                case FileFormat.DOCX:
                    return new RenderDocx();
                case FileFormat.HTML:
                    return new RenderHTML();
                default:
                    throw new ArgumentException("Output file format is not supported");
            }
        }
    }
}
