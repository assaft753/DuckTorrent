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
        public event FinishDownloading DownloadFinished;
        public event ErrorDownloading DownloadError;



        public Downloader(ConfigDetails configDetails, XMLHandler xMLHandler)
        {
            ConfigDetails = configDetails;
            this.xMLHandler = xMLHandler;
            Downloading = new Dictionary<string, FileSeed>();
        }

        public void StartDownloading(FileSeed fileSeed)
        {
            try
            {
                if (this.Downloading.ContainsKey(fileSeed.FileName) == false)
                {
                    this.DownloadStarted(fileSeed.FileName, fileSeed.Size.ToString(), fileSeed.Seeds.Count.ToString());
                    var DownloadTasks = new List<DownloadTask>();
                    this.Downloading.Add(fileSeed.FileName, fileSeed);
                    int ChunkSize = (int)fileSeed.Size / fileSeed.Seeds.Count;
                    int leftover = (int)fileSeed.Size % fileSeed.Seeds.Count;

                    List<TcpClient> tcpClientsList = new List<TcpClient>();
                    int currentPos = 0;

                    for (int i = 0; i < fileSeed.Seeds.Count; i++)
                    {

                        TcpClient tcpClient = new TcpClient(fileSeed.Seeds[i].Ip, fileSeed.Seeds[i].Port);
                        //tcpClient.ReceiveTimeout = 1000;
                        //tcpClient.SendTimeout = 1000;
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


                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();

                    if (System.IO.File.Exists(this.ConfigDetails.DownloadPath + "\\" + fileSeed.FileName))
                    {
                        System.IO.File.Delete(this.ConfigDetails.DownloadPath + "\\" + fileSeed.FileName);
                    }

                    using (FileStream fileStream = new FileStream(this.ConfigDetails.DownloadPath + "\\" + fileSeed.FileName, FileMode.Create, FileAccess.Write))
                    {
                        BinaryWriter binaryWriter = new BinaryWriter(fileStream);
                        for (int x = 0; x < DownloadTasks.Count; x++)
                        {
                            Task.WaitAll(DownloadTasks[x].DownloadHandler);
                            var data = DownloadTasks[x].DownloadHandler.Result;
                            if (data == null)
                            {
                                throw new Exception();
                            }
                            binaryWriter.Write(data, 0, data.Length);
                        }

                    }
                    stopWatch.Stop();
                    var time = (float)stopWatch.ElapsedMilliseconds / 1000;
                    var speed = ((float)fileSeed.Size / 1000) / time;
                    this.Downloading.Remove(fileSeed.FileName);
                    System.IO.File.Copy(this.ConfigDetails.DownloadPath + "\\" + fileSeed.FileName, this.ConfigDetails.UploadPath + "\\" + fileSeed.FileName, true);
                    this.DownloadFinished(speed.ToString(), time.ToString(), fileSeed.FileName);
                }
            }
            catch
            {
                this.DownloadError(fileSeed.FileName);
                this.Downloading.Remove(fileSeed.FileName);
            }
        }


        private Byte[] SinglePartDownload(TcpClient tcpClient, int ChunkSize, int initPos, FileSeed fileSeed)
        {
            try
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
                        //var data = new byte[ChunkSize];
                        //networkStream.Read(data, 0, data.Length);
                        BinaryReader reader = new BinaryReader(networkStream);
                        var strdata = reader.ReadString();
                        var data = Convert.FromBase64String(strdata);
                        return data;
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
