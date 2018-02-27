using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckTorrentClient
{
    public class UploadDetails
    {
        public String IP { get; set; }
        public String FileName { get; set; }
        public int ChunkSize { get; set; }
        public String Status { get; set; }

        public UploadDetails(string iP, string fileName, int chunkSize, string status)
        {
            IP = iP;
            FileName = fileName;
            ChunkSize = chunkSize;
            Status = status;
        }
    }
}
