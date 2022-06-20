using EasyCrudNET.Interfaces;
using EasyCrudNET.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET.Configuration
{
    public class SqlServerDatabase : IDatabase
    {

        private string _connectionString;

        public SqlServerDatabase(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection GetSqlConnection()
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new ArgumentNullException("Invalid 'String Connection'");
            }

            return new SqlConnection(_connectionString);
        }
    }
}
