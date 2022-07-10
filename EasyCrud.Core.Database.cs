using EasyCrudNET.Exceptions;
using EasyCrudNET.Extensions;
using EasyCrudNET.Interfaces;
using EasyCrudNET.Interfaces.Core;
using EasyCrudNET.Mappers;
using EasyCrudNET.Resources;
using FastMember;
using System.Data.SqlClient;
using System.Text;

namespace EasyCrudNET
{
    public partial class EasyCrud
    {
        /// <summary>
        /// Create a new Sql connection.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <exception cref="SqlConnectionException"></exception>
        public void SetSqlConnection(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new SqlConnectionException(Messages.Get("EmptyConnectionStringError"));
            }

            _sqlConnection = new SqlConnection(connectionString);
        }

        private void _SqlOperationWrapper(Action<SqlCommand> action)
        {
            string queryBuilt = _query.ToString();

            if (string.IsNullOrWhiteSpace(queryBuilt))
            {
                throw new DatabaseExecuteException(Messages.Get("EmptyQueryError"));
            }

            SqlCommand cmd = new SqlCommand(queryBuilt, _sqlConnection);
    
            if (_queryValues.Count > 0)
            {
                cmd.MapSqlParameters(queryBuilt, _queryValues);
                _queryValues.Clear();
            }

            try
            {
                _sqlConnection.Open();

                action(cmd);
            }
            catch (Exception ex)
            {
                throw new DatabaseExecuteException(string.Concat(Messages.Get("DatabaseError"), "\n=> ", ex.Message, " => ", ex.StackTrace), ex.InnerException);
            }
            finally
            {
                _query = new StringBuilder();
             
                _sqlConnection.Close();
            }
        }

        public IDatabase BindValues(object values)
        {
            foreach(var value in values.GetType().GetProperties())
            {
                var propName = value.Name;
                var propValue = values.GetType().GetProperty(propName).GetValue(values);

                _queryValues.Add((propName, propValue));
            }

            return this;
        }

        public IDatabase ExecuteRawQuery(string query)
        {
            if (_query.Length > 0)
            {
                throw new SqlBuilderException(Messages.Get("QueryAlreadyBuiltError"));
            }

            _query = new StringBuilder(query);

            _SqlOperationWrapper(cmd =>
            {
                var rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    var resCollector = new List<(string, object)>();

                    for (int i = 0; i < rd.FieldCount; i++)
                    {
                        if (!rd.IsDBNull(i))
                        {
                            resCollector.Add((rd.GetName(i), rd.GetValue(i)));
                        }
                    }

                    _sqlDataReaderResponses.Add(resCollector);
                }

                rd.Close();

            });

            return this;
        }

        public IDatabase ExecuteQuery()
        {
            _SqlOperationWrapper(cmd =>
            {
                var rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    var resCollector = new List<(string, object)>();

                    for (int i = 0; i < rd.FieldCount; i++)
                    {
                        if (!rd.IsDBNull(i))
                        {
                            resCollector.Add((rd.GetName(i), rd.GetValue(i)));
                        }
                    }

                    _sqlDataReaderResponses.Add(resCollector);
                }

                rd.Close();
            });

            return this;
        }

        public int SaveChangesRawQuery(string query)
        {

            if (_query.Length > 0)
            {
                throw new SqlBuilderException(Messages.Get("QueryAlreadyBuiltError"));
            }

            if (query.ToLower().Contains("select"))
            {
                throw new DatabaseExecuteException(Messages.Get("CannotExecuteOpError"));
            }

            _query = new StringBuilder(query);

            int rows = 0;

            _SqlOperationWrapper(cmd =>
            {
                rows = cmd.ExecuteNonQuery();
            });

            return rows;
        }

        public int SaveChanges()
        {
            if (_query.ToString().ToLower().Contains("select"))
            {
                throw new DatabaseExecuteException(Messages.Get("CannotExecuteOpError"));
            }

            int rows = 0;

            _SqlOperationWrapper(cmd =>
            {
                rows = cmd.ExecuteNonQuery();
            });

            return rows;
        }

        public List<List<(string FieldName, object FieldValue)>> GetResult()
        {
            var responseCopy = new List<List<(string, object)>>();

            responseCopy.AddRange(_sqlDataReaderResponses);

            _sqlDataReaderResponses.Clear();
                
            return responseCopy;
        }

        public IEnumerable<T> MapResultTo<T>() where T : class, new()
        {
            try
            {
                if (_sqlDataReaderResponses.Count  <= 0)
                {
                    throw new EntityMappingException(string.Format(Messages.Get("CannotMapResultError")));
                }

                List<T> entities = new List<T>();

                _classMapper.CollectMapperMetadata<T>();

                foreach (var readerResponse in _sqlDataReaderResponses)
                {
                    entities.Add(_classMapper.ConvertSqlReaderResult<T>(readerResponse));
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
                List<T> entities = new List<T>();

                foreach (var res in _sqlDataReaderResponses)
                {
                    entities.Add(map.Invoke(res));
                }

                _sqlDataReaderResponses.Clear();

                return entities;
            }
            catch(Exception ex)
            {
                throw new EntityMappingException(string.Format(Messages.Get("MapError"), typeof(T).Name, ex.Message), ex);
            }
        }
    }
}
