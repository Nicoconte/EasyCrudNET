using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET.Interfaces.SqlBuilder
{
    public interface IDeleteStatement : IConditionStatement
    {
        public IDeleteStatement Delete();
        public IDeleteStatement From(string tableName);
    }
}
