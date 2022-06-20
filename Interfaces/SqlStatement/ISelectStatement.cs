using EasyCrudNET.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET.Interfaces.SqlStatement
{
    public interface ISelectStatement : IConditionStatement, IDatabaseExecutor
    {
        public ISelectStatement Select(params string[] columns);
        public ISelectStatement From(string tableName);
        public ISelectStatement InnerJoin(string tableToRelate);
        public ISelectStatement On(string firstRelation, string secondRelation);
    }
}
