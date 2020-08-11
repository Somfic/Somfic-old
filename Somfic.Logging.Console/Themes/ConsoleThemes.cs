using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Somfic.Logging.Console.Themes
{
    public static class ConsoleThemes
    {
        public static IConsoleTheme Vanilla => new VanillaConsoleTheme();

        public static IConsoleTheme VisualStudioCode => new VisualStudioCodeTheme();
    }

    public class VisualStudioCodeTheme : IConsoleTheme
    {
        public Color Background => Color.FromArgb(15, 17, 26);
        public Color Text => Color.FromArgb(143, 147, 162);
        public Color SecondaryText => Color.FromArgb(70, 75, 93);
        public Color PunctuationText => SecondaryText;
        public Color Critical => Color.FromArgb(199, 144, 234);
        public Color Error => Color.FromArgb(240, 113, 120);
        public Color Warning => Color.FromArgb(247, 140, 108);
        public Color Information => Color.FromArgb(137, 221, 255);
        public Color Debug => Text;
        public Color Trace => SecondaryText;
        public Color Strings => Color.FromArgb(195, 232, 141);
        public Color Numbers => Color.FromArgb(247, 140, 108);
        public Color Booleans => Numbers;
    }
}
