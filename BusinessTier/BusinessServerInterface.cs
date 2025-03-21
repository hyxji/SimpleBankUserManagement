// Izhary Pauline Rodriguez Fortun
// 21486144
// Prac: Thursday 8 AM - 10 AM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Drawing;
using DBInterface;
using CommonContracts;

namespace BusinessTier
{
    [ServiceContract]
    public interface BusinessServerInterface
    {
        [OperationContract]
        int GetNumEntries();

        [OperationContract]
        [FaultContract(typeof(IndexOutOfRangeFault))]
        void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out Bitmap icon);

        [OperationContract]
        [FaultContract(typeof(SearchFault))]
        void GetValuesForSearch(string query, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out Bitmap icon);

    }
}
