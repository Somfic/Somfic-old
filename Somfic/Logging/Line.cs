using System;

namespace Somfic.Logging
{
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

        public void Update(string value)
        {
            //Update the text.
            Text += value;

            //Go to the position.
            Console.SetCursorPosition(Left, UpStart);

            //Write the additional text.
            Console.Write(value);

            //Save the horizontal position in case we want to update it.
            Left = Console.CursorLeft;
        }

        public void EraseAndUpdate(string value)
        {
            //Erase the line.

            //Set the new left.
            Left = LeftStart;

            //If the new text is shorter than the old text, add spaces to clear the old text.
            if (value.Length < Text.Length)
            {
                value += new string(' ', Text.Length - value.Length);
            }

            //Clear the text.
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
