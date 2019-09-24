using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Somfic.Logging;
using Somfic.Logging.LoadingBar;
using Somfic.Logging.Theme;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.UseConsole();
            Logger.UseFormat();
            Logger.UseSoftWrap();

            Logger.Log("this is one line");
            Logger.Log("this is two lines, can you believe it? i surely can't come up with more text");
            Logger.Warning("Oh no, we're about to hit an exception. I sure hope it's going to be something bad because I need it to take up space to test if this actually works.");
            Logger.Debug("0/0");
            int i = 1;
            try { int x = 0 / (1 - i); }
            catch(Exception ex) { Logger.Error("Let's see how 0/0 went ...", ex); }
            Logger.Log("So I guess this is somthing" + Themes.Dark);

        Thread.Sleep(-1);
        }
    }
}