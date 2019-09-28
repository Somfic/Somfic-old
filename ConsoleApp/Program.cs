using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
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
        static Logger log;

        static void Main(string[] args)
        {
            log = new Logger().UseConsole().UseTCP(1234);

            while(true)
            {
                Thread.Sleep(1000);
                log.Log(DateTime.Now);
            }
        }
    }
}