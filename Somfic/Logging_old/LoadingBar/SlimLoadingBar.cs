using Somfic.Logging.Theme;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Somfic.Logging.LoadingBar
{
    public class SlimLoadingBar : ILoadingBar
    {
        public bool HasFinished { get; private set; }

        public int Current { get; private set; }

        public int Max { get;  set; }

        public int TextWidth { get; private set; }

        public string Item { get; private set; }

        public ITheme Theme { get; private set; }

        public Line barLine { get; private set; }
        private readonly char[] loadingChars = { '-', '/', '|', '\\' };
        private readonly int loopDelay = 200;
        private char loadingChar;
        private char frontChar = '░';
        private char backChar = '▒';

        public SlimLoadingBar(int max)
        {
            barLine = CustomConsole.Write("");

            TextWidth = 30;
            Item = "";
            Max = max;

            Task.Run(() =>
            {
                while (!HasFinished)
                {
                    foreach (char c in loadingChars)
                    {
                        loadingChar = c;
                        Refresh();
                        TextWidth = Console.WindowWidth * 1 / 4;
                        Thread.Sleep(loopDelay);
                    }
                }
            });

            Console.WriteLine();

            Update(0);
        }

        public void Done()
        {
            Done(string.Empty);
        }

        public void Done(string message)
        {
            HasFinished = true;
            Item = message;
            Refresh();
        }


        public void Next()
        {
            Update(Current++);
        }

        public void Next(string item)
        {
            Update(Current++, item);
        }

        public void Update(int current)
        {
            Update(current, Item);
        }

        public void Update(int current, string item)
        {
            Current = current;
            Item = item;
            Refresh();
        }

        private void Refresh()
        {
            //Get the percentage.
            decimal percentage = Math.Min(Current / (decimal)Max, 1);

            string value = Item;

            //If the Item is longer than MaxWidth, shorten it.
            if (value.Length > TextWidth)
            {
                value = value.Substring(0, TextWidth);
            }
            else
            {
                //Add the rest of the white-spaces.
                value += new string(' ', TextWidth - value.Length);
            }

            value += string.Format("{0,4}", $"{(int)Math.Ceiling(percentage * 100)}%");
            value += $" ({loadingChar}) ";

            int charsLeft = Console.WindowWidth - value.Length - 2;
            int doneChars = charsLeft - (int)Math.Ceiling(charsLeft * percentage);

            value += $"&a{new string(backChar, charsLeft - doneChars)}&8";
            value += new string(frontChar, doneChars);

            barLine.Replace(value);
        }

        public void UseTheme(ITheme theme)
        {
            Theme = theme;
        }
    }
}
