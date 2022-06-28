using EasyCrudNET.Interfaces.SqlStatement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET
{
    public partial class EasyCrud : ISelectStatement
    {
        public ISelectStatement Select(params string[] columns)
        {
            if (columns == null || columns?.Length < 0)
            {
                throw new ArgumentException("Invalid args: Columns weren't passed");
            }

            var cols = columns?.Length == 0 ? "*" : string.Join(",", columns.ToList());

            _currQuery.Append(string.Concat("SELECT ", cols));

            return this;
        }

        public ISelectStatement From(string tableName)
        {
            if (tableName == null || string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentException("Invalid args: Table name wasn't provided");
            }

            _currQuery.Append(string.Concat(" FROM ", tableName));

            return this;
        }

        public ISelectStatement InnerJoin(string tableToRelate)
        {
            _currQuery.Append(string.Concat(" INNER JOIN ", tableToRelate));

            return this;
        }

        public ISelectStatement On(string firstRelation, string secondRelation)
        {
            var relation = string.Concat(" ON ", firstRelation, "=", secondRelation);

            _currQuery.Append(relation);

            return this;
        }
    }
}
