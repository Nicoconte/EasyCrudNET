using EasyCrudNET.Exceptions;
using EasyCrudNET.Extensions;
using EasyCrudNET.Mappers;
using EasyCrudNET.Resources;
using System.Data.SqlClient;
using System.Text;

namespace EasyCrudNET
{
    public partial class EasyCrud
    {

        public void SetSqlConnection(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new SqlConnectionException(Messages.Get("EmptyConnectionStringError"));
            }

            _sqlConnection = new SqlConnection(connectionString);
        }

        public IEnumerable<T> Execute<T>(object values = null, string query = "") where T : class, new()
        {
            string sqlQuery = string.Empty.GetSqlQuery(query, _query.ToString());

            SqlCommand cmd = new SqlCommand(sqlQuery, _sqlConnection);

            if (values != null)
                cmd.MapSqlParameters(sqlQuery, values);

            var entities = new List<T>();

            try
            {
                _sqlConnection.Open();

                SqlDataReader rd = cmd.ExecuteReader();

                //Get column mapping from T
                _classMapper.CollectAttributes<T>();

                List<MapperMetadata> mapping = null;

                //Check if we should get the mapping done
                if (_classMapper.IsMappable<T>())
                {
                    mapping = _classMapper.GetMappingMetadataByType<T>();
                }

                while (rd.Read())
                {
                    var mappingCopy = new List<MapperMetadata>();

                    if (mapping != null)
                        mappingCopy.AddRange(mapping);

                    var entity = rd.ConvertToObject<T>(mappingCopy);
                    entities.Add(entity);
                }

                _sqlConnection.Close();
            }
            catch (Exception ex)
            {
                throw new DatabaseExecuteException(Messages.Get("DatabaseError"), ex.InnerException);
            }
            finally
            {
                _query = new StringBuilder();
                _sqlConnection.Close();
            }

            return entities;
        }

        public async Task<IEnumerable<T>> ExecuteAsync<T>(object values = null, string query = "") where T : class, new()
        {
            string sqlQuery = string.Empty.GetSqlQuery(query, _query.ToString());

            SqlCommand cmd = new SqlCommand(sqlQuery, _sqlConnection);

            if (values != null)
                cmd.MapSqlParameters(sqlQuery, values);

            var entities = new List<T>();

            try
            {
                _sqlConnection.Open();

                SqlDataReader rd = await cmd.ExecuteReaderAsync();

                //Get column mapping from T
                _classMapper.CollectAttributes<T>();

                List<MapperMetadata> mapping = null;

                //Check if we should get the mapping done
                if (_classMapper.IsMappable<T>())
                {
                    mapping = _classMapper.GetMappingMetadataByType<T>();
                }

                while (rd.Read())
                {
                    var mappingCopy = new List<MapperMetadata>();

                    if (mapping != null)
                        mappingCopy.AddRange(mapping);

                    var entity = rd.ConvertToObject<T>(mappingCopy);
                    entities.Add(entity);
                }

                _sqlConnection.Close();
            }
            catch (Exception ex)
            {
                throw new DatabaseExecuteException(Messages.Get("DatabaseError"), ex.InnerException);
            }
            finally
            {
                _query = new StringBuilder();
                _sqlConnection.Close();
            }

            return entities;
        }

        public int Execute(object values = null, string query = "")
        {
            string sqlQuery = string.Empty.GetSqlQuery(query, _query.ToString());

            if (sqlQuery.ToLower().Contains("select"))
            {
                throw new DatabaseExecuteException(Messages.Get("CannotExecuteOpError"));
            }

            SqlCommand cmd = new SqlCommand(sqlQuery, _sqlConnection);

            if (values != null)
                cmd.MapSqlParameters(sqlQuery, values);

            try
            {
                _sqlConnection.Open();

                var rows = cmd.ExecuteNonQuery();

                _sqlConnection.Close();

                return rows;
            }
            catch (Exception ex)
            {
                throw new DatabaseExecuteException(Messages.Get("DatabaseError"), ex.InnerException);
            }
            finally
            {
                _query = new StringBuilder();
                _sqlConnection.Close();
            }
        }

        public async Task<int> ExecuteAsync(object values = null, string query = "")
        {
            string sqlQuery = string.Empty.GetSqlQuery(query, _query.ToString());

            if (sqlQuery.ToLower().Contains("select"))
            {
                throw new DatabaseExecuteException(Messages.Get("CannotExecuteOpError"));
            }

            SqlCommand cmd = new SqlCommand(sqlQuery, _sqlConnection);

            if (values != null)
                cmd.MapSqlParameters(sqlQuery, values);

            try
            {
                _sqlConnection.Open();

                var rows = await cmd.ExecuteNonQueryAsync();

                _sqlConnection.Close();

                return rows;
            }
            catch (Exception ex)
            {
                throw new DatabaseExecuteException(Messages.Get("DatabaseError"), ex.InnerException);
            }
            finally
            {
                _query = new StringBuilder();
                _sqlConnection.Close();
            }
        }

        public async Task<SqlDataReader> ExecuteAndGetReaderAsync(object values = null, string query = "")
        {
            var sqlQuery = string.IsNullOrWhiteSpace(query) ? _query.ToString() : query;

            SqlCommand cmd = new SqlCommand(sqlQuery, _sqlConnection);

            if (values != null)
                cmd.MapSqlParameters(sqlQuery, values);

            try
            {
                _sqlConnection.Open();

                var rd = await cmd.ExecuteReaderAsync();

                return rd;
            }
            catch (Exception ex)
            {
                throw new DatabaseExecuteException(Messages.Get("DatabaseError"), ex.InnerException);
            }
            finally
            {
                _query = new StringBuilder();
            }
        }

        public SqlDataReader ExecuteAndGetReader(object values = null, string query = "")
        {
            var sqlQuery = string.IsNullOrWhiteSpace(query) ? _query.ToString() : query;

            SqlCommand cmd = new SqlCommand(sqlQuery, _sqlConnection);

            if (values != null)
                cmd.MapSqlParameters(sqlQuery, values);

            try
            {
                _sqlConnection.Open();

                var rd = cmd.ExecuteReader();

                return rd;
            }
            catch (Exception ex)
            {
                throw new DatabaseExecuteException(Messages.Get("DatabaseError"), ex.InnerException);
            }
            finally
            {
                _query = new StringBuilder();
            }
        }
    }
}
