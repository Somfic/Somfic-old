using Somfic.Logging.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Somfic.Logging.LoadingBar
{
    /// <summary>
    /// The interface for loading bars.
    /// </summary>
    public interface ILoadingBar
    {
        /// <summary>
        /// The current task, or step, the loading bar is in.
        /// </summary>
        int Current { get; }

        /// <summary>
        /// The maximum amount of tasks, or steps, the loading bar has.
        /// </summary>
        int Max { get; }

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
