using EasyCrudNET.Exceptions;
using EasyCrudNET.Mappers;
using EasyCrudNET.Mappers.DataAnnotation;
using EasyCrudNET.Resources;
using FastMember;
using System.Data.SqlClient;

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

                //Check if we have to use mapping data
                bool mapperHasValues = mapperMetadata != null || mapperMetadata?.Count != 0;

                for (int i = 0; i < rd.FieldCount; i++)
                {
                    if (!rd.IsDBNull(i))
                    {
                        string fieldName = rd.GetName(i);

                        //Check if any prop match with field name
                        bool hasMember = members.Any(m => string.Equals(m.Name, fieldName));

                        //If we dont have any mapping data but we have a member, we use the fieldname to set prop value
                        if (!mapperHasValues && hasMember)
                        {
                            accessor[t, fieldName] = rd.GetValue(i);
                            continue;
                        }

                        MapperMetadata data = null;

                        //this happen when we have mapping data but the property should be ignore. Alse use the fieldanem to set prop value
                        if (hasMember)
                        {
                            accessor[t, fieldName] = rd.GetValue(i);

                            data = mapperMetadata?.FirstOrDefault(c => _ShouldIgnoreMapping(c.ColumnAction));

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

                        //We map base on mapperMetadata
                        data = mapperMetadata?.FirstOrDefault(c => c.ColumnName.ToLower() == fieldName.ToLower());

                        if (data == null)
                            throw new EntityMappingException(string.Format(Messages.Get("EntityMappingError"), fieldName));

                        accessor[t, data.PropertyName] = rd.GetValue(i);

                        mapperMetadata?.Remove(data);
                    }
                }

                return t;
            }
            catch(Exception ex)
            {
                throw new EntityMappingException(string.Format(Messages.Get("BaseErrorWithMsg"), ex.Message), ex);
            }
        }

        private static bool _ShouldIgnoreMapping(ColumnAction action)
        {
            return action == ColumnAction.Ignore;
        }
    }
}
