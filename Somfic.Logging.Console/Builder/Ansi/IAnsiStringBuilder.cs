using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Somfic.Logging.Console.Builder.Ansi
{
    /// <summary>
    /// A class that builds a string using multiple ANSI-escape-characters-filled strings
    /// </summary>
    public interface IAnsiStringBuilder
    {
        IList<IAnsiStringBuilderPart> Parts { get; }

        /// <summary>
        /// Builds the string with ANSI escape-characters
        /// </summary>
        string Build();

        /// <summary>
        /// Builds the string without any ANSI escape-characters
        /// </summary>
        string BuildClean();

        /// <summary>
        /// Builds the string with ANSI escape-characters with soft wrap
        /// <param name="maxSize">The maximum width of the sentence</param>
        /// </summary>
        string BuildWithWrap(int maxSize);

        /// <summary>
        /// Builds the string without any ANSI escape-characters with soft wrap
        /// <param name="maxSize">The maximum width of the sentence</param>
        /// </summary>
        string BuildCleanWithWrap(int maxSize);

        /// <summary>
        /// Creates a new <see cref="IAnsiStringBuilderPart"/> and adds it to the builder
        /// </summary>
        IAnsiStringBuilderPart AddPart(string content = "");

        /// <summary>
        /// Appends a colored ANSI escape-character for the foreground
        /// </summary>
        /// <param name="color">The color the foreground should be set to</param>
        IAnsiStringBuilder EndWith(Color color);

        /// <summary>
        /// Appends a colored ANSI escape-character for a specific region
        /// </summary>
        /// <param name="region">The applicable region</param>
        /// <param name="color">The color the region should be set to</param>
        IAnsiStringBuilder EndWith(Color color, AnsiColorRegion region);

        /// <summary>
        /// Appends an ANSI escape-character
        /// </summary>
        /// <param name="ansiCode">The ANSI escape-character</param>
        IAnsiStringBuilder EndWith(AnsiCode ansiCode);

        /// <summary>
        /// Appends a colored ANSI escape-character for the background
        /// </summary>
        /// <param name="color">The color the background should be set to</param>
        IAnsiStringBuilder EndWithBackground(Color color);

        /// <summary>
        /// Appends a colored ANSI escape-character for the underline
        /// </summary>
        /// <param name="color">The color the underline should be set to</param>
        IAnsiStringBuilder EndWithUnderline(Color color);
    }
}
