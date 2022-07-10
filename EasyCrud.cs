using System.Text;
using System.Data.SqlClient;
using EasyCrudNET.Mappers;

namespace EasyCrudNET
{
    public partial class EasyCrud
    {
        private StringBuilder _query = new StringBuilder(string.Empty);
    }
}