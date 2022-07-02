using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET.Interfaces.SqlStatement
{
    public interface IInsertStatement : IDatabase
    {
        public IInsertStatement Insert(params string[] fields);
        public IInsertStatement Into(string table);
        public IInsertStatement Values(params string[] scalarValues);
    }
}
