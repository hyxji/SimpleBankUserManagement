// Izhary Pauline Rodriguez Fortun
// 21486144
// Prac: Thursday 8 AM - 10 AM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CommonContracts
{
    [DataContract]
    public class SearchFault
    {
        [DataMember]
        public string Issue { get; set; }

        [DataMember]
        public int ErrorCode { get; set; }
    }
}
