using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using HomeTransfer.model;

namespace HomeTransfer.controller
{

    class HomeTransferUDPClient
    {
        public static void broadcastPackage(String tag)
        {
            HomeTransferModel model = HomeTransferModel.getInstance();
            HomeTransferServerData local = model.getLocalHomeTransferData();

            UdpClient udpClient = new UdpClient();
            udpClient.Connect("255.255.255.255", 1138);
            Byte[] senddata = Encoding.ASCII.GetBytes(local.name + ";" + local.IP + ";" + local.port + ";" + tag);
            udpClient.Send(senddata, senddata.Length);
        }
    }
}
