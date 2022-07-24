using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET.Interfaces.Core
{
    public interface IDatabase
    {    
        /// <summary>
        /// Debug query in the console before execution
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public IDatabase DebugQuery(string message = "");

        /// <summary>
        /// Sets the values ​​of the scalar variables declared in the query. 
        /// Important, the name of the object properties must match with the name of the scalar variables 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public IDatabase BindValues(object values);
    
        /// <summary>
        /// Get the result from SELECT statement. 
        /// Its return a Matrix of List where 
        /// every row is represent by a list and the row's column by tuples (column name, column value) 
        /// </summary>
        /// <returns>List<List<(string, object)>></returns>
        public List<List<(string FieldName, object FieldValue)>> GetResult();

        /// <summary>
        /// Map the result from SELECT statement into T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> MapResultTo<T>() where T : class, new();        

        /// <summary>
        /// Map manually the result SELECT statement into T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="map"></param>
        /// <returns></returns>
        public IEnumerable<T> MapResultTo<T>(Func<List<(string FieldName, object FieldValue)>, T> map) where T : class, new();

        /// <summary>
        /// Execute query built (SELECT STATEMENT) with sql builder. In order to get the result 
        /// we should call some OUTPUT method (Like GetResult or MapResultTo)
        /// </summary>
        /// <returns></returns>
        public IDatabase ExecuteQuery();
        
        public IDatabase FromSql(string query);

        /// <summary>
        /// Execute query built (INSERT/DELETE/UPDATE STATEMENT) with sql builder. 
        /// Its return the number of rows affected
        /// </summary>
        /// <returns></returns>
        public int SaveChanges();
    }
}
