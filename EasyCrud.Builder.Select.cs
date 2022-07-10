using EasyCrudNET.Exceptions;
using EasyCrudNET.Interfaces.SqlBuilder;
using EasyCrudNET.Resources;

namespace EasyCrudNET
{
    public partial class EasyCrud : ISelectStatement
    {
        public ISelectStatement Select(params string[] columns)
        {
            if (columns == null || columns?.Length < 0)
            {
                throw new SqlBuilderException(Messages.Get("NotEnoughParameterError"));
            }

            var cols = columns?.Length == 0 ? "*" : string.Join(",", columns.ToList());

            _query.Append(string.Concat("SELECT ", cols));

            return this;
        }

        public ISelectStatement From(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new SqlBuilderException(Messages.Get("TableNotProvidedError"));
            }

            _query.Append(string.Concat(" FROM ", tableName));

            return this;
        }

        public ISelectStatement InnerJoin(string tableToRelate)
        {

            if (string.IsNullOrWhiteSpace(tableToRelate))
            {
                throw new SqlBuilderException(Messages.Get("TableNotProvidedError"));
            }

            _query.Append(string.Concat(" INNER JOIN ", tableToRelate));

            return this;
        }

        public ISelectStatement On(string firstRelation, string secondRelation)
        {
            if (string.IsNullOrWhiteSpace(firstRelation) || string.IsNullOrWhiteSpace(secondRelation))
            {
                throw new SqlBuilderException(Messages.Get("NotEnoughParameterError"));
            }

            var relation = string.Concat(" ON ", firstRelation, "=", secondRelation);

            _query.Append(relation);

            return this;
        }
    }
}
