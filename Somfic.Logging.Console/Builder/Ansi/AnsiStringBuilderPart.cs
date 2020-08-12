using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Somfic.Logging.Console.Builder.Ansi
{
    /// <inheritdoc />
    public class AnsiStringBuilderPart : IAnsiStringBuilderPart
    {
        internal AnsiStringBuilderPart(string content)
        {
            Prefix = new StringBuilder();
            Content = new StringBuilder(content);
            Suffix = new StringBuilder();
        }

        private StringBuilder Prefix { get; }
        private StringBuilder Content { get; }
        private StringBuilder Suffix { get; }

        /// <inheritdoc />
        public string Build() => $"{Prefix}{Content}{Suffix}";

        /// <inheritdoc />
        public string BuildClean() => Content.ToString();

        /// <inheritdoc />
        public IAnsiStringBuilderPart With(Color color) => With(color, AnsiColorRegion.Foreground);

        /// <inheritdoc />
        public IAnsiStringBuilderPart With(Color color, AnsiColorRegion region)
        {
            string colorCode = string.Format(AnsiDefaults.ColorFormat, (int)region, color.R, color.G, color.B);
            Prefix.AppendFormat(AnsiDefaults.Format, colorCode);
            return this;
        }

        /// <inheritdoc />
        public IAnsiStringBuilderPart With(AnsiCode ansiCode)
        {
            Prefix.AppendFormat(AnsiDefaults.Format, (int)ansiCode);
            return this;
        }

        /// <inheritdoc />
        public IAnsiStringBuilderPart WithBackground(Color color) => With(color, AnsiColorRegion.Background);

        /// <inheritdoc />
        public IAnsiStringBuilderPart WithUnderline(Color color)
        {
            return With(color, AnsiColorRegion.Underline)
                  .With(AnsiCode.Underline);
        }

        /// <inheritdoc />
        public IAnsiStringBuilderPart Set(string content)
        {
            return Clear().Append(content);
        }

        /// <inheritdoc />
        public IAnsiStringBuilderPart Append(string content)
        {
            Content.Append(content);
            return this;
        }

        /// <inheritdoc />
        public IAnsiStringBuilderPart Clear()
        {
            Content.Clear();
            return this;
        }
    }
}
