using System;
using System.Collections.Generic;
using System.Text;

namespace Somfic.Logging.Console.Builder.Ansi
{
    public static class AnsiDefaults
    {
        public const string Format = "\u001b[{0}m";
        public const string ColorFormat = "{0};2;{1};{2};{3}";
    }
}
