using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Somfic.Logging;
using Somfic.Logging.LoadingBar;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Start the icon async.
            Task ShowIcon = Task.Run(() => Logger.ShowIcon());

            //Check if there's a new version.


            Thread.Sleep(-1);
        }
    }
}