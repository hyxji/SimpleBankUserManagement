// Izhary Pauline Rodriguez Fortun
// 21486144
// Prac: Thursday 8 AM - 10 AM

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BusinessTier
{
     class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Business Tier starting");
            var tcp = new NetTcpBinding();
            var host = new ServiceHost(typeof(BusinessServer));
            host.AddServiceEndpoint(typeof(BusinessServerInterface), tcp, "net.tcp://localhost:8101/BusinessService");
            host.Open();

            Console.WriteLine("Business Tier Online");
            Console.ReadLine();
            host.Close();
        }
    }
}
