using EasyCrudNET.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET
{
    public partial class EasyCrud
    {
        public IDatabaseExecutor DebugQuery(string message = "")
        {
            var messageFixed = string.IsNullOrWhiteSpace(message) ? _currQuery.ToString() : string.Concat(message, ": ", _currQuery.ToString());

            Console.WriteLine(messageFixed);

            return this;
        }
        public string GetRawQuery()
        {
            return _currQuery.ToString();
        }
    }
}
