using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET.Interfaces.Core
{
    public interface ITransaction
    {
        public ITransaction BeginTransaction();
        public ITransaction Query(string sqlQuery, object sqlValues);
        public bool Commit();
    }
}
