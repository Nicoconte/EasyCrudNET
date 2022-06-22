
using EasyCrudNET.Interfaces.SqlStatement;
using EasyCrudNET.Extensions;

using System.Text;
using System.Data.SqlClient;
using EasyCrudNET.Interfaces.Database;
using EasyCrudNET.Mappers;
using EasyCrudNET.Configuration;

namespace EasyCrudNET
{
    public class EasyCrud : ISelectStatement, IInsertStatement, IUpdateStatement, IDeleteStatement
    {
        public EasyCrud(SqlConnection conn)
        {
            _conn = conn;
        }

        private SqlConnection _conn;
        private StringBuilder _currQuery = new StringBuilder(string.Empty);

        public Mapper Mapper = null;

        #region select
        public ISelectStatement Select(params string[] columns)
        {
            if (columns == null || columns?.Length < 0)
            {
                throw new ArgumentException("Invalid args: Columns weren't passed");
            }

            var cols = columns?.Length == 0 ? "*" : string.Join(",", columns.ToList());

            _currQuery.Append(string.Concat("SELECT ", cols));

            return this;
        }

        public ISelectStatement From(string tableName)
        {
            if (tableName == null || string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentException("Invalid args: Table name wasn't provided");
            }

            _currQuery.Append(string.Concat(" FROM ", tableName));

            return this;
        }

        public ISelectStatement InnerJoin(string tableToRelate)
        {
            _currQuery.Append(string.Concat(" INNER JOIN ", tableToRelate));

            return this;
        }

        public ISelectStatement On(string firstRelation, string secondRelation)
        {
            var relation = string.Concat(" ON ", firstRelation, "=", secondRelation);

            _currQuery.Append(relation);

            return this;
        }
        #endregion select

        #region condition
        public IConditionStatement And()
        {
            _currQuery.Append(" AND ");

            return this;
        }

        public IConditionStatement And(string column, string scalarVariable)
        {
            if (column == null || string.IsNullOrWhiteSpace(column) || scalarVariable == null || string.IsNullOrWhiteSpace(scalarVariable))
            {
                throw new ArgumentException("Invalid args: Column or ScalarVariable weren't provided");
            }

            _currQuery.Append(string.Concat(" AND ", column, "=", scalarVariable));

            return this;
        }

        public IConditionStatement GreaterThan(string column, string scalarVariable)
        {
            if (column == null || string.IsNullOrWhiteSpace(column) || scalarVariable == null || string.IsNullOrWhiteSpace(scalarVariable))
            {
                throw new ArgumentException("Invalid args: Column or ScalarVariable weren't provided");
            }

            _currQuery.Append(string.Concat(" ", column, " > ", scalarVariable));

            return this;
        }

        public IConditionStatement LessThan(string column, string scalarVariable)
        {
            _currQuery.Append(string.Concat(" ", column, " < ", scalarVariable));

            return this;
        }

        public IConditionStatement In(params object[] values)
        {
            if (values == null || values?.Length < 0)
            {
                throw new ArgumentException("Invalid args: Columns weren't passed");
            }

            var inValues = string.Concat("(", string.Join(",", values.ToList()), ")");

            _currQuery.Append(string.Concat(" IN ", inValues));

            return this;
        }

        public IConditionStatement IsNull(string column)
        {
            _currQuery.Append(string.Concat(" ", column," IS NULL"));

            return this;
        }

        public IConditionStatement Like(string column, string expression)
        {
            _currQuery.Append(string.Concat(" ", column, " LIKE ", $"'{expression}'"));

            return this;
        }

        public IConditionStatement IsNotNull(string column)
        {
            _currQuery.Append(string.Concat(" ", column, " IS NOT NULL"));


            return this;
        }

        public IConditionStatement Or()
        {
            _currQuery.Append(" OR ");

            return this;
        }

        public IConditionStatement Or(string column, string scalarVariable)
        {
            if (string.IsNullOrWhiteSpace(column) && string.IsNullOrWhiteSpace(scalarVariable))
            {
                throw new ArgumentNullException("Invalid args. Column or scalarVariable weren't provided");
            }

            _currQuery.Append(string.Concat(" OR ", column, "=", scalarVariable));

            return this;
        }

        public IConditionStatement OrderBy(string column)
        {

            if (string.IsNullOrWhiteSpace(column))
            {
                throw new ArgumentException("Invalid args. Column wasn't provided");
            }

            _currQuery.Append(string.Concat(" ORDER BY ", column));

            return this;
        }

