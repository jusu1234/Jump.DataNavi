using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataNavi.Database
{
    public static class ConnectionString
    {
        public static string ORCL = System.Configuration.ConfigurationManager.ConnectionStrings["ORCL"].ConnectionString;
    }
}
