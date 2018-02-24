using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DuckTorrentClasses
{
    public class ConfigDetails
    {
        [XmlIgnore]
        private static readonly string STARTIPADD = "http://";
        [XmlIgnore]
        private static readonly string ENDIPADD = ":8090/ServerApi";

        public UserDetails User { get; set; }
        public int Port { get; set; }
        public string ServerIP { get; set; }
        public string DownloadPath { get; set; }
        public string UploadPath { get; set; }

        public ConfigDetails(UserDetails user, int port, string serverIP, string downloadPath, string uploadPath)
        {
            User = user;
            Port = port;
            ServerIP = serverIP;
            DownloadPath = downloadPath;
            UploadPath = uploadPath;
        }

        public ConfigDetails() { }

        public string GenerateServerIPAdress()
        {
            return STARTIPADD + this.ServerIP + ENDIPADD;
        }
    }
}
