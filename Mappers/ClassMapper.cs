using EasyCrudNET.Mappers.DataAnnotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET.Mappers
{
    public class ClassMapper
    {
        public List<MapperMetadata> MappingMetadata = new List<MapperMetadata>();

        public List<MapperMetadata> GetMappingMetadataByType<T>()
        {
            try
            {
                return MappingMetadata.FindAll(c => c.EntityType == typeof(T)).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public bool IsMappable<T>()
        {  
            return MappingMetadata.Any(a => a.EntityType == typeof(T));
        }

        public void CollectAttributes<T>() where T : class, new()
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
                    MappingMetadata.Add(new MapperMetadata
                    {
                        EntityType = t.GetType(),
                        PropertyName = property.Name,
                        ColumnName = mapsto.ColumnName,
                        ColumnAction = mapsto.ColumnType
                    });
                }
            }
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
