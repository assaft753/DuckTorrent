using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckTorrentClasses
{
    public class IP
    {
        public string Ip { get; set; }
        public int Port { get; set; }

        public IP(string ip, int port)
        {
            Ip = ip;
            Port = port;
        }
        public IP() { }
    }
}
