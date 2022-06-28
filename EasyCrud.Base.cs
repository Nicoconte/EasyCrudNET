
using EasyCrudNET.Interfaces.SqlStatement;
using EasyCrudNET.Extensions;

using System.Text;
using System.Data.SqlClient;
using EasyCrudNET.Interfaces.Database;
using EasyCrudNET.Mappers;
using EasyCrudNET.Configuration;

namespace EasyCrudNET
{
    public partial class EasyCrud
    {
        public EasyCrud(SqlConnection conn)
        {
            _conn = conn;
        }

        private SqlConnection _conn;

        private StringBuilder _currQuery = new StringBuilder(string.Empty);

        private ClassMapper _classMapper = new ClassMapper();
    }
}