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
            return values
                .GetType()
                .GetProperties()
                .Select(c => (c.Name, values.GetType().GetProperty(c.Name).GetValue(values)))
                .ToList();
        }
    }
}
