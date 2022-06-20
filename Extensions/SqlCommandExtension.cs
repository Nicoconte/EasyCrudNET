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
        public static void MapSqlParameters(this SqlCommand cmd, string sqlQuery, object values)
        {
            int propsCount = 0;

            foreach (var value in values.GetType().GetProperties())
            {
                if (sqlQuery.Contains($"@{value.Name}"))
                {
                    var propFinded = value.Name;
                    var propValue = values?.GetType()?.GetProperty(propFinded)?.GetValue(values);
                    cmd.Parameters.AddWithValue($"@{propFinded}", propValue);

                    propsCount++;
                }
            }

            if (propsCount != values.GetType().GetProperties().Length)
            {
                throw new ArgumentException("Invalid args: Not enough values passed");
            }
        }
    }
}

