using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USFMToolsSharp.Models.Markers;

namespace USFMConverter.Core.Util
{
    public class BooksComparison : IComparer<USFMDocument>
    {
        private List<string> bookNames = new() { "GEN", "EXO", "LEV", "NUM", "DEU", "JOS", "JDG", "RUT", "1SA", "2SA", "1KI", "2KI", "1CH", "2CH", "EZR", "NEH", "EST", "JOB", "PSA", "PRO", "ECC", "SNG", "ISA", "JER", "LAM", "EZK", "DAN", "HOS", "JOL", "AMO", "OBA", "JON", "MIC", "NAM", "HAB", "ZEP", "HAG", "ZEC", "MAL", "MAT", "MRK", "LUK", "JHN", "ACT", "ROM", "1CO", "2CO", "GAL", "EPH", "PHP", "COL", "1TH", "2TH", "1TI", "2TI", "TIT", "PHM", "HEB", "JAS", "1PE", "2PE", "1JN", "2JN", "3JN", "JUD", "REV" };

        private int CompareBookNames(string a, string b)
        {
            return bookNames.IndexOf(a.ToUpper())
                .CompareTo(
                    bookNames.IndexOf(b.ToUpper())
                );
        }

        public int Compare(USFMDocument? x, USFMDocument? y)
        {
            TOC3Marker? toc3X = x.Contents.FirstOrDefault(marker => marker is TOC3Marker) as TOC3Marker;
            TOC3Marker? toc3Y = y.Contents.FirstOrDefault(marker => marker is TOC3Marker) as TOC3Marker;

            if (toc3X == null)
            {
                return 1;
            }
            if (toc3Y == null)
            {
                return -1;
            }

            return CompareBookNames(toc3X.BookAbbreviation.ToUpper(), toc3Y.BookAbbreviation.ToUpper());
        }
    }
}
