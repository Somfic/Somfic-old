using System.Drawing;

namespace Somfic.Logging.Themes
{
    public interface ITheme
    {
        ColorScheme Main { get; }

        ColorScheme Warning { get; }

        ColorScheme Error { get; }

        ColorScheme Special { get; }
    }

    public class ColorScheme
    {
        public ColorScheme(Color foreground, Color background)
        {
            Foreground = foreground;
            Background = background;
        }

        public Color Foreground { get; set; }

        public Color Background { get; set; }
    }
}
