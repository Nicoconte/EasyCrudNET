using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET.Exceptions
{
    public class DatabaseExecuteException : Exception
    {
        public DatabaseExecuteException(string message, Exception inner)
            : base($"Database execute exception. Error: {message}", inner) { }

        public DatabaseExecuteException(string message)
            : base($"Database execute exception. Error: {message}") { }
    }
}
