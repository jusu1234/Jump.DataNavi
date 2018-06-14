using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataNavi.Database
{
    public static class QueryString
    {
        public static string Dept_Select = @"SELECT * FROM dept 
                                              WHERE 
                                                    DEPTNO = :DEPTNO
                                                AND (NVL(:DNAME,'-') = '-' OR DNAME = :DNAME)";
    }
}
