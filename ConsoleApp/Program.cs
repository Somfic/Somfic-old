using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Somfic.Logging;
using Somfic.Logging.Handlers;
using Somfic.Version;
using Somfic.Version.Github;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleHandler console = new ConsoleHandler();
            LogFileHandler logFile = new LogFileHandler(Directory.GetCurrentDirectory(), "log");


            Logger.AddHandler(new ConsoleHandler());
            Logger.AddHandler(new LogFileHandler(Directory.GetCurrentDirectory(), "EliteAPI"));

            VersionController versionController = new GithubVersionControl("EliteAPI", "EliteAPI");
            if (versionController.HasNewer)
            {
                Logger.Log("A new version is available.");
            }


            try
            {
                File.Open("X:\\A.txt", FileMode.Append);

            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
            
            
            Console.ReadLine();
        }
    }
}