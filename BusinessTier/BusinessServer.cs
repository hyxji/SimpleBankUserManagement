// Izhary Pauline Rodriguez Fortun
// 21486144
// Prac: Thursday 8 AM - 10 AM

using DBInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BusinessTier
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class BusinessServer : BusinessServerInterface
    {
        private uint LogNumber = 0;

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void Log(string logString)
        {
            LogNumber++; 
            string logMessage = $"Log #{LogNumber}: {logString} - Time: {DateTime.Now}";
            Console.WriteLine(logMessage);  
        }
        public int GetNumEntries()
        {
            ChannelFactory<DataServerInterface> channelFactory = new ChannelFactory<DataServerInterface>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:8100/DataService"));
            DataServerInterface dataServer = channelFactory.CreateChannel();
            Log($"Got total entries");

            return dataServer.GetNumEntries();
        }

        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out Bitmap icon)
        {
            ChannelFactory<DataServerInterface> channelFactory = new ChannelFactory<DataServerInterface>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:8100/DataService"));
            DataServerInterface dataServer = channelFactory.CreateChannel();

            dataServer.GetValuesForEntry(index, out acctNo, out pin, out bal, out fName, out lName, out icon);
            Log($"Got values for index entry");
        }

        public void GetValuesForSearch(string query, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out Bitmap icon)
        {
            ChannelFactory<DataServerInterface> channelFactory = new ChannelFactory<DataServerInterface>(
                new NetTcpBinding(),
                new EndpointAddress("net.tcp://localhost:8100/DataService")
            );
            DataServerInterface dataServer = channelFactory.CreateChannel();

            dataServer.GetValuesForSearch(query, out acctNo, out pin, out bal, out fName, out lName, out icon);
            Log($"Got values for search entry");
        }

    }
}
