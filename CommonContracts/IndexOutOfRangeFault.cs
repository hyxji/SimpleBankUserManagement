// Izhary Pauline Rodriguez Fortun
// 21486144
// Prac: Thursday 8 AM - 10 AM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CommonContracts
{

    [DataContract]
    public class IndexOutOfRangeFault
    {
        [DataMember]
        public string Issue { get; set; }
    }
}

