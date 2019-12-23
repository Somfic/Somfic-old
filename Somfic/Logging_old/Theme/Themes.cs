using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Somfic.Logging.Theme
{
    public static class Themes
    {
        public static ITheme Dark { get => new ThemeDark(); }
        public static ITheme Light { get => new ThemeLight(); }
    }

    internal class ThemeDark : ITheme
    {
        public ConsoleColor BackGround => ConsoleColor.Black;
        public ConsoleColor Main => ConsoleColor.White;
        public ConsoleColor Warning => ConsoleColor.Yellow;
        public ConsoleColor Error => ConsoleColor.Red;
        public ConsoleColor Special => ConsoleColor.Cyan;
        public ConsoleColor Debug => ConsoleColor.DarkGray;
        public ConsoleColor Success => ConsoleColor.Green;
    }

    internal class ThemeLight : ITheme
    {
        public ConsoleColor BackGround => ConsoleColor.White;
        public ConsoleColor Main => ConsoleColor.Black;
        public ConsoleColor Warning => ConsoleColor.DarkYellow;
        public ConsoleColor Error => ConsoleColor.Red;
        public ConsoleColor Special => ConsoleColor.Cyan;
        public ConsoleColor Debug => ConsoleColor.DarkGray;
        public ConsoleColor Success => ConsoleColor.DarkGreen;
    }
}
