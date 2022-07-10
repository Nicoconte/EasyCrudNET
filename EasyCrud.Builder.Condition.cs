using EasyCrudNET.Exceptions;
using EasyCrudNET.Interfaces.SqlBuilder;
using EasyCrudNET.Resources;

namespace EasyCrudNET
{
    public partial class EasyCrud : IConditionStatement
    {
        public IConditionStatement And()
        {
            _query.Append(" AND ");

            return this;
        }

        public IConditionStatement And(string column, string scalarVariable)
        {
            if (string.IsNullOrWhiteSpace(column) || string.IsNullOrWhiteSpace(scalarVariable))
            {
                throw new SqlBuilderException(Messages.Get("NotEnoughParameterError"));
            }

            _query.Append(string.Concat(" AND ", column, "=", scalarVariable));

            return this;
        }

        public IConditionStatement GreaterThan(string column, string scalarVariable)
        {
            if (string.IsNullOrWhiteSpace(column) || string.IsNullOrWhiteSpace(scalarVariable))
            {
                throw new SqlBuilderException(Messages.Get("NotEnoughParameterError"));
            }

            _query.Append(string.Concat(" ", column, " > ", scalarVariable));

            return this;
        }

        public IConditionStatement LessThan(string column, string scalarVariable)
        {
            if (string.IsNullOrWhiteSpace(column) || string.IsNullOrWhiteSpace(scalarVariable))
            {
                throw new SqlBuilderException(Messages.Get("NotEnoughParameterError"));
            }

            _query.Append(string.Concat(" ", column, " < ", scalarVariable));

            return this;
        }

        public IConditionStatement In(params object[] values)
        {
            if (values == null || values?.Length < 0)
            {
                throw new SqlBuilderException(Messages.Get("NotEnoughParameterError"));
            }

            var inValues = string.Concat("(", string.Join(",", values.ToList()), ")");

            _query.Append(string.Concat(" IN ", inValues));

            return this;
        }

        public IConditionStatement IsNull(string column)
        {
            if (string.IsNullOrWhiteSpace(column))
            {
                throw new SqlBuilderException(Messages.Get("ColumnNotProvidedError"));
            }

            _query.Append(string.Concat(" ", column, " IS NULL"));

            return this;
        }

        public IConditionStatement Like(string column, string expression)
        {

            if (string.IsNullOrWhiteSpace(column) || string.IsNullOrWhiteSpace(expression))
            {
                throw new SqlBuilderException(Messages.Get("NotEnoughParameterError"));
            }

            _query.Append(string.Concat(" ", column, " LIKE ", $"'{expression}'"));

            return this;
        }

        public IConditionStatement IsNotNull(string column)
        {

            if (string.IsNullOrWhiteSpace(column))
            {
                throw new SqlBuilderException(Messages.Get("ColumnNotProvidedError"));
            }

            _query.Append(string.Concat(" ", column, " IS NOT NULL"));

            return this;
        }

        public IConditionStatement Or()
        {
            _query.Append(" OR ");

            return this;
        }

        public IConditionStatement Or(string column, string scalarVariable)
        {
            if (string.IsNullOrWhiteSpace(column) || string.IsNullOrWhiteSpace(scalarVariable))
            {
                throw new SqlBuilderException(Messages.Get("NotEnoughParameterError"));
            }

            _query.Append(string.Concat(" OR ", column, "=", scalarVariable));

            return this;
        }

        public IConditionStatement Where()
        {
            _query.Append(" WHERE");

            return this;
        }

        public IConditionStatement Where(string column, string scalarVariable)
        {
            if (string.IsNullOrWhiteSpace(column) || string.IsNullOrWhiteSpace(scalarVariable))
            {
                throw new SqlBuilderException(Messages.Get("NotEnoughParameterError"));
            }

            _query.Append(string.Concat(" WHERE ", column, "=", scalarVariable));

            return this;
        }

        public IConditionStatement Between(string column, string firstScalarVariable, string secondScalarVariable)
        {
            if (string.IsNullOrWhiteSpace(column) || string.IsNullOrWhiteSpace(firstScalarVariable) || string.IsNullOrWhiteSpace(secondScalarVariable))
            {
                throw new SqlBuilderException(Messages.Get("NotEnoughParameterError"));
            }

            _query.Append(string.Concat(" ", column, " BETWEEN ", firstScalarVariable, " AND ", secondScalarVariable));

            return this;
        }

        public IConditionStatement Not()
        {
            _query.Append(" NOT ");

            return this;
        }
    }
}
