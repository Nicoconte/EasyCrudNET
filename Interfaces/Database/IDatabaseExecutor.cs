using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET.Interfaces.Database
{
    public interface IDatabaseExecutor
    {
        public IEnumerable<T> Execute<T>(object values) where T : class, new();
        public IEnumerable<T> Execute<T>() where T : class, new();

        public int Execute(object values);
        public int Execute();


        public Task<IEnumerable<T>> ExecuteAsync<T>(object values) where T : class, new();
        public Task<IEnumerable<T>> ExecuteAsync<T>() where T : class, new();

        public Task<int> ExecuteAsync(object values);
        public Task<int> ExecuteAsync();

        public IDatabaseExecutor DebugQuery(string message = "");
        public string GetRawQuery();
    }
}
