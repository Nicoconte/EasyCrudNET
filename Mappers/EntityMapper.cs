using EasyCrudNET.Exceptions;
using EasyCrudNET.Mappers.Attributes;
using EasyCrudNET.Resources;
using FastMember;

namespace EasyCrudNET.Mappers
{
    public class EntityMapper
    {
        private List<MetadataMapper> _mappingMetadata = new List<MetadataMapper>();

        public void CollectMapperMetadata<T>()
        {
            Type type = typeof(T);

            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(false);

                var fieldMapping = attributes.FirstOrDefault(a => a.GetType() == typeof(Field));

                if (fieldMapping != null)
                {
                    var mapsto = (Field)fieldMapping;

                    _mappingMetadata.Add(new MetadataMapper
                    {
                        EntityType = type,
                        PropertyName = property.Name,
                        FieldName = mapsto.FieldName
                    });
                }
            }
        }

        public T ConvertSqlReaderResult<T>(List<(string FieldName, object FieldValue)> pairs) where T : class, new()
        {
            var t = new T();
            var accessor = TypeAccessor.Create(t.GetType());
            var members = accessor.GetMembers();

            //Check if we have to use mapping data
            bool mapperHasValues = _mappingMetadata != null || _mappingMetadata?.Count != 0;

            foreach(var (FieldName, FieldValue)  in pairs)
            {
                //Check if any prop match with field name
                bool hasMember = members.Any(m => string.Equals(m.Name, FieldName));

                //If we dont have any mapping data but we have a member, we use the fieldname to set prop value
                if (!mapperHasValues && hasMember || hasMember)
                {
                    accessor[t, FieldName] = FieldValue;
                    continue;
                }

                //We map based on mapperMetadata
                MetadataMapper data = _mappingMetadata?.FirstOrDefault(c => c.FieldName.ToLower() == FieldName.ToLower());

                if (data == null)
                    throw new EntityMappingException(string.Format(Messages.Get("EntityMappingError"), FieldName));

                accessor[t, data.PropertyName] = FieldValue;
            }

            //Clear metadata after use
            //TODO: Storage in cache so we avoid collect attributes again 
            _mappingMetadata.Clear();  

            return t;
        }
    }

    public class MetadataMapper
    {
        public Type EntityType { get; set; }
        public string PropertyName { get; set; }
        public string FieldName { get; set; }
    }
}
