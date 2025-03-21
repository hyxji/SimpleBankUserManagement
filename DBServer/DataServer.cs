// Izhary Pauline Rodriguez Fortun
// 21486144
// Prac: Thursday 8 AM - 10 AM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ServiceModel;
using DBInterface;
using DBLib;
using CommonContracts;

namespace DBServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class DataServer : DataServerInterface
    {
        private readonly Database _db = Database.Instance;

        public int GetNumEntries()
        {
            return _db.GetNumRecords();
        }

        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out Bitmap icon)
        {
            if (index < 0 || index >= _db.GetNumRecords())
            {
                Console.WriteLine("Client tried to get a record that was out of range...");
                throw new FaultException<IndexOutOfRangeFault>(new IndexOutOfRangeFault()
                { Issue = "Index was not in range..." });
            }

            acctNo = _db.GetAcctNoByIndex(index);
            pin = _db.GetPINByIndex(index);
            bal = _db.GetBalanceByIndex(index);
            fName = _db.GetFirstNameByIndex(index);
            lName = _db.GetLastNameByIndex(index);
            icon = new Bitmap(_db.GetIconByIndex(index));

        }

        public void GetValuesForSearch(string query, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out Bitmap icon)
        {
            var entry = _db.Search(query);

            if (entry != null)
            {
                acctNo = entry.acctNo;
                pin = entry.pin;
                bal = entry.balance;
                fName = entry.firstName;
                lName = entry.lastName;
                icon = entry.icon;
            }
            else
            {
                throw new FaultException<SearchFault>(new SearchFault { Issue = "Entry not found" });
            }
        }

    }
}
