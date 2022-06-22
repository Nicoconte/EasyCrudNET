using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET.Mappers
{
    public class Mapper
    {
        private List<ClassMapper> _classMapping = new List<ClassMapper>();

        /// <summary>
        /// Set the mapper for the Type "T". 
        /// Firt string is the table column name, second one is the class property name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableMappers"></param>
        public void SetMap<T>(List<(string, string)> tableMappers)
        {
            var classMapper = new ClassMapper();

            classMapper.Type = typeof(T);
            classMapper.ColumnMappers.AddRange(tableMappers);

            _classMapping.Add(classMapper);
        }

        public ClassMapper GetClassMapperByType<T>()
        {
            try
            {
                return _classMapping?.Find(c => c?.Type == typeof(T));
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
