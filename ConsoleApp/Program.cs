using System;
using System.Threading;
using Somfic.Logging;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomConsole.HideCursor();

            var time = CustomConsole.Write(DateTime.Now, 1);

            CustomConsole.Write("my name jeff");

            while(true)
            {
                time.EraseAndUpdate(DateTime.Now.ToString());
            }
        }
    }
}