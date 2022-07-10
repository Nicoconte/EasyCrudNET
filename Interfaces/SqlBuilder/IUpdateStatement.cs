using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET.Interfaces.SqlBuilder
{
    public interface IUpdateStatement : IConditionStatement
    {
        public IUpdateStatement Update(string tableName);
        public IUpdateStatement Set(string column, string scalarVariable);
    }
}
