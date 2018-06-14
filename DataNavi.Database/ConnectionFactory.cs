using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataNavi.Database
{
    public static class ConnectionFactory
    {
        public static Func<DbConnection> OrclConnection = () => new OracleConnection(ConnectionString.ORCL);
    }
}
