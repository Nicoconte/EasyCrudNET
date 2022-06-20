using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET.Extensions
{
    public static class StringExtension
    {
        public static string GetSqlQuery(this string str, string entryQuery, string currQuery)
        {
            if (!string.IsNullOrWhiteSpace(entryQuery) && !string.IsNullOrWhiteSpace(currQuery))
                str = currQuery.ToString();


            if (string.IsNullOrWhiteSpace(entryQuery) && !string.IsNullOrWhiteSpace(currQuery))
                str = currQuery.ToString();

            if (string.IsNullOrWhiteSpace(currQuery) && !string.IsNullOrWhiteSpace(entryQuery))
                str = entryQuery;

            if (string.IsNullOrWhiteSpace(currQuery) && string.IsNullOrWhiteSpace(entryQuery))
                throw new ArgumentException("Invalid Query. Provide at least one query");

            return str;
        }
    }
}
