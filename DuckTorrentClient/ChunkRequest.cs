using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckTorrentClient
{
    public class ChunkRequest
    {
        public string FileName { get; set; }
        public int Offset { get; set; }
        public int ChunkSize { get; set; }
        public long FileSize { get; set; }

        public ChunkRequest(string fileName, int offset, int chunkSize, int fileSize)
        {
            FileName = fileName;
            Offset = offset;
            ChunkSize = chunkSize;
            FileSize = fileSize;
        }

        public ChunkRequest() { }
    }
}
