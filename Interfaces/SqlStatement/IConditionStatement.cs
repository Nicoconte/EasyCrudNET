using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET.Interfaces.SqlStatement
{
    public interface IConditionStatement : IDatabase, IClauseStatement
    {
        public IConditionStatement Where();
        public IConditionStatement Where(string column, string scalarVariable);
        
        public IConditionStatement Or();
        public IConditionStatement Or(string column, string scalarVariable);
        
        public IConditionStatement And();
        public IConditionStatement And(string column, string scalarVariable);

        public IConditionStatement Not();
        
        public IConditionStatement Like(string column, string expression);

        public IConditionStatement In(params object[] values);

        public IConditionStatement LessThan(string column, string scalarVariable);
        public IConditionStatement GreaterThan(string column, string scalarVariable);

        public IConditionStatement IsNotNull(string column);
        public IConditionStatement IsNull(string column);

        public IConditionStatement Between(string column, string firstScalarVariable, string secondScalarVariable);
    }
}
