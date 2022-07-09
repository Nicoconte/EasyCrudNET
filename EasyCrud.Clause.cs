using EasyCrudNET.Exceptions;
using EasyCrudNET.Interfaces.SqlStatement;
using EasyCrudNET.Resources;

namespace EasyCrudNET
{
    public partial class EasyCrud : IClauseStatement
    {
        public IClauseStatement Asc()
        {
            _query.Append(" ASC");
            return this;
        }

        public IClauseStatement Desc()
        {
            _query.Append(" DESC");
            return this;
        }

        public IClauseStatement GroupBy(string column)
        {
            if (string.IsNullOrWhiteSpace(column))
            {
                throw new SqlBuilderException(Messages.Get("ColumnNotProvidedError"));
            }

            _query.Append(string.Concat(" GROUP BY ", column));

            return this;
        }

        public IClauseStatement Having(string conditionQuery)
        {
            if (string.IsNullOrWhiteSpace(conditionQuery))
            {
                throw new SqlBuilderException(Messages.Get("ConditionQueryError"));
            }

            _query.Append(string.Concat(" HAVING ", conditionQuery));

            return this;
        }

        public IClauseStatement OrderBy(string column)
        {
            if (string.IsNullOrWhiteSpace(column))
            {
                throw new SqlBuilderException(Messages.Get("ColumnNotProvidedError"));
            }

            _query.Append(string.Concat(" ORDER BY ", column));

            return this;
        }
    }
}
