using System.Collections.Generic;
using System.Text;

namespace Somfic.Logging.Console.Themes
{
    /// <summary>
    /// A set of default console themes
    /// </summary>
    public static class ConsoleThemes
    {
        public static IConsoleTheme Vanilla => new VanillaConsoleTheme();

        public static IConsoleTheme Code => new CodeConsoleTheme();
    }
}
