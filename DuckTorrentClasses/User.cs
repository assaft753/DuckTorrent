using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckTorrentClasses
{
    public class User
    {
        public UserDetails UserInfo { get; set; }
        public List<File> Files { get; set; }
        public int Port { get; set; }
        public String Ip { get; set; }

        public User(UserDetails userDetails, List<File> files, int port, string ip)
        {
            UserInfo = userDetails;
            Files = files;
            Port = port;
            Ip = ip;
        }
        public User() { }
    }
}
