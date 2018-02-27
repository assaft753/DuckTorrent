using DuckTorrentClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DuckTorrentClient
{
    public class DownloadTask
    {
        public Task<byte[]> DownloadHandler { get; set; }
        public TcpClient tcpClient { get; set; }
        public int ChunkSize { get; set; }
        public int initPos { get; set; }
        public FileSeed fileSeed { get; set; }

        public DownloadTask(TcpClient tcpClient, int chunkSize, int initPos, FileSeed fileSeed, Func<TcpClient, int, int, FileSeed, Byte[]> func)
        {
            this.tcpClient = tcpClient;
            this.ChunkSize = chunkSize;
            this.initPos = initPos;
            this.fileSeed = fileSeed;
            this.DownloadHandler = new Task<Byte[]>(() =>
             {
                 return func(this.tcpClient, this.ChunkSize, this.initPos, this.fileSeed);
             });
        }
    }
}
