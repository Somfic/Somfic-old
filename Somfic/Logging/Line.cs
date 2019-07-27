using System;

namespace Somfic.Logging
{
    /// <summary>
    /// Class that holds methods to update lines in the console.
    /// </summary>
    public class Line
    {
        private readonly int OrgLeft = -1;
        private readonly int OrgUp = -1;

        internal Line(string s)
        {
            //Go to the original position if we've updated before.
            if (OrgLeft != -1 && OrgUp != -1)
            {
                Console.SetCursorPosition(OrgLeft, OrgUp);
            }

            //Set the text.
            Text = s;

            //Save the location of the line in this class.
            UpStart = Console.CursorTop;
            LeftStart = Console.CursorLeft;

            //Write to the console.
            Console.Write(s);

            //Save the horizontal position in case we want to update it.
            Left = Console.CursorLeft;

            //Go to the next line.
            Console.WriteLine();

            //Save this position.
            OrgLeft = Console.CursorLeft;
            OrgUp = Console.CursorTop;
        }

        /// <summary>
        /// Add a piece of text to the line.
        /// </summary>
        /// <param name="value">Value to be added to the line.</param>
        public void Update(object value)
        {
            //Turn the object into a string.
            value = CustomConsole.ToString(value);

            //Update the text.
            Text += value;

            //Go to the position.
            Console.SetCursorPosition(Left, UpStart);

            //Write the additional text.
            Console.Write(value);

            //Save the horizontal position in case we want to update it.
            Left = Console.CursorLeft;
        }

        /// <summary>
        /// Replace the line by a new string.
        /// </summary>
        /// <param name="value">Replacement string.</param>
        public void Replace(object value)
        {
            //Turn the object into a string.
            value = CustomConsole.ToString(value);
            int stringLenght = value.ToString().Length;

            //Set the new left.
            Left = LeftStart;

            //If the new text is shorter than the old text, add spaces to clear the old text.
            if (stringLenght < Text.Length)
            {
                value += new string(' ', Text.Length - stringLenght);
            }

            //Clear text.
            Text = string.Empty;

            //Update the line.
            Update(value);


        }


        public string Text { get; private set; }
        internal int UpStart { get; private set; }
        internal int LeftStart { get; private set; }
        internal int Left { get; private set; }
    }
}
