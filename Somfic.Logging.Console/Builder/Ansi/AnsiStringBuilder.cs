using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Somfic.Logging.Console.Builder.Ansi
{
    /// <inheritdoc />
    public class AnsiStringBuilder : IAnsiStringBuilder
    {
        public AnsiStringBuilder()
        {
            Parts = new List<IAnsiStringBuilderPart>();
            Suffix = new StringBuilder();
        }

        private StringBuilder Suffix { get; }


        /// <inheritdoc />
        public IList<IAnsiStringBuilderPart> Parts { get; }

        /// <inheritdoc />
        public string Build()
        {
            string allParts = string.Join("", Parts.Select(x => x.Build()));
            return $"{allParts}{Suffix}";
        }

        /// <inheritdoc />
        public string BuildClean()
        {
            string allParts = string.Join("", Parts.Select(x => x.BuildClean()));
            return allParts;
        }

        public string BuildWithWrap(int maxSize)
        {
            char[] wrapCharacters =  {' ', '-', ':', ';'};

            StringBuilder wrapped = new StringBuilder();
            Regex wrapRegex = new Regex($@"(?<=[{string.Join("", wrapCharacters)}])");

            int spaceLeft = maxSize;
            foreach (var part in Parts)
            {
                string raw = part.Build();
                string clean = part.BuildClean();

                if (clean.Length <= spaceLeft)
                {
                    // This part fits
                    wrapped.Append(raw);
                }
                else
                {
                    // This part doesn't fit
                    part.Clear();

                    var words = wrapRegex.Split(clean);
                    foreach (string word in words)
                    {
                        if (word.EndsWith(Environment.NewLine))
                        {
                            spaceLeft = maxSize;
                        }

                        if (word.StartsWith(Environment.NewLine))
                        {
                            spaceLeft = maxSize - word.Length;
                        }

                        if (word.Length <= spaceLeft)
                        {
                            part.Append(word);
                        }
                        else
                        {
                            part.Append(Environment.NewLine);
                            part.Append(word);
                            spaceLeft = maxSize;
                        }

                        spaceLeft -= word.Length;
                    }

                    wrapped.Append(part.Build());
                }

                spaceLeft = maxSize;
            }

            return wrapped.ToString();
        }

        public string BuildCleanWithWrap(int maxSize)
        {
            Regex pattern = new Regex($@"(?<line>.{{1,{maxSize}}})(?<!\s)(\s+|$)|(?<line>.+?)(\s+|$)");
            var wrappedLines = pattern.Matches(BuildClean()).Cast<Match>().Select(m => m.Groups["line"].Value);
            return string.Join(Environment.NewLine, wrappedLines);
        }


        /// <inheritdoc />
        public IAnsiStringBuilderPart AddPart(string content = "")
        {
            IAnsiStringBuilderPart part = new AnsiStringBuilderPart(content);
            Parts.Add(part);
            return part;
        }

        /// <inheritdoc />
        public IAnsiStringBuilder EndWith(Color color) => EndWith(color, AnsiColorRegion.Foreground);

        /// <inheritdoc />
        public IAnsiStringBuilder EndWith(Color color, AnsiColorRegion region)
        {
            string colorCode = string.Format(AnsiDefaults.ColorFormat, (int)region, color.R, color.G, color.B);
            Suffix.AppendFormat(AnsiDefaults.Format, colorCode);
            return this;
        }

        /// <inheritdoc />
        public IAnsiStringBuilder EndWith(AnsiCode ansiCode)
        {
            Suffix.AppendFormat(AnsiDefaults.Format, (int)ansiCode);
            return this;
        }

        /// <inheritdoc />
        public IAnsiStringBuilder EndWithBackground(Color color) => EndWith(color, AnsiColorRegion.Background);

        /// <inheritdoc />
        public IAnsiStringBuilder EndWithUnderline(Color color) => EndWith(color, AnsiColorRegion.Underline);
    }
}
