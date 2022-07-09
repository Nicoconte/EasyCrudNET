using EasyCrudNET.Exceptions;
using EasyCrudNET.Interfaces.SqlStatement;
using EasyCrudNET.Resources;

namespace EasyCrudNET
{
    public partial class EasyCrud : IUpdateStatement
    {
        public IUpdateStatement Update(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new SqlBuilderException(Messages.Get("TableNotProvidedError"));
            }

            _query.Append(string.Concat("UPDATE ", tableName));

            return this;
        }

        public IUpdateStatement Set(string column, string scalarVariable)
        {

            if (string.IsNullOrWhiteSpace(column) || string.IsNullOrWhiteSpace(scalarVariable))
            {
                throw new SqlBuilderException(Messages.Get("NotEnoughParameterError"));
            }

            if (_query.ToString().Contains("SET"))
            {
                _query.Append(string.Concat(",", column, "=", scalarVariable));
            }
            else
            {
                _query.Append(string.Concat(" SET ", column, "=", scalarVariable));
            }
            return this;
        }
    }
}
