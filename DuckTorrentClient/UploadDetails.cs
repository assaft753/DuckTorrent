using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckTorrentClient
{
    public class UploadDetails
    {
        public String IP;
        public String FileName;
        public int ChunkSize;
        public String Status;

        public UploadDetails(string iP, string fileName, int chunkSize, string status)
        {
            IP = iP;
            FileName = fileName;
            ChunkSize = chunkSize;
            Status = status;
        }
    }
}
