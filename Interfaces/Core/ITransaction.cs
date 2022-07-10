using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET.Interfaces.Core
{
    public interface ITransaction
    {
        public ITransaction BeginTransaction(Action<List<(string TransactionQuery, object TransactionValues)>> setupAction);
        public bool Commit();
    }
}
