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
        public USFMDocument LoadUSFM(IEnumerable<FileInfo> files)
        {
            var usfmDoc = new USFMDocument();
            var parser = new USFMParser(new List<string> { "s5" });

            foreach (var file in files)
            {
                var text = File.ReadAllText(file.FullName);

                usfmDoc.Insert(parser.ParseFromString(text));
                // increase progress bar
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
