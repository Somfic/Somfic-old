using Microsoft.Extensions.Logging;
using Somfic.Logging.Console.Themes;

namespace Somfic.Logging.Console
{
    public static class PrettyConsoleLoggerExtensions
    {
        public static ILoggingBuilder AddPrettyConsole(this ILoggingBuilder builder, IConsoleTheme theme = null)
        {
            if(theme == null) { theme = ConsoleThemes.Code; }

            builder.AddProvider(new PrettyConsoleLoggerProvider(theme));

            return builder;
        }
    }
}