        public IConditionStatement Where()
        {
            _currQuery.Append(" WHERE");

            return this;
        }

        public IConditionStatement Where(string column, string scalarVariable)
        {
            if (string.IsNullOrWhiteSpace(column) && string.IsNullOrWhiteSpace(scalarVariable))
            {
                throw new ArgumentNullException("Invalid args. Column or scalarVariable weren't provided");
            }

            _currQuery.Append(string.Concat(" WHERE ", column, "=", scalarVariable));

            return this;
        }
        #endregion condition

        #region insert
        public IInsertStatement Insert(params string[] fields)
        {
            if (fields == null || fields.Length < 0)
            {
                throw new ArgumentException("Invalid args. ScalarValues weren't provided");
            }

            var cols = fields.Length == 0 ? string.Empty : string.Concat("(", fields.ToList(), ")");

            _currQuery.Append(string.Concat("INSERT ", cols));

            return this;
        }

        public IInsertStatement Into(string table)
        {
            if (string.IsNullOrWhiteSpace(table))
            {
                throw new ArgumentNullException($"Invalid args. Invalid table {table}");
            }

            _currQuery.Append(string.Concat(" INTO ", table));

            return this;
        }

        public IInsertStatement Values(params string[] scalarValues)
        {

            if (scalarValues == null || scalarValues.Length == 0)
            {
                throw new ArgumentException("Invalid args. No scalarValues provided");
            }

            _currQuery.Append(string.Concat(" VALUES (", string.Join(",", scalarValues.ToList()), ")"));

            return this;
        }
        #endregion insert

        #region executor
        public IEnumerable<T> Execute<T>(object values=null, string query = "") where T : class, new()
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

                ClassMapper classMapper = null;

                if (Mapper != null)
                {
                    classMapper = Mapper.GetClassMapperByType<T>();
                }

                while (rd.Read())
                {
                    var entity = rd.ConvertToObject<T>(classMapper);
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

            Console.WriteLine(sqlQuery);

            SqlCommand cmd = new SqlCommand(sqlQuery, _conn);

            if (values != null)
                cmd.MapSqlParameters(sqlQuery, values);

            var entities = new List<T>();

            try
            {
                _conn.Open();

                SqlDataReader rd = await cmd.ExecuteReaderAsync();

                ClassMapper classMapper = null;

                if (Mapper != null)
                {
                    classMapper = Mapper.GetClassMapperByType<T>();
                }

                while (rd.Read())
                {
                    var entity = rd.ConvertToObject<T>(classMapper);
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

        public int Execute(object values=null, string query = "")
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

        public async Task<int> ExecuteAsync(object values=null, string query = "")
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

        public SqlDataReader ExecuteAndGetReader(object values = null, string query="")
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

        #endregion executor

        #region update
        public IUpdateStatement Update(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException($"Invalid args. Invalid table {tableName}");
            }

            _currQuery.Append(string.Concat("UPDATE ", tableName));

            return this;
        }

        public IUpdateStatement Set(string column, string scalarVariable)
        {

            if (string.IsNullOrWhiteSpace(column) && string.IsNullOrWhiteSpace(scalarVariable))
            {
                throw new ArgumentNullException("Invalid args. No column or scalarVariable were provided");
            }

            if (_currQuery.ToString().Contains("SET"))
            {
                _currQuery.Append(string.Concat(",", column, "=", scalarVariable));
            }
            else
            {
                _currQuery.Append(string.Concat(" SET ", column, "=", scalarVariable));
            }
            return this;
        }
        #endregion update

        #region debugger
        public IDatabaseExecutor DebugQuery(string message = "")
        {
            var messageFixed = string.IsNullOrWhiteSpace(message) ? _currQuery.ToString() : string.Concat(message, ": ", _currQuery.ToString());

            Console.WriteLine(messageFixed);

            return this;
        }
        public string GetRawQuery()
        {
            return _currQuery.ToString();
        }
        #endregion debugger

        #region delete
        public IDeleteStatement Delete()
        {
            _currQuery.Append("DELETE ");

            return this;
        }

        IDeleteStatement IDeleteStatement.From(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException($"Invalid args. Invalid table {tableName}");
            }

            _currQuery.Append(string.Concat(" FROM ", tableName));

            return this;
        }
        #endregion delete
    }
}