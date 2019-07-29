using System;
using System.IO;
using System.Threading;
using Somfic.Logging;
using Somfic.Logging.LoadingBar;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(1000);


            var x = new DirectoryInfo(@"D:\Projects").GetFiles("*", SearchOption.AllDirectories);
                        SlimLoadingBar bar = new SlimLoadingBar(x.Length);

            int i = 0;
            foreach (var item in x)
            {
                i++;
                bar.Update(i, item.Name);
                Thread.Sleep(100);
            }

            Thread.Sleep(-1);
        }
    }
}