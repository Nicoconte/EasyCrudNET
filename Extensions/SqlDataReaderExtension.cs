using EasyCrudNET.Mappers;
using EasyCrudNET.Mappers.DataAnnotation;
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
        public static T ConvertToObject<T>(this SqlDataReader rd, List<MapperMetadata> mapperMetadata=null) where T : class, new()
        { 
            try
            {
                Type type = typeof(T);
                var accessor = TypeAccessor.Create(type);
                var members = accessor.GetMembers();
                var t = new T();

                bool mapperHasValues = mapperMetadata != null || mapperMetadata?.Count != 0;

                for (int i = 0; i < rd.FieldCount; i++)
                {
                    if (!rd.IsDBNull(i))
                    {
                        string fieldName = rd.GetName(i);

                        bool hasMember = members.Any(m => string.Equals(m.Name, fieldName));

                        if (!mapperHasValues && hasMember)
                        {
                            accessor[t, fieldName] = rd.GetValue(i);
                            continue;
                        }


                        MapperMetadata data = null;

                        if (hasMember)
                        {
                            data = mapperMetadata?.FirstOrDefault(c => _ShouldIgnoreMapping(c.ColumnAction));

                            accessor[t, fieldName] = rd.GetValue(i);

                            if (data == null)
                            {
                                mapperMetadata.RemoveAll(c => c.ColumnName.ToLower() == fieldName.ToLower());
                            }
                            else
                            {
                                mapperMetadata?.Remove(data);
                            }

                            continue;
                        }

                        data = mapperMetadata?.FirstOrDefault(c => c.ColumnName.ToLower() == fieldName.ToLower());

                        accessor[t, data.PropertyName] = rd.GetValue(i);

                        mapperMetadata?.Remove(data);
                    }
                }

                return t;
            }
            catch(Exception ex)
            {
                throw new Exception($"Error during convertion. Reason: {ex.Message}");
            }
        }

        private static bool _ShouldIgnoreMapping(ColumnAction action)
        {
            return action == ColumnAction.Ignore;
        }
    }
}
