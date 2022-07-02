using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET.Interfaces.SqlStatement
{
    public interface IClauseStatement : IDatabase
    {        
        public IClauseStatement GroupBy(string column);
        
        public IClauseStatement Having(string conditionQuery);

        public IClauseStatement OrderBy(string column);

        public IClauseStatement Desc();
        public IClauseStatement Asc();
    }
}
