using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using HomeTransfer.model;

namespace HomeTransfer.controller
{
    class HomeTransferController
    {
        private static HomeTransferController controller;
        private HomeTransferServer homeTransferServer;
        private HomeTransferUDPServer homeTransferUDPServer;
        private Thread serverThread;
        private Thread udpServerThread;

        private HomeTransferController()
        {

        }

        public void initController()
        {
            try
            {
                homeTransferServer = new HomeTransferServer(HomeTransferModel.getInstance().getLocalHomeTransferData().port);
                homeTransferUDPServer = new HomeTransferUDPServer();
                serverThread = new Thread(new ThreadStart(homeTransferServer.run));
                udpServerThread = new Thread(new ThreadStart(homeTransferUDPServer.run));
                serverThread.Start();
                udpServerThread.Start();
            } catch(Exception e)
            {
                Console.WriteLine("Error: Server could not be created");
            }
        }

        public void exitController()
        {
            try
            {
                homeTransferServer.stop();
                homeTransferUDPServer.stop();
                serverThread.Abort();
                udpServerThread.Abort();
            } catch (Exception e)
            {
                Console.WriteLine("Error: Servers could not been closed");
            }
        }

        public static HomeTransferController getInstance()
        {
            if (controller == null)
            {
                Console.WriteLine("Controller is made");
                controller = new HomeTransferController();
            }
            return controller;
        }

        public void updateObserver()
        {
            Form1 form1 = Form1.getInstance() as Form1;
            if (form1 != null)
            {
                form1.updateGUI();
            }
        }

        public void refreshList()
        {
            HomeTransferModel.getInstance().deleteServers();
            broadcastUDP("discover");
            updateObserver();
        }

        public void deleteFiles()
        {
            HomeTransferModel.getInstance().deleteFiles();
            updateObserver();
        }

        public void broadcastUDP(String tag)
        {
            HomeTransferUDPClient.broadcastPackage(tag);
        }

        public static void sendFiles(String server, String[] files)
        {
            // TODO: Get the selected remote instance's IP and start a sender thread
            String[] str = server.Split('@');
            String IP = str[1];
            HomeTransferServerData data = HomeTransferModel.getInstance().getServerData(IP);
            HomeTransferClient sender = new HomeTransferClient(data.IP, data.port, files);
            Thread senderThread = new Thread(new ThreadStart(sender.run));
            senderThread.Start();
        }
    }
}
