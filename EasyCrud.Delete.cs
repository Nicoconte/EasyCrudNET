using EasyCrudNET.Interfaces.SqlStatement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET
{
    public partial class EasyCrud : IDeleteStatement
    {
        public IDeleteStatement Delete()
        {
            _currQuery.Append("DELETE ");

            return this;
        }

        IDeleteStatement IDeleteStatement.From(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException($"Invalid args. Invalid table {tableName}");
            }

            _currQuery.Append(string.Concat(" FROM ", tableName));

            return this;
        }
    }
}
