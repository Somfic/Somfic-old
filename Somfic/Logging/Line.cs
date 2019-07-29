using System;
using System.Text.RegularExpressions;

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
            Write(s);

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
            var OrgLeft = Console.CursorLeft;
            var OrgUp = Console.CursorTop;

            //Turn the object into a string.
            value = CustomConsole.ToString(value);

            //Update the text.
            Text += value;

            //Go to the position.
            Console.SetCursorPosition(Left, UpStart);

            Write(value.ToString());

            //Save the horizontal position in case we want to update it.
            Console.ForegroundColor = ConsoleColor.White;
            Left = Console.CursorLeft;

            //Go back.
            Console.SetCursorPosition(OrgLeft, OrgUp);
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

        private void Write(string value)
        {
            if (Regex.Matches(value.ToString(), "&([0-9,a-f])([^&]*)").Count == 0)
            {
                Console.Write(value);
            }
            else
            {

                if (!value.StartsWith("&"))
                {
                    Console.Write(value.Split('&')[0]);
                }

                //Write the additional text.
                foreach (Match match in Regex.Matches(value, "&([0-9,a-f])([^&]*)"))
                {
                    if (match.Groups[1].Value.ToLower()[0] == '0') { Console.ForegroundColor = ConsoleColor.Black; }
                    else if (match.Groups[1].Value.ToLower()[0] == '1') { Console.ForegroundColor = ConsoleColor.DarkBlue; }
                    else if (match.Groups[1].Value.ToLower()[0] == '2') { Console.ForegroundColor = ConsoleColor.Green; }
                    else if (match.Groups[1].Value.ToLower()[0] == '3') { Console.ForegroundColor = ConsoleColor.DarkCyan; }
                    else if (match.Groups[1].Value.ToLower()[0] == '4') { Console.ForegroundColor = ConsoleColor.DarkRed; }
                    else if (match.Groups[1].Value.ToLower()[0] == '5') { Console.ForegroundColor = ConsoleColor.DarkMagenta; }
                    else if (match.Groups[1].Value.ToLower()[0] == '6') { Console.ForegroundColor = ConsoleColor.DarkYellow; }
                    else if (match.Groups[1].Value.ToLower()[0] == '7') { Console.ForegroundColor = ConsoleColor.Gray; }
                    else if (match.Groups[1].Value.ToLower()[0] == '8') { Console.ForegroundColor = ConsoleColor.DarkGray; }
                    else if (match.Groups[1].Value.ToLower()[0] == '9') { Console.ForegroundColor = ConsoleColor.Blue; }
                    else if (match.Groups[1].Value.ToLower()[0] == 'a') { Console.ForegroundColor = ConsoleColor.Green; }
                    else if (match.Groups[1].Value.ToLower()[0] == 'b') { Console.ForegroundColor = ConsoleColor.Cyan; }
                    else if (match.Groups[1].Value.ToLower()[0] == 'c') { Console.ForegroundColor = ConsoleColor.Red; }
                    else if (match.Groups[1].Value.ToLower()[0] == 'd') { Console.ForegroundColor = ConsoleColor.Magenta; }
                    else if (match.Groups[1].Value.ToLower()[0] == 'e') { Console.ForegroundColor = ConsoleColor.Yellow; }
                    else if (match.Groups[1].Value.ToLower()[0] == 'f') { Console.ForegroundColor = ConsoleColor.White; }

                    string sentence = match.Groups[2].Value;


                    Console.Write(sentence);
                }
            }
        }


        public string Text { get; private set; }
        internal int UpStart { get; private set; }
        internal int LeftStart { get; private set; }
        internal int Left { get; private set; }
    }
}
