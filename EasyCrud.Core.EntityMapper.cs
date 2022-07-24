using EasyCrudNET.Exceptions;
using EasyCrudNET.Mappers;
using EasyCrudNET.Resources;

namespace EasyCrudNET
{
    public partial class EasyCrud
    {
        private EntityMapper _entityMapper = new EntityMapper();

        public IEnumerable<T> MapResultTo<T>() where T : class, new()
        {
            try
            {
                if (_sqlDataReaderResponses.Count == 0 || _sqlDataReaderResponses == null)
                {
                    throw new EntityMappingException(string.Format(Messages.Get("CannotMapResultError")));
                }

                List<T> entities = new List<T>();

                _entityMapper.CollectMapperMetadata<T>();

                foreach (var readerResponse in _sqlDataReaderResponses)
                {
                    entities.Add(_entityMapper.ConvertSqlReaderResult<T>(readerResponse));
                }

                _sqlDataReaderResponses.Clear();

                return entities;
            }
            catch (Exception ex)
            {
                throw new EntityMappingException(string.Format(Messages.Get("MapError"), typeof(T).Name, ex.Message), ex);
            }
        }
        public IEnumerable<T> MapResultTo<T>(Func<List<(string FieldName, object FieldValue)>, T> map) where T : class, new()
        {
            try
            {
                if (_sqlDataReaderResponses.Count == 0 || _sqlDataReaderResponses == null)
                {
                    throw new EntityMappingException(string.Format(Messages.Get("CannotMapResultError")));
                }

                List<T> entities = new List<T>();

                foreach (var res in _sqlDataReaderResponses)
                {
                    entities.Add(map.Invoke(res));
                }

                _sqlDataReaderResponses.Clear();

                return entities;
            }
            catch (Exception ex)
            {
                throw new EntityMappingException(string.Format(Messages.Get("MapError"), typeof(T).Name, ex.Message), ex);
            }
        }
    }
}
