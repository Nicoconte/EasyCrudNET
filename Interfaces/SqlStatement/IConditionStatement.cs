using EasyCrudNET.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET.Interfaces.SqlStatement
{
    public interface IConditionStatement : IDatabaseExecutor
    {
        public IConditionStatement Where();
        public IConditionStatement Where(string column, string scalarVariable);
        
        public IConditionStatement Or();
        public IConditionStatement Or(string column, string scalarVariable);
        
        public IConditionStatement And();
        public IConditionStatement And(string column, string scalarVariable);
        
        public IConditionStatement Like(string column, string expression);
       
        public IConditionStatement OrderBy(string column);

        public IConditionStatement In(params object[] values);

        public IConditionStatement LessThan(string column, string scalarVariable);
        public IConditionStatement GreaterThan(string column, string scalarVariable);

        public IConditionStatement NotNull(string column);
        public IConditionStatement IsNull(string column);
    }
}
