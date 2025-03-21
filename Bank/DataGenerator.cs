// Izhary Pauline Rodriguez Fortun
// 21486144
// Prac: Thursday 8 AM - 10 AM

using System;
using System.Collections.Generic;
using System.Drawing;

namespace DBLib
{
    internal class DataGenerator
    {
        private readonly Random _rand = new Random();

        private readonly string[] _fNameList = {
            "Chico", "Oreo", "Peppa", "Ari", "Moosie", "Yuki", "Louza", "Millie", "Nigel", "Ira", "Iris", "Izel", "Manuel", "Adalyn", "Aidan", "Hana", "Kanika"
        };

        private readonly string[] _lNameList = {
            "Smith", "Rodriguez", "Holland", "Jones", "Swift", "Brown", "Wilson", "Moore", "Taylor", "Ortega", "Kennedy", "Biden", "Kamala", "Harris"
        };

        private readonly List<Bitmap> _icons;

        public DataGenerator()
        {
            _icons = new List<Bitmap>();

            for (var i = 0; i < 10; i++)
            {
                var image = new Bitmap(64, 64);
                for (var x = 0; x < 64; x++)
                {
                    for (var y = 0; y < 64; y++)
                    {
                        image.SetPixel(x, y, Color.FromArgb(_rand.Next(256), _rand.Next(256), _rand.Next(256)));
                    }
                }
                _icons.Add(image);
            }
        }

        private string GetFirstName() => _fNameList[_rand.Next(_fNameList.Length)];

        private string GetLastName() => _lNameList[_rand.Next(_lNameList.Length)];

        private uint GetPIN() => (uint)_rand.Next(9999);

        private uint GetAcctNo() => (uint)_rand.Next(100000000, 999999999);

        private int GetBalance() => _rand.Next(-10000, 10000);

        private Bitmap GetIcon() => _icons[_rand.Next(_icons.Count)];

        public void GetNextAccount(out uint pin, out uint acctNo, out string firstName, out string
            lastName, out int balance, out Bitmap icon)
        {
            pin = GetPIN();
            acctNo = GetAcctNo();
            firstName = GetFirstName();
            lastName = GetLastName();
            balance = GetBalance();
            icon = GetIcon();
        }

        public int NumOAccts() => 100000;
    }
}
