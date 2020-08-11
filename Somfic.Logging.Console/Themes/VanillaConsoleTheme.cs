using System;
using System.Drawing;

namespace Somfic.Logging.Console.Themes
{
    /// <summary>
    /// A default 16-color Console Theme
    /// </summary>
    public class VanillaConsoleTheme : IConsoleTheme
    {
        internal VanillaConsoleTheme()
        {

        }

        public Color Background => ConsoleColor.Black.ToColor();
        public Color Text => ConsoleColor.Gray.ToColor();
        public Color SecondaryText => ConsoleColor.DarkGray.ToColor();
        public Color PunctuationText => ConsoleColor.DarkGray.ToColor();
        public Color Critical => ConsoleColor.DarkCyan.ToColor();
        public Color Error => ConsoleColor.Red.ToColor();
        public Color Warning => ConsoleColor.DarkYellow.ToColor();
        public Color Information => ConsoleColor.White.ToColor();
        public Color Debug => ConsoleColor.DarkGray.ToColor();
        public Color Trace => ConsoleColor.DarkGreen.ToColor();
        public Color Strings => ConsoleColor.Magenta.ToColor();
        public Color Numbers => ConsoleColor.Magenta.ToColor();
        public Color Booleans => ConsoleColor.Magenta.ToColor();
    }
}