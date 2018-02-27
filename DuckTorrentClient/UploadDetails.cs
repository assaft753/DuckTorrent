using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckTorrentClient
{
    public class UploadView
    {
        public String IP { get; set; }
        public String FileName { get; set; }
        public string ChunkSize { get; set; }
        public String Status { get; set; }
        public int Id { get; set; }

        public UploadView() { }

    }
}
