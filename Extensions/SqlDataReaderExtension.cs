using EasyCrudNET.Mappers;
using FastMember;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET.Extensions
{
    public static class SqlDataReaderExtension
    {
        public static T ConvertToObject<T>(this SqlDataReader rd, ClassMapper classMapper=null) where T : class, new()
        {
            Type type = typeof(T);
            var accessor = TypeAccessor.Create(type);
            var members = accessor.GetMembers();
            var t = new T();

            for (int i = 0; i < rd.FieldCount; i++)
            {
                if (!rd.IsDBNull(i))
                {
                    string fieldName = rd.GetName(i);

                    if (classMapper != null)
                    {
                        var (col, prop) = classMapper.GetColumnByFieldName(fieldName);

                        if (prop == null || col == null) throw new ArgumentNullException($"Invalid args. Field not found. Field {fieldName}");

                        accessor[t, prop] = rd.GetValue(i);
                    }
                    else
                    {
                        if (members.Any(m => string.Equals(m.Name, fieldName, StringComparison.OrdinalIgnoreCase)))
                        {
                            accessor[t, fieldName] = rd.GetValue(i);
                        }
                    }
                }
            }

            return t;
        }
    }
}
