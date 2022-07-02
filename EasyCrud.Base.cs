﻿
using EasyCrudNET.Interfaces.SqlStatement;
using EasyCrudNET.Extensions;

using System.Text;
using System.Data.SqlClient;
using EasyCrudNET.Interfaces;
using EasyCrudNET.Mappers;

namespace EasyCrudNET
{
    public partial class EasyCrud
    {
        public EasyCrud() { }

        private SqlConnection _conn;

        private StringBuilder _currQuery = new StringBuilder(string.Empty);

        private ClassMapper _classMapper = new ClassMapper();
    }
}