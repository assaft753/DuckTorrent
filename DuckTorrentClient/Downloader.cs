using DuckTorrentClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DuckTorrentClient
{
    public class Downloader
    {
        Dictionary<String, FileSeed> Downloading;
        ConfigDetails ConfigDetails;
        XMLHandler xMLHandler;

        public Downloader(ConfigDetails configDetails, XMLHandler xMLHandler)
        {
            ConfigDetails = configDetails;
            this.xMLHandler = xMLHandler;
            Downloading = new Dictionary<string, FileSeed>();
        }

        public void StartDownloading(FileSeed fileSeed)
        {
            if (this.Downloading.ContainsKey(fileSeed.FileName) == false)
            {
                this.Downloading.Add(fileSeed.FileName, fileSeed);
                List<Task<Byte[]>> seedsTask = new List<Task<Byte[]>>();
                int ChunkSize = (int)fileSeed.Size / fileSeed.Seeds.Count;
                int leftover = (int)fileSeed.Size % fileSeed.Seeds.Count;
                int currentPos = 0;
                for (int i = 0; i < fileSeed.Seeds.Count; i++)
                {
                    TcpClient tcpClient = new TcpClient(fileSeed.Seeds[i].Ip, fileSeed.Seeds[i].Port);
                    if (i == fileSeed.Seeds.Count - 1)
                    {
                        seedsTask.Add(new Task<Byte[]>(() => this.SinglePartDownload(tcpClient, ChunkSize + leftover, currentPos, fileSeed)));
                    }
                    else
                    {
                        seedsTask.Add(new Task<Byte[]>(() => this.SinglePartDownload(tcpClient, ChunkSize, currentPos, fileSeed)));
                        currentPos += ChunkSize;
                    }
                }
                //update main for start
                foreach (var task in seedsTask)
                {
                    task.Start();
                }
                using (FileStream fileStream = new FileStream(this.ConfigDetails.DownloadPath + "\\" + fileSeed.FileName, FileMode.Create, FileAccess.Write))
                {
                    for (int i = 0; i < seedsTask.Count; i++)
                    {
                        seedsTask[i].Wait();
                        var data = seedsTask[i].Result;
                        using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                        {
                            binaryWriter.Write(data, 0, data.Length);
                        }
                    }
                }
                this.Downloading.Remove(fileSeed.FileName);
                System.IO.File.Copy(this.ConfigDetails.DownloadPath + "\\" + fileSeed.FileName, this.ConfigDetails.UploadPath + "\\" + fileSeed.FileName, true);
                //update main for finish
            }
        }

        private Byte[] SinglePartDownload(TcpClient tcpClient, int ChunkSize, int initPos, FileSeed fileSeed)
        {
            using (NetworkStream networkStream = tcpClient.GetStream())
            {
                using (StreamWriter sw = new StreamWriter(networkStream))
                {
                    ChunkRequest chunkRequest = new ChunkRequest(fileSeed.FileName, initPos, ChunkSize);
                    var xmlString = this.xMLHandler.Serialize<ChunkRequest>(chunkRequest);
                    var xmlStringRepair = xmlString.Replace('\n', ' ').Replace('\r', ' ').Trim();
                    sw.WriteLine(xmlStringRepair);
                    sw.Flush();
                    var data = new byte[ChunkSize];
                    networkStream.Read(data, 0, data.Length);
                    return data;
                }
            }
        }
    }
}
