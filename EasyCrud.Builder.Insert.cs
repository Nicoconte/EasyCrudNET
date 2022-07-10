using EasyCrudNET.Exceptions;
using EasyCrudNET.Interfaces.SqlBuilder;
using EasyCrudNET.Resources;

namespace EasyCrudNET
{
    public partial class EasyCrud : IInsertStatement
    {
        public IInsertStatement Insert(params string[] fields)
        {
            if (fields == null || fields.Length < 0)
            {
                throw new SqlBuilderException(Messages.Get("NotEnoughParameterError"));
            }

            var cols = fields.Length == 0 ? string.Empty : string.Concat("(", fields.ToList(), ")");

            _query.Append(string.Concat("INSERT ", cols));

            return this;
        }

        public IInsertStatement Into(string table)
        {
            if (string.IsNullOrWhiteSpace(table))
            {
                throw new SqlBuilderException(Messages.Get("TableNotProvidedError"));
            }

            _query.Append(string.Concat(" INTO ", table));

            return this;
        }

        public IInsertStatement Values(params string[] scalarValues)
        {

            if (scalarValues == null || scalarValues.Length == 0)
            {
                throw new SqlBuilderException(Messages.Get("NotEnoughParameterError"));
            }

            _query.Append(string.Concat(" VALUES (", string.Join(",", scalarValues.ToList()), ")"));

            return this;
        }
    }
}
