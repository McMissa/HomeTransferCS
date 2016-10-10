using HomeTransfer.controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

/*
 * Changes made regarding the java version:
 * - Hashmap is replaced by Dictionary
 * - File objects are replaced by Strings
 */

namespace HomeTransfer.model
{
    class HomeTransferModel
    {

        private static HomeTransferModel homeTransferModel;
        private static Dictionary<String, HomeTransferServerData> homeTransferServerDictionary;
        private static HomeTransferServerData localData;
        private static Dictionary<String, String> homeTransferFileDictionary;

        private HomeTransferModel()
        {
            homeTransferServerDictionary = new Dictionary<String, HomeTransferServerData>();
            homeTransferFileDictionary = new Dictionary<String, String>();

            String name = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            String IP = "";
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        IP = ip.ToString();
                        break;
                    }
                }
            } catch(Exception e)
            {
                Console.WriteLine("Error: Local host not available");
            }

            String port = "11" + IP.Substring(IP.Length -2);
            port = port.Replace('.', '0');

            localData = new HomeTransferServerData();
            localData.name = name;
            localData.IP = IP;
            localData.port = Convert.ToInt32(port);
        }

        public static HomeTransferModel getInstance()
        {
            if (homeTransferModel == null)
            {
                homeTransferModel = new HomeTransferModel();
            }
            return homeTransferModel;
        }

        public void addServer(HomeTransferServerData data)
        {
            if ( !(homeTransferServerDictionary.ContainsKey(data.IP) || data.IP.Equals(localData.IP)) )
            {
                homeTransferServerDictionary.Add(data.IP, data);
                HomeTransferController.getInstance().updateObserver();
            }
        }

        public void removeServer(HomeTransferServerData data)
        {
            if (homeTransferServerDictionary.ContainsKey(data.IP))
            {
                homeTransferServerDictionary.Remove(data.IP);
            }
        }

        private void addDirectory(String directory)
        {
            // TODO: Implement recursive adding of directories
        }

        public void addFiles(String[] files)
        {
            Boolean changed = false;
            foreach(String file in files)
            {
                if (!homeTransferFileDictionary.ContainsKey(file))
                {
                    homeTransferFileDictionary.Add(file, file);
                    changed = true;
                }
            }
            if (changed)
            {
                HomeTransferController.getInstance().updateObserver();
            }
            // TODO: Implement adding of file arrays
        }

        public void deleteServers() {
            homeTransferServerDictionary.Clear();
        }

        public void deleteFiles()
        {
            homeTransferFileDictionary.Clear();
        }

        public String[] getServers()
        {
            String[] servers = new String[homeTransferServerDictionary.Count()];
            int index = 0;
            foreach(KeyValuePair<String, HomeTransferServerData> pair in homeTransferServerDictionary)
            {
                servers[index++] = pair.Value.name + "@" + pair.Value.IP;
            }
            return servers;
        }

        public String[] getFiles()
        {
            String[] files = new String[homeTransferFileDictionary.Count];
            int index = 0;
            foreach(KeyValuePair<String, String> pair in homeTransferFileDictionary)
            {
                files[index] = pair.Value;
                index++;
            }
            return files;
        }

        public HomeTransferServerData getServerData(String IP)
        {
            return homeTransferServerDictionary[IP];
        }

        public String getFile(String name)
        {
            return homeTransferFileDictionary[name];
        }

        public HomeTransferServerData getLocalHomeTransferData()
        {
            return localData;
        }
    }
}
