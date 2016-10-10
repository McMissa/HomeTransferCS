using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HomeTransfer.controller
{
    class HomeTransferServer
    {
        private Boolean running;
        private TcpListener server;

        public HomeTransferServer(int port)
        {
            server = new TcpListener(IPAddress.Any, port);
        }

        public void stop()
        {
            running = false;
            server.Stop();
        }

        public void run()
        {
            running = true;
            server.Start();

            while (running)
            {
                try
                {
                    Int64 bytesReceived = 0;
                    int count;
                    var fileNameBuffer = new byte[1 << 8];
                    var buffer = new byte[1024 * 8];

                    TcpClient tcpClient = server.AcceptTcpClient();
                    NetworkStream ns = tcpClient.GetStream();
                    StreamReader sr = new StreamReader(ns);
                    int numberOfBytes = 1 << 20;

                    int bytesRead = ns.Read(fileNameBuffer, 0, fileNameBuffer.Length);
                    String fileName = System.Text.Encoding.UTF8.GetString(fileNameBuffer);
//                    fileName = fileName.Substring(0, bytesRead); not necessary here when everything is filled with \0
//                    fileName = fileName.Trim(); not necessary here when everything is filled with \0
                    fileName = fileName.Replace("\0", String.Empty);

                    using (var fileIO = File.Create(fileName))
                    {
                        while ((count = ns.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fileIO.Write(buffer, 0, count);
                            bytesReceived += count;
                        }
                    }
                } catch (Exception e)
                {
                    Console.WriteLine("TCP listening finished.");
                }
            }
        }
    }
}
