using EasyCrudNET.Interfaces.SqlStatement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET
{
    public partial class EasyCrud : IInsertStatement
    {
        public IInsertStatement Insert(params string[] fields)
        {
            if (fields == null || fields.Length < 0)
            {
                throw new ArgumentException("Invalid args. ScalarValues weren't provided");
            }

            var cols = fields.Length == 0 ? string.Empty : string.Concat("(", fields.ToList(), ")");

            _currQuery.Append(string.Concat("INSERT ", cols));

            return this;
        }

        public IInsertStatement Into(string table)
        {
            if (string.IsNullOrWhiteSpace(table))
            {
                throw new ArgumentNullException($"Invalid args. Invalid table {table}");
            }

            _currQuery.Append(string.Concat(" INTO ", table));

            return this;
        }

        public IInsertStatement Values(params string[] scalarValues)
        {

            if (scalarValues == null || scalarValues.Length == 0)
            {
                throw new ArgumentException("Invalid args. No scalarValues provided");
            }

            _currQuery.Append(string.Concat(" VALUES (", string.Join(",", scalarValues.ToList()), ")"));

            return this;
        }
    }
}
