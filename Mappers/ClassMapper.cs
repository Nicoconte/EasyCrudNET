using EasyCrudNET.Exceptions;
using EasyCrudNET.Mappers.DataAnnotation;
using EasyCrudNET.Resources;
using FastMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET.Mappers
{
    public class ClassMapper
    {
        private List<MapperMetadata> _mapperMetadata = new List<MapperMetadata>();

        public void CollectMapperMetadata<T>() where T : class, new()
        {
            T t = new T();

            var properties = t.GetType().GetProperties();

            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(false);

                var columnMapping = attributes.FirstOrDefault(a => a.GetType() == typeof(ColumnMapper));

                if (columnMapping != null)
                {
                    var mapsto = (ColumnMapper)columnMapping;
                    _mapperMetadata.Add(new MapperMetadata
                    {
                        EntityType = t.GetType(),
                        PropertyName = property.Name,
                        ColumnName = mapsto.ColumnName,
                        ColumnAction = mapsto.ColumnType
                    });
                }
            }
        }

        public T ConvertSqlReaderResult<T>(List<(string ColumnName, object ColumnValue)> pairs) where T : class, new()
        {
            Type type = typeof(T);
            var accessor = TypeAccessor.Create(type);
            var members = accessor.GetMembers();
            var t = new T();

            //Check if we have to use mapping data
            bool mapperHasValues = _mapperMetadata != null || _mapperMetadata?.Count != 0;

            foreach(var pair in pairs)
            {

                string fieldName = pair.ColumnName;
                object fieldValue = pair.ColumnValue;

                //Check if any prop match with field name
                bool hasMember = members.Any(m => string.Equals(m.Name, fieldName));

                //If we dont have any mapping data but we have a member, we use the fieldname to set prop value
                if (!mapperHasValues && hasMember || hasMember)
                {
                    accessor[t, fieldName] = fieldValue;
                    continue;
                }

                //We map based on mapperMetadata
                MapperMetadata data = _mapperMetadata?.FirstOrDefault(c => c.ColumnName.ToLower() == fieldName.ToLower());

                if (data == null)
                    throw new EntityMappingException(string.Format(Messages.Get("EntityMappingError"), fieldName));

                accessor[t, data.PropertyName] = fieldValue;
            }

            return t;
        }

        private static bool _ShouldIgnoreMapping(ColumnAction action)
        {
            return action == ColumnAction.Ignore;
        }
    }

    public class MapperMetadata
    {
        public Type EntityType { get; set; }
        public string PropertyName { get; set; }
        public string ColumnName { get; set; }
        public ColumnAction ColumnAction { get; set; }
    }
}
