using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeTransfer.model
{
    class HomeTransferServerData
    {
        public String name;
        public String IP;
        public int port;
        public String type;

        public static HomeTransferServerData createFromUDP(Byte[] packet)
        {
            HomeTransferServerData data = new HomeTransferServerData();
            String packetData = System.Text.Encoding.Default.GetString(packet).Trim();
            String[] packetDataSplitted = packetData.Split(';');
            data.name = packetDataSplitted[0];
            data.IP = packetDataSplitted[1];
            data.port = Convert.ToInt32(packetDataSplitted[2]);
            data.type = packetDataSplitted[3];
            return data;
        }
    }
}
