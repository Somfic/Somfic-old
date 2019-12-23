using Newtonsoft.Json;
using System;

namespace Somfic.Logging
{
    public static class CustomConsole
    {
        public static void HideCursor(bool value = true)
        {
            Console.CursorVisible = !value;
        }

        public static Line Write(object value)
        {
            return Write(value, Console.CursorLeft, Console.CursorTop);
        }

        public static Line Write(object value, int left, int up)
        {
            //Set offset.
            Console.CursorLeft = left;
            Console.CursorTop = up;

            //Convert the object to a string and get the line object.
            Line line = new Line(ToString(value));

            //Return the line.
            return line;
        }

        internal static string ToString(object value)
        {
            //Get the type name.
            string typeName = value.GetType().Namespace;

            //Check whether the object is a custom class.
            if (!typeName.StartsWith("System"))
            {
                //The object is a custom type.
                //Try converting the object to JSON.
                try
                {
                    string json = JsonConvert.SerializeObject(value);

                    //Set the new JSON line to the value variable.
                    value = json;
                }
                catch { }
            }

            return value.ToString();
        }
    }
}
