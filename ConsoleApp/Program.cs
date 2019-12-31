using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Somfic.Logging;
using Somfic.Logging.Handlers;

namespace ConsoleApp
{
    class Program
    {
        static Logger log;

        static void Main(string[] args)
        {
            log = new Logger();
            log.AddHandler(new ConsoleHandler());
            log.AddHandler(new LogFileHandler("C:\\Users\\Lucas\\Documents\\Somfic", "EliteAPI"));

            log.Log("Hello this is a test on whether or not this stupid console actually does what i want it to do", new Test());
            log.Log(Severity.Warning, "We're about to trigger an exception!");
            log.Log(Severity.Error, new IndexOutOfRangeException(":(", new FileNotFoundException("Issa null bro", "C:\\path.txt")));

            try
            {
                File.Open("X:\\A.txt", FileMode.Append);

            }
            catch (Exception ex)
            {
                log.Log(ex);
            }
            
            Console.ReadLine();
        }
    }

    public class Test
    {
        public string Name { get; set; } = "Jack";
        public string LastName { get; set; } = "Johnson";
    }
}