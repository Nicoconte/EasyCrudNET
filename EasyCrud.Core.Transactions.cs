using EasyCrudNET.Extensions;
using EasyCrudNET.Helpers;
using EasyCrudNET.Interfaces.Core;
using System.Data.SqlClient;

namespace EasyCrudNET
{
    public partial class EasyCrud : ITransaction
    {
        private List<(string TransactionQuery, object TransactionValues)> _transactionQueries { get; set; } = new List<(string TransactionQuery, object TransactionValues)>();

        public ITransaction BeginTransaction(Action<List<(string TransactionQuery, object TransactionValues)>> setupAction)
        {
            if (_transactionQueries.Count > 0)
            {
                _transactionQueries.Clear();
            }

            setupAction(_transactionQueries);
            
            return this;
        }

        public bool Commit()
        {
            if (!_transactionQueries.Any())
            {
                throw new Exception("nop");
            }

            SqlTransaction transaction = null;

            int rows = 0;

            try
            {
                _sqlConnection.Open();
                transaction = _sqlConnection.BeginTransaction();
  
                SqlCommand cmd = _sqlConnection.CreateCommand();

                cmd.Transaction = transaction;                

                foreach(var (query, values) in _transactionQueries)
                {
                    cmd.CommandText = query;

                    if (values != null)
                        cmd.MapSqlParameters(query, ObjectHelper.DestructureObject(values));

                    rows += cmd.ExecuteNonQuery();
                }

                transaction.Commit();

                return rows > 0;
            }
            catch (Exception ex)
            {
                if (transaction != null)
                    transaction.Rollback();

                throw new Exception($"Transaction rollback started. Something goes wrong during transaction. Error: {ex.Message}. StrackTrace: {ex.StackTrace}");
            }
            finally
            {
                _sqlConnection.Close();
                _transactionQueries.Clear();
            }
        }
    }
}
