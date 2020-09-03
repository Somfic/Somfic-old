using System;
using System.Text;

namespace Somfic.Gui.Console.Layout
{
    public static class ConsoleLayouts
    {
        public static IConsoleLayout Center => new CenterLayout();
    }

    /// <inheritdoc />
    public class CenterLayout : IConsoleLayout
    {
        private LayoutContext _context;

        /// <inheritdoc />
        public void Setup(LayoutContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public string Build(string content)
        {
            StringBuilder sb = new StringBuilder();
            string[] lines = content.Split(new[] {Environment.NewLine}, StringSplitOptions.None);

            foreach (string line in lines)
            {
                int spaces = _context.Width - line.Length;
                int spaceLeft = spaces / 2;
                int spaceRight = spaces - spaceLeft;

                sb.AppendLine(line.PadLeft(spaceLeft).PadRight(spaceRight));
            }

            return sb.ToString();
        }
    }
}
