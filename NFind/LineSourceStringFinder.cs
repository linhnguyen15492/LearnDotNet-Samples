using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class LineSourceStringFinder : IStringFinder
    {
        private readonly ILineSource source;
        private readonly string stringToFind;
        private readonly bool findContainingLines;
        private readonly StringComparison stringComparision;
        private int line;

        public LineSourceStringFinder(ILineSource source, string stringToFind, bool caseSensitive, bool findContainingLines)
        {
            this.source = source;
            this.stringToFind = stringToFind;
            this.findContainingLines = findContainingLines;

            stringComparision = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            line = 0;
        }

        public Line? Next()
        {
            var next = source.ReadLine();
            while (next != null)
            {
                line++;

                bool matched = findContainingLines ? next.Contains(stringToFind, stringComparision) : !next.Contains(stringToFind, stringComparision);

                if (matched)
                {
                    return new Line()
                    {
                        LineNumber = line,
                        Text = next
                    };
                }

                next = source.ReadLine();
            }

            return null;
        }
    }
}
