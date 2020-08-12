using System;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using Somfic.Logging.Console.Themes;

namespace Somfic.Logging.Console
{
    public class ConsoleLoggerProvider : ILoggerProvider
    {
        private readonly IConsoleTheme _theme;


        public ConsoleLoggerProvider(IConsoleTheme theme)
        {
            _theme = theme;
        }

        public ConsoleLoggerProvider()
        {
            _theme = new VanillaConsoleTheme();
        }

        /// <summary>
        /// Disposes of the logger
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Creates a new ConsoleLogger
        /// </summary>
        /// <param name="categoryName">The category of these logs</param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            // Get the STD handle
            IntPtr iStdOut = GetStdHandle(StdOutputHandle);

            // Try to enable the use of ANSI codes
            bool colorSupported = GetConsoleMode(iStdOut, out uint outConsoleMode) &&
                                  SetConsoleMode(iStdOut, outConsoleMode | EnableVirtualTerminalProcessing);

            return new ConsoleLogger(categoryName, colorSupported, _theme);
        }

        private const int StdOutputHandle = -11;
        private const uint EnableVirtualTerminalProcessing = 0x0004;

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);
    }
}