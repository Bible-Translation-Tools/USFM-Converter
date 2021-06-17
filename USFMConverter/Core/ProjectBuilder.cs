using System.Collections.Generic;
using System.IO;
using System.Linq;
using USFMConverter.Core.ConstantValue;
using USFMConverter.Core.Data;

namespace USFMConverter.Core
{
    public class ProjectBuilder
    {
        private Project project;
        private RenderFormat renderFormat;
        private List<string> supportedExtensions = new List<string> { ".usfm", ".txt", ".sfm" };

        public ProjectBuilder()
        {
            project = new Project();
            renderFormat = new RenderFormat();
            project.FormatOptions = renderFormat;
        }

        public Project Build()
        {
            return project;
        }

        public void AddFile(FileInfo file)
        {
            project.Files.Add(file);
        }

        public void AddFiles(ICollection<FileInfo> files)
        {
            project.Files.AddRange(files);
        }

        public void LoadFolder(FileInfo folder)
        {
            var dirinfo = new DirectoryInfo(folder.FullName);
            var filesInFolder = dirinfo.GetFiles("*", SearchOption.AllDirectories);

            var files = filesInFolder
                .Where(f => supportedExtensions.Contains(f.Extension));

            this.AddFiles(files.ToList());
        }

        public void SetProjectName(string name)
        {
            project.Name = name;
        }

        public void RemoveFile(string fileName)
        {
            var fileToRemove = project.Files.Find(f => f.FullName == fileName);
            if (fileToRemove != null)
            {
                project.Files.Remove(fileToRemove);
            }
        }

        public void SetLineSpacing(double spacing)
        {
            renderFormat.LineSpacing = spacing;
        }

        public void SetTextAlignment(TextAlignment alignment)
        {
            renderFormat.TextAlign = alignment;
        }

        public void SetColumns(int count)
        {
            renderFormat.ColumnCount = count;
        }

        public void SetTextDirection(bool leftToRight)
        {
            renderFormat.LeftToRight = leftToRight;
        }

        public void SetChapterBreak(bool breakChapter)
        {
            renderFormat.ChapterBreak = breakChapter;
        }

        public void SetVerseBreak(bool breakVerse)
        {
            renderFormat.VerseBreak = breakVerse;
        }

        public void SetFontSize(int pointSize)
        {
            renderFormat.FontSize = pointSize;
        }
    }
}
