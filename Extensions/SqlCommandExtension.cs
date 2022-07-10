using EasyCrudNET.Exceptions;
using EasyCrudNET.Resources;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET.Extensions
{
    public static class SqlCommandExtension
    {
        public static void MapSqlParameters(this SqlCommand cmd, string query, List<(string PropName, object PropValue)> pairs)
        {
            int propsCount = 0;

            foreach(var pair in pairs)
            {
                var sqlName = $"@{pair.PropName}";

                if (query.Contains(sqlName))
                {
                    cmd.Parameters.AddWithValue(sqlName, pair.PropValue);

                    propsCount++;
                }
            }

            if (propsCount != pairs.Count)
            {
                throw new DatabaseExecuteException(Messages.Get("NotEnoughArgsToMapError"));
            }
        }
    }
}

