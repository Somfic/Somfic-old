using System.Drawing;

namespace Somfic.Logging.Console.Builder.Ansi
{
    /// <summary>
    /// A class that builds a pretty base string using ANSI escape-characters
    /// </summary>
    public interface IAnsiStringBuilderPart
    {
        /// <summary>
        /// Builds the string with ANSI escape-characters
        /// </summary>
        string Build();

        /// <summary>
        /// Builds the string without any ANSI escape-characters
        /// </summary>
        string BuildClean();

        /// <summary>
        /// Prepends a colored ANSI escape-character for the foreground
        /// </summary>
        /// <param name="color">The color the foreground should be set to</param>
        IAnsiStringBuilderPart With(Color color);

        /// <summary>
        /// Prepends a colored ANSI escape-character for a specific region
        /// </summary>
        /// <param name="region">The applicable region</param>
        /// <param name="color">The color the region should be set to</param>
        IAnsiStringBuilderPart With(Color color, AnsiColorRegion region);

        /// <summary>
        /// Prepends a colored ANSI escape-character for the background
        /// </summary>
        /// <param name="color">The color the background should be set to</param>
        IAnsiStringBuilderPart WithBackground(Color color);

        /// <summary>
        /// Prepends an ANSI escape-character
        /// </summary>
        /// <param name="ansiCode">The ANSI escape-character</param>
        IAnsiStringBuilderPart With(AnsiCode ansiCode);

        /// <summary>
        /// Prepends a colored ANSI escape-character for the underline
        /// </summary>
        /// <param name="color">The color the underline should be set to</param>
        IAnsiStringBuilderPart WithUnderline(Color color);

        /// <summary>
        /// Overwrites the string's content
        /// </summary>
        /// <param name="content">The new string content</param>
        IAnsiStringBuilderPart Set(string content);

        /// <summary>
        /// Appends to the string's content
        /// </summary>
        /// <param name="content">The string to append</param>
        IAnsiStringBuilderPart Append(string content);

        /// <summary>
        /// Clears the string's content, does not clear ANSI characters
        /// </summary>
        IAnsiStringBuilderPart Clear();
    }
}