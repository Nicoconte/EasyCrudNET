using System.Text;
using System.Data.SqlClient;
using EasyCrudNET.Mappers;

namespace EasyCrudNET
{
    public partial class EasyCrud
    {
        private SqlConnection _sqlConnection;

        private StringBuilder _query = new StringBuilder(string.Empty);

        private ClassMapper _classMapper = new ClassMapper();
    }
}