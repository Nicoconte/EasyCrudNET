using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET.Mappers.DataAnnotation
{
    public class ColumnMapper : Attribute
    {
        public string ColumnName { get; set; }
        public ColumnAction ColumnType { get; set; }

        public ColumnMapper(string name, ColumnAction columnType=ColumnAction.Map)
        {
            ColumnName = name;
            ColumnType = columnType;
        }

        public ColumnMapper(ColumnAction columnType = ColumnAction.Map)
        {
            ColumnType = columnType;
        }

        public ColumnMapper(string name)
        {
            ColumnName = name;
            ColumnType = ColumnAction.Map;
        }
    }

    public enum ColumnAction
    {
        Ignore,
        Map
    }
}
