using EasyCrudNET.Interfaces.SqlStatement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET
{
    public partial class EasyCrud : IConditionStatement
    {
        public IConditionStatement And()
        {
            _currQuery.Append(" AND ");

            return this;
        }

        public IConditionStatement And(string column, string scalarVariable)
        {
            if (column == null || string.IsNullOrWhiteSpace(column) || scalarVariable == null || string.IsNullOrWhiteSpace(scalarVariable))
            {
                throw new ArgumentException("Invalid args: Column or ScalarVariable weren't provided");
            }

            _currQuery.Append(string.Concat(" AND ", column, "=", scalarVariable));

            return this;
        }

        public IConditionStatement GreaterThan(string column, string scalarVariable)
        {
            if (column == null || string.IsNullOrWhiteSpace(column) || scalarVariable == null || string.IsNullOrWhiteSpace(scalarVariable))
            {
                throw new ArgumentException("Invalid args: Column or ScalarVariable weren't provided");
            }

            _currQuery.Append(string.Concat(" ", column, " > ", scalarVariable));

            return this;
        }

        public IConditionStatement LessThan(string column, string scalarVariable)
        {
            _currQuery.Append(string.Concat(" ", column, " < ", scalarVariable));

            return this;
        }

        public IConditionStatement In(params object[] values)
        {
            if (values == null || values?.Length < 0)
            {
                throw new ArgumentException("Invalid args: Columns weren't passed");
            }

            var inValues = string.Concat("(", string.Join(",", values.ToList()), ")");

            _currQuery.Append(string.Concat(" IN ", inValues));

            return this;
        }

        public IConditionStatement IsNull(string column)
        {
            _currQuery.Append(string.Concat(" ", column, " IS NULL"));

            return this;
        }

        public IConditionStatement Like(string column, string expression)
        {
            _currQuery.Append(string.Concat(" ", column, " LIKE ", $"'{expression}'"));

            return this;
        }

        public IConditionStatement IsNotNull(string column)
        {
            _currQuery.Append(string.Concat(" ", column, " IS NOT NULL"));


            return this;
        }

        public IConditionStatement Or()
        {
            _currQuery.Append(" OR ");

            return this;
        }

        public IConditionStatement Or(string column, string scalarVariable)
        {
            if (string.IsNullOrWhiteSpace(column) && string.IsNullOrWhiteSpace(scalarVariable))
            {
                throw new ArgumentNullException("Invalid args. Column or scalarVariable weren't provided");
            }

            _currQuery.Append(string.Concat(" OR ", column, "=", scalarVariable));

            return this;
        }

        public IConditionStatement Where()
        {
            _currQuery.Append(" WHERE");

            return this;
        }

        public IConditionStatement Where(string column, string scalarVariable)
        {
            if (string.IsNullOrWhiteSpace(column) && string.IsNullOrWhiteSpace(scalarVariable))
            {
                throw new ArgumentNullException("Invalid args. Column or scalarVariable weren't provided");
            }

            _currQuery.Append(string.Concat(" WHERE ", column, "=", scalarVariable));

            return this;
        }

        public IConditionStatement Between(string column, string firstScalarVariable, string secondScalarVariable)
        {
            if (string.IsNullOrWhiteSpace(column) || string.IsNullOrWhiteSpace(firstScalarVariable) || string.IsNullOrWhiteSpace(secondScalarVariable))
            {
                throw new ArgumentNullException("Invalid args. Scalar variables or column weren't provided");
            }

            _currQuery.Append(string.Concat(" ", column, " BETWEEN ", firstScalarVariable, " AND ", secondScalarVariable));

            return this;
        }

        public IConditionStatement Not()
        {
            _currQuery.Append(" NOT ");

            return this;
        }
    }
}
