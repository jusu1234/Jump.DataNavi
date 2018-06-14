using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataNavi.Model
{
    [DataContract]  
    public class DeptModel
    {
        [Key]
        [DataMember]
        public string DEPTNO { get; set; }

        [DataMember]
        public string DNAME { get; set; }

        [DataMember]
        public string LOC { get; set; }
    }
}
