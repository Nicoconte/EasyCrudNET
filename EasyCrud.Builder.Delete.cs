using EasyCrudNET.Exceptions;
using EasyCrudNET.Interfaces.SqlBuilder;
using EasyCrudNET.Resources;

namespace EasyCrudNET
{
    public partial class EasyCrud : IDeleteStatement
    {
        public IDeleteStatement Delete()
        {
            _query.Append("DELETE ");

            return this;
        }

        IDeleteStatement IDeleteStatement.From(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new SqlBuilderException(Messages.Get("TableNotProvidedError"));
            }

            _query.Append(string.Concat(" FROM ", tableName));

            return this;
        }
    }
}
