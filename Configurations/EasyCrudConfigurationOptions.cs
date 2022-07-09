using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCrudNET.Configurations
{
    public class EasyCrudConfigurationOptions
    {
        public string ConnectionString { get; set; }

        public void UseSqlServerConnection(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
