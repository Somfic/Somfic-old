using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Somfic.Logging
{
    public static class Logger
    {
        public static void ShowIcon()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

            ShowIcon(@"C:\Users\Lucas\Documents\Somfic\Icon");
        }

        private static void ShowIcon(string path)
        {
            //Find all files.
            if (!Directory.Exists(path)) { Console.WriteLine("DIRECTORY DOESN'T EXIST."); }

            FileInfo[] files = new DirectoryInfo(path).GetFiles();

            //Go through each file, and find out what the size of the console has to be.
            int maxX = 0;
            int maxY = 0;
            foreach (var file in files)
            {
                IEnumerable<string> lines = File.ReadAllLines(file.FullName).Skip(1);
                if(lines.Count() > maxY) { maxY = lines.Count(); }

                foreach (string line in lines)
                {
                    if (line.Length > maxX) { maxX = line.Length; }
                }
            }

            //Set the size of the console.
            Console.WindowWidth = maxX;
            Console.WindowHeight = maxY;

            //Draw each part.
            Console.CursorVisible = false;
            foreach (var file in files)
            {
                //Read the first character of the file.
                string colorChar = File.ReadAllLines(file.FullName)[0];

                //If it's a number, set it as ForegroundColor, otherwise, set the ForegroundColor to White.
                if (!int.TryParse(colorChar, out int colorInt)) { Console.ForegroundColor = ConsoleColor.White; }
                else { Console.ForegroundColor = (ConsoleColor)colorInt; }

                int y = 0;
                foreach (string line in File.ReadAllLines(file.FullName).Skip(1))
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        int x = 0;
                        foreach (char c in line)
                        {
                            if (c != ' ')
                            {
                                Console.SetCursorPosition(x, y);
                                Console.Write(c);
                                Thread.Sleep(1);
                            }
                            x++;
                        }
                    }
                    y++;
                }
            }
        }
    }
}
