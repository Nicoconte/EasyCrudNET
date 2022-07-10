using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET.Mappers.Attributes
{
    public class Field : Attribute
    {
        public string FieldName { get; set; }

        public Field(string name)
        {
            FieldName = name;
        }
    }
}
