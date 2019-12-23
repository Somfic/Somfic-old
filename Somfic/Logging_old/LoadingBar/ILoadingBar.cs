using Somfic.Logging.Theme;

namespace Somfic.Logging.LoadingBar
{
    /// <summary>
    /// The interface for loading bars.
    /// </summary>
    public interface ILoadingBar
    {
        /// <summary>
        /// Whether the task behind the loading bar has been completed.
        /// </summary>
        bool HasFinished { get; }

        /// <summary>
        /// The current task, or step, the loading bar is in.
        /// </summary>
        int Current { get; }

        /// <summary>
        /// The maximum amount of characters for the Item.
        /// </summary>
        int TextWidth { get; }

        /// <summary>
        /// The maximum amount of tasks, or steps, the loading bar has.
        /// </summary>
        int Max { get; set; }

        /// <summary>
        /// The current item that's being processed.
        /// </summary>
        string Item { get; }

        /// <summary>
        /// Updates the loading bar with new information.
        /// </summary>
        /// <param name="current">The current step.</param>
        /// <param name="item">The additional message.</param>
        void Update(int current, string item);

        /// <summary>
        /// Updates the loading bar with new information.
        /// </summary>
        /// <param name="current">The current step.</param>
        void Update(int current);

        /// <summary>
        /// Marks the loading bar as completed.
        /// </summary>
        /// <param name="message">The additional message.</param>
        void Done(string message);

        /// <summary>
        /// Marks the loading bar as completed.
        /// </summary>
        void Done();

        /// <summary>
        /// Applies a custom theme to the loading bar.
        /// </summary>
        /// <param name="theme">The theme to be applied.</param>
        void UseTheme(ITheme theme);
    }
}
