using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Somfic.Logging
{
    public static class CustomConsole
    {
        public static Line Write(object value)
        {
            string typeName = value.GetType().FullName;

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

            //Convert the object to a string and return.
            return new Line(value.ToString());
        }
    }
}
