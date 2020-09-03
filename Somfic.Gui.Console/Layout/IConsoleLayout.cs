namespace Somfic.Gui.Console
{
    public interface IConsoleLayout
    {
        void Setup(LayoutContext context);

        string Build(string content);
    }
}