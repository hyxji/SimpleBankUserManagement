// Izhary Pauline Rodriguez Fortun
// 21486144
// Prac: Thursday 8 AM - 10 AM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using DBInterface;

namespace DBServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Data Server starting");
            var tcp = new NetTcpBinding();
            var host = new ServiceHost(typeof(DataServer));
            host.AddServiceEndpoint(typeof(DataServerInterface), tcp, "net.tcp://localhost:8100/DataService");
            host.Open();

            Console.WriteLine("Data Server Online");
            Console.ReadLine();
            host.Close();
        }
    }

}
