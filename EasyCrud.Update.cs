using EasyCrudNET.Interfaces.SqlStatement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET
{
    public partial class EasyCrud : IUpdateStatement
    {
        public IUpdateStatement Update(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException($"Invalid args. Invalid table {tableName}");
            }

            _currQuery.Append(string.Concat("UPDATE ", tableName));

            return this;
        }

        public IUpdateStatement Set(string column, string scalarVariable)
        {

            if (string.IsNullOrWhiteSpace(column) && string.IsNullOrWhiteSpace(scalarVariable))
            {
                throw new ArgumentNullException("Invalid args. No column or scalarVariable were provided");
            }

            if (_currQuery.ToString().Contains("SET"))
            {
                _currQuery.Append(string.Concat(",", column, "=", scalarVariable));
            }
            else
            {
                _currQuery.Append(string.Concat(" SET ", column, "=", scalarVariable));
            }
            return this;
        }
    }
}
