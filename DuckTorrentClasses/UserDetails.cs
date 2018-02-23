using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckTorrentClasses
{
    public class UserDetails
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public UserDetails() { }

        public UserDetails(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
