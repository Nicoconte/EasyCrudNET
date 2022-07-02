using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET.Interfaces
{
    public interface IDatabase
    {
        /// <summary>
        /// Get Sql Server connection
        /// </summary>
        /// <returns></returns>
        public void SetSqlConnection(string connectionString);

        /// <summary>
        /// Execute query and return a List<T> 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="query"></param>
        /// <returns>a list of T objects</returns>
        public IEnumerable<T> Execute<T>(object values=null, string query="") where T : class, new();

        /// <summary>
        /// Execute async query and return a List<T> 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="query"></param>
        /// <returns>a list of T objects</returns>
        public Task<IEnumerable<T>> ExecuteAsync<T>(object values=null, string query="") where T : class, new();

        /// <summary>
        /// Execute query (Insert, update or delete) and return affected rows 
        /// </summary
        /// <param name="values"></param>
        /// <param name="query"></param>
        /// <returns>affected rows (INT)</returns>
        public int Execute(object values=null, string query="");

        /// <summary>
        /// Execute async query (Insert, update or delete) and return affected rows 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="query"></param>
        /// <returns>affected rows (INT)</returns>
        public Task<int> ExecuteAsync(object values=null, string query="");

        /// <summary>
        /// Execute async query and return a SqlDataReader object. Do not forget to close it after use.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="query"></param>
        /// <returns>SqlDataReader object</returns>
        public Task<SqlDataReader> ExecuteAndGetReaderAsync(object values=null, string query="");

        /// <summary>
        /// Execute async query and return a SqlDataReader object. Do not forget to close it after use.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="query"></param>
        /// <returns>SqlDataReader object</returns>
        public SqlDataReader ExecuteAndGetReader(object values=null, string query="");        
    
        /// <summary>
        /// Debug query in the console before execution
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public IDatabase DebugQuery(string message = "");

        /// <summary>
        /// Get the query built
        /// </summary>
        /// <returns>Query built</returns>
        public string GetRawQuery();
    }
}
