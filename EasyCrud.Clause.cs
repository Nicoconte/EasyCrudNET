using EasyCrudNET.Interfaces.SqlStatement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET
{
    public partial class EasyCrud : IClauseStatement
    {
        public IClauseStatement Asc()
        {
            _currQuery.Append(" ASC");
            return this;
        }

        public IClauseStatement Desc()
        {
            _currQuery.Append(" DESC");
            return this;
        }

        public IClauseStatement GroupBy(string column)
        {
            if (column == null || string.IsNullOrWhiteSpace(column))
            {
                throw new ArgumentException("Invalid args: Column wasn't provided");
            }

            _currQuery.Append(string.Concat(" GROUP BY ", column));

            return this;
        }

        public IClauseStatement Having(string conditionQuery)
        {
            if (string.IsNullOrWhiteSpace(conditionQuery))
            {
                throw new ArgumentException("Invalid args: conditionQuery wasn't provided");
            }

            _currQuery.Append(string.Concat(" HAVING ", conditionQuery));

            return this;
        }

        public IClauseStatement OrderBy(string column)
        {
            if (string.IsNullOrWhiteSpace(column))
            {
                throw new ArgumentException("Invalid args. Column wasn't provided");
            }

            _currQuery.Append(string.Concat(" ORDER BY ", column));

            return this;
        }
    }
}
