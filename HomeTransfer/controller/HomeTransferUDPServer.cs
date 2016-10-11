using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using HomeTransfer.model;

namespace HomeTransfer.controller
{
    class HomeTransferUDPServer
    {
        private Boolean running;
        private UdpClient udpClient;

        public HomeTransferUDPServer()
        {
            udpClient = new UdpClient(1138);
        }

        public void stop()
        {
            running = false;
            udpClient.Close();
        }

        public void run()
        {
            running = true;

            while (running)
            {
                try
                {
                    IPEndPoint RemoteIPEndPoint = new IPEndPoint(IPAddress.Any, 1138);
                    Byte[] receiveBytes = udpClient.Receive(ref RemoteIPEndPoint);

                    HomeTransferServerData data = HomeTransferServerData.createFromUDP(receiveBytes);
                    if (data.type.Equals("close"))
                    {
                        HomeTransferModel.getInstance().removeServer(data);
                    }
                    else
                    {
                        HomeTransferModel.getInstance().addServer(data);
                        if (data.type.Equals("discover"))
                        {
                            HomeTransferController.getInstance().broadcastUDP("response");
                        }
                    }
                    HomeTransferController.getInstance().updateObserver();
                }
                catch (Exception e)
                {
                    Console.WriteLine("UDP listening finished.");
                }
            }
        }
    }
}
