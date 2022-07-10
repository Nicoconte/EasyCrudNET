using System.Text;
using System.Data.SqlClient;
using EasyCrudNET.Mappers;

namespace EasyCrudNET
{
    public partial class EasyCrud
    {
        private SqlConnection _sqlConnection;
        private List<List<(string FieldName, object FieldValue)>> _sqlDataReaderResponses = new List<List<(string, object)>>();

        private StringBuilder _query = new StringBuilder(string.Empty);
        private List<(string PropName, object PropValue)> _queryValues = new List<(string, object)>();

        private ClassMapper _classMapper = new ClassMapper();
    }
}