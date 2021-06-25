using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using USFMConverter.Core.Data;
using USFMToolsSharp;
using USFMToolsSharp.Models.Markers;

namespace USFMConverter.Core.Render
{
    public abstract class RenderDocument
    {
        private Action<double> UpdateProgress;

        public RenderDocument(Action<double>? updateProgress = null)
        {
            if (updateProgress != null)
            {
                UpdateProgress = updateProgress;
            }
            else
            {
                UpdateProgress = new Action<double>((value) => { return; });
            }
        }

        protected USFMDocument LoadUSFM(IEnumerable<FileInfo> files)
        {
            var usfmDoc = new USFMDocument();
            List<FileInfo> fileList = files.ToList();

            var parser = new USFMParser(new List<string> { "s5" });
            int totalFiles = fileList.Count;

            for (int i = 0; i < totalFiles; i++)
            {
                var text = File.ReadAllText(fileList[i].FullName);

                usfmDoc.Insert(parser.ParseFromString(text));
                // update progress bar
                var percent = (double)i / totalFiles * 100;
                UpdateProgress(percent);
            }

            return usfmDoc;
        }

        public void Render(Project project)
        {
            Render(project, LoadUSFM(project.Files));
        }

        public abstract void Render(Project project, USFMDocument usfmDoc);
    }
}
