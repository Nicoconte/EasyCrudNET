using EasyCrudNET.Extensions;
using EasyCrudNET.Interfaces.Database;
using EasyCrudNET.Mappers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET
{
    public partial class EasyCrud
    {
        public IEnumerable<T> Execute<T>(object values = null, string query = "") where T : class, new()
        {
            string sqlQuery = string.Empty.GetSqlQuery(query, _currQuery.ToString());

            SqlCommand cmd = new SqlCommand(sqlQuery, _conn);

            if (values != null)
                cmd.MapSqlParameters(sqlQuery, values);

            var entities = new List<T>();

            try
            {
                _conn.Open();

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

                _conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception($"Something goes wrong during the operation. Error: {ex.Message}");
            }
            finally
            {
                _currQuery = new StringBuilder();
                _conn.Close();
            }

            return entities;
        }

        public async Task<IEnumerable<T>> ExecuteAsync<T>(object values = null, string query = "") where T : class, new()
        {
            string sqlQuery = string.Empty.GetSqlQuery(query, _currQuery.ToString());

            SqlCommand cmd = new SqlCommand(sqlQuery, _conn);

            if (values != null)
                cmd.MapSqlParameters(sqlQuery, values);

            var entities = new List<T>();

            try
            {
                _conn.Open();

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

                _conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception($"Something goes wrong during the operation. Error: {ex.Message}");
            }
            finally
            {
                _currQuery = new StringBuilder();
                _conn.Close();
            }

            return entities;
        }

        public int Execute(object values = null, string query = "")
        {
            string sqlQuery = string.Empty.GetSqlQuery(query, _currQuery.ToString());

            if (sqlQuery.ToLower().Contains("select"))
            {
                throw new ArgumentException("Invalid query. This method will only execute with insert/delete/update statement");
            }

            SqlCommand cmd = new SqlCommand(sqlQuery, _conn);

            if (values != null)
                cmd.MapSqlParameters(sqlQuery, values);

            try
            {
                _conn.Open();

                var rows = cmd.ExecuteNonQuery();

                _conn.Close();

                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception($"Something goes wrong during the operation. Error: {ex.Message}");
            }
            finally
            {
                _currQuery = new StringBuilder();
                _conn.Close();
            }
        }

        public async Task<int> ExecuteAsync(object values = null, string query = "")
        {
            string sqlQuery = string.Empty.GetSqlQuery(query, _currQuery.ToString());

            if (sqlQuery.ToLower().Contains("select"))
            {
                throw new ArgumentException("Invalid query. This method will only execute with insert/delete/update statement");
            }

            SqlCommand cmd = new SqlCommand(sqlQuery, _conn);

            if (values != null)
                cmd.MapSqlParameters(sqlQuery, values);

            try
            {
                _conn.Open();

                var rows = await cmd.ExecuteNonQueryAsync();

                _conn.Close();

                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception($"Something goes wrong during the operation. Error: {ex.Message}");
            }
            finally
            {
                _currQuery = new StringBuilder();
                _conn.Close();
            }
        }

        public async Task<SqlDataReader> ExecuteAndGetReaderAsync(object values = null, string query = "")
        {
            var sqlQuery = string.IsNullOrWhiteSpace(query) ? _currQuery.ToString() : query;

            SqlCommand cmd = new SqlCommand(sqlQuery, _conn);

            if (values != null)
                cmd.MapSqlParameters(sqlQuery, values);

            try
            {
                _conn.Open();

                var rd = await cmd.ExecuteReaderAsync();

                return rd;
            }
            catch (Exception ex)
            {
                throw new Exception($"Something goes wrong during the operation. Error: {ex.Message}");
            }
            finally
            {
                _currQuery = new StringBuilder();
            }
        }

        public SqlDataReader ExecuteAndGetReader(object values = null, string query = "")
        {
            var sqlQuery = string.IsNullOrWhiteSpace(query) ? _currQuery.ToString() : query;

            SqlCommand cmd = new SqlCommand(sqlQuery, _conn);

            if (values != null)
                cmd.MapSqlParameters(sqlQuery, values);

            try
            {
                _conn.Open();

                var rd = cmd.ExecuteReader();

                return rd;
            }
            catch (Exception ex)
            {
                throw new Exception($"Something goes wrong during the operation. Error: {ex.Message}");
            }
            finally
            {
                _currQuery = new StringBuilder();
            }
        }
    }
}
