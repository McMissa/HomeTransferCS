using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HomeTransfer.controller
{
    class HomeTransferClient
    {
        public static Boolean running;
        private String IP;
        private int port;
        private String[] files;
        private const int NUMBER_OF_BYTES = 1 << 20;
        private const double MB_PER_ROUND = NUMBER_OF_BYTES / 1000000.0;
        private const Boolean REALTIME = false;
        private const int UPDATE_TIME_MILIS = 5000;

        public HomeTransferClient(String IP, int port, String[] files)
        {
            this.IP = IP;
            this.port = port;
            this.files = files;
        }

        public void run()
        {
            Console.WriteLine("Send dat shit");
            var buffer = new byte[NUMBER_OF_BYTES];

            for (int i=0; i<files.Length; i++)
            {
                TcpClient tcpClient = new TcpClient(IP, port);

                using (StreamReader sr = new StreamReader(files[i]))
                {
                    NetworkStream ns = tcpClient.GetStream();
                    StreamWriter sw = new StreamWriter(ns);

                    char[] filename = new char[1 << 8];
                    //filename = Encoding.ASCII.GetBytes(files[i]);
                    String[] fileNameSplit = files[i].Split('\\');
                    String fileNameClean = fileNameSplit[fileNameSplit.Length - 1];
                    filename = new char[fileNameClean.Length * sizeof(char)];
                    System.Buffer.BlockCopy(fileNameClean.ToCharArray(), 0, filename, 0, filename.Length);

                    //sw.Write(filename, 0, filename.Length);
                    //sw.Flush();

                    byte[] fileNameBytes = new byte[1 << 8];
                    System.Buffer.BlockCopy(Encoding.UTF8.GetBytes(fileNameClean), 0, fileNameBytes, 0, Encoding.UTF8.GetBytes(fileNameClean).Length);
//                    byte[] fileNameBytes = Encoding.UTF8.GetBytes(fileNameClean);
                    ns.Write(fileNameBytes, 0, fileNameBytes.Length);
                    ns.Flush();

                    //System.Threading.Thread.Sleep(50);

                    using (var fileIO = File.OpenRead(files[i]))
                    {
                        var buffer2 = new byte[NUMBER_OF_BYTES];
                        int count;
                        while ((count = fileIO.Read(buffer2, 0, buffer2.Length)) > 0)
                        {
                            ns.Write(buffer2, 0, count);
                        }
                    }

                    ns.Flush();
                    sw.Close();
                    ns.Close();
                    tcpClient.Close();
                }

            }
        }
    }

}
