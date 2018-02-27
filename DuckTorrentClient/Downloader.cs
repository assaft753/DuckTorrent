using ClientApplication;
using DuckTorrentClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DuckTorrentClient
{
    public class Downloader
    {
        Dictionary<String, FileSeed> Downloading;
        ConfigDetails ConfigDetails;
        XMLHandler xMLHandler;
        public event StartDownloading DownloadStarted;
        public event FinishDownload DownloadFinished;



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
                var DownloadTasks = new List<DownloadTask>();
                this.Downloading.Add(fileSeed.FileName, fileSeed);
                //List<Task<Byte[]>> seedsTask = new List<Task<Byte[]>>();
                int ChunkSize = (int)fileSeed.Size / fileSeed.Seeds.Count;
                int leftover = (int)fileSeed.Size % fileSeed.Seeds.Count;

                List<TcpClient> tcpClientsList = new List<TcpClient>();
                int currentPos = 0;

                for (int i = 0; i < fileSeed.Seeds.Count; i++)
                {
                    TcpClient tcpClient = new TcpClient(fileSeed.Seeds[i].Ip, fileSeed.Seeds[i].Port);
                    tcpClientsList.Add(tcpClient);
                }


                for (int j = 0; j < fileSeed.Seeds.Count; j++)
                {
                    if (j == fileSeed.Seeds.Count - 1)
                    {
                        DownloadTasks.Add(new DownloadTask(tcpClientsList[j], ChunkSize + leftover, currentPos, fileSeed, this.SinglePartDownload));
                        DownloadTasks[j].DownloadHandler.Start();
                    }
                    else
                    {
                        DownloadTasks.Add(new DownloadTask(tcpClientsList[j], ChunkSize, currentPos, fileSeed, this.SinglePartDownload));
                        DownloadTasks[j].DownloadHandler.Start();
                        currentPos += ChunkSize;
                    }
                }

                this.DownloadStarted(fileSeed.FileName, fileSeed.Size.ToString(), fileSeed.Seeds.Count.ToString());
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                using (FileStream fileStream = new FileStream(this.ConfigDetails.DownloadPath + "\\" + fileSeed.FileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    for (int x = 0; x < DownloadTasks.Count; x++)
                    {
                        DownloadTasks[x].DownloadHandler.Wait();
                        var data = DownloadTasks[x].DownloadHandler.Result;
                        BinaryWriter binaryWriter = new BinaryWriter(fileStream);
                        binaryWriter.Write(data, 0, data.Length);
                    }
                }
                stopWatch.Stop();
                var time = (float)stopWatch.ElapsedMilliseconds / 1000;
                var speed = ((float)fileSeed.Size / 1000) / time;
                this.Downloading.Remove(fileSeed.FileName);
                System.IO.File.Copy(this.ConfigDetails.DownloadPath + "\\" + fileSeed.FileName, this.ConfigDetails.UploadPath + "\\" + fileSeed.FileName, true);
                this.DownloadFinished(speed.ToString(), time.ToString(), fileSeed.FileName);
                //update main for finish
            }
        }

        private Byte[] SinglePartDownload(TcpClient tcpClient, int ChunkSize, int initPos, FileSeed fileSeed)
        {
            MessageBox.Show(tcpClient.Client.LocalEndPoint.ToString());
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
