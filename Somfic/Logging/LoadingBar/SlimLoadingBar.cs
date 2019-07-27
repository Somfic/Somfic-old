using Somfic.Logging.Themes;
using System;

namespace Somfic.Logging.LoadingBar
{
    public class SlimLoadingBar : ILoadingBar
    {
        public int Current { get; private set; }

        public int Max { get; private set; }

        public string Item { get; private set; }

        private readonly Line barLine;

        public SlimLoadingBar(string text)
        {
            barLine = CustomConsole.Write("");

        }

        public void Done()
        {
            Done(string.Empty);
        }

        public void Done(string message)
        {
            throw new NotImplementedException();
        }

        public void Update(int current)
        {
            Update(current, Item);
        }

        public void Update(int current, string item)
        {
            throw new NotImplementedException();
        }

        public void UseTheme(ITheme theme)
        {
            throw new NotImplementedException();
        }
    }
}
