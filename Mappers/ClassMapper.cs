using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET.Mappers
{
    public class ClassMapper
    {
        public Type Type { get; set; }

        /// <summary>
        /// First goes the table column (field), then the property name
        /// </summary>
        public List<(string, string)> ColumnMappers { get; set; } = new List<(string, string)>();

        public (string, string) GetColumnByFieldName(string fieldName)
        {
            try
            {
                return ColumnMappers.Find(c => c.Item1.ToLower() == fieldName.ToLower());
            }
            catch (Exception ex)
            {
                return (null, null);
            }
        }
    }
}
