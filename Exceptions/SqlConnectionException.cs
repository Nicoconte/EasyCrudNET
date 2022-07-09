using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET.Exceptions
{
    public class SqlConnectionException : Exception
    {
        public SqlConnectionException(string message) : base($"Sql connection exception: {message}") { }
    }
}
