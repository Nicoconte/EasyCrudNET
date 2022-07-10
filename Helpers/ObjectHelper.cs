using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET.Helpers
{
    public class ObjectHelper
    {
        /// <summary>
        /// Destructure object into a List of tuples. First tuple value is the name of the object property
        /// Second one is the value of that object property
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static List<(string PropName, object PropValue)> DestructureObject(object values)
        {

            List<(string PropName, object PropValue)> objectInfoContainer = new List<(string PropName,object PropValue)>();

            foreach (var value in values.GetType().GetProperties())
            {
                var propName = value.Name;
                var propValue = values.GetType().GetProperty(propName).GetValue(values);

                objectInfoContainer.Add((propName, propValue));
            }

            return objectInfoContainer;
        }
    }
}
