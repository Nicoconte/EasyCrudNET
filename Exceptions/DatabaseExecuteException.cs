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
            : base($"\n-> Database execute exception. Error: {message}", inner) { }

        public DatabaseExecuteException(string message)
            : base($"\n-> Database execute exception. Error: {message}") { }
    }
}
