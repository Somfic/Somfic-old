namespace Somfic.Gui.Console
{
    public interface IConsoleGUI
    {
        void SetText(string text);
        void SetLayout(IConsoleLayout layout);
        void Draw();
    }
    
    class ConsoleGui : IConsoleGUI
    {
        private string _text;
        private IConsoleLayout _layout;

        public void SetText(string text)
        {
            _text = text;
        }

        public void SetLayout(IConsoleLayout layout)
        {
            _layout = layout;
        }

        public void Draw()
        {
            _layout.Setup(new LayoutContext());
            string text = _layout.Build(_text);

            System.Console.WriteLine(text);
        }
    }
}