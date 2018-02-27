using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckTorrentClient
{
    class DownloadView
    {
        public string FileName { get; set; }
        public string Status { get; set; }
        public string Size { get; set; }// + "Bytes"; }
        public string Speed { get; set; }
        public string TimePassed { get; set; }
        public string Sources { get; set; }
    }
}
