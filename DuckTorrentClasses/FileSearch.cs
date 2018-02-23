using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckTorrentClasses
{
    public class FileSearch
    {
        public string FileName { get; set; }
        public UserDetails UserInfo { get; set; }

        public FileSearch(string fileName, UserDetails userInfo)
        {
            FileName = fileName;
            UserInfo = userInfo;
        }

        public FileSearch() { }
    }


}
