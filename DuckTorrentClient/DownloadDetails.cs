using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckTorrentClient
{
    public class DownloadDetails
    {
        public byte[] Data { get; set; }
        public int ChunkSize { get; set; }
        public int InitPos { get; set; }

        public DownloadDetails(byte[] data, int chunkSize, int initPos)
        {
            Data = data;
            ChunkSize = chunkSize;
            InitPos = initPos;
        }
    }
}
