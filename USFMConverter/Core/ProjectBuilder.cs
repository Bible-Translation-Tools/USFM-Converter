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

        public void SetLineSpacing(LineSpacing lineSpacing)
        {
            renderFormat.LineSpacing = lineSpacing;
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

        public void SetFontSize(TextSize size)
        {
            renderFormat.TextSize = size;
        }

        public void EnableNoteTaking(bool enabled)
        {
            renderFormat.NoteTaking = enabled;
        }

        public void EnableTableOfContents(bool enabled)
        {
            renderFormat.TableOfContents = enabled;
        }
    }
}
