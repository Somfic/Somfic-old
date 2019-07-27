using System;
using System.Threading;
using Somfic.Logging;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = CustomConsole.Write("e");
            CustomConsole.Write("haha").Update("h");
            CustomConsole.Write("hok");
            s.Update("s");
        }
    }
}