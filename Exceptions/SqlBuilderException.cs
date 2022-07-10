using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET.Exceptions
{
    public class SqlBuilderException : Exception
    {
        public SqlBuilderException(string message, Exception inner) 
            : base($"\n->Sql builder exception: {message}", inner)
        { }
        public SqlBuilderException(string message)
            : base($"\n->Sql builder exception: {message}")
        { }
    }
}
