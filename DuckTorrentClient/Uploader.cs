using DuckTorrentClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DuckTorrentClient
{

    public delegate void UpdateUploads();
    public class Uploader
    {
        private TcpListener TcpListener;
        private XMLHandler XMLHandler;
        private Boolean StopListeningFlag;
        private ConfigDetails ConfigDetails;
        private Task Listener;
        private List<Task> Tasks;
        //public List<UploadDetails> UploadDetails;
        private Thread thread;

        public event UpdateUploads UpdateList;

        public Uploader(TcpListener tcpListener, XMLHandler xMLHandler, ConfigDetails configDetails)
        {
            TcpListener = tcpListener;
            XMLHandler = xMLHandler;
            StopListeningFlag = false;
            ConfigDetails = configDetails;
            /*UploadDetails = new List<UploadDetails>()
            {
                new DuckTorrentClient.UploadDetails("123", "aaa", 2, "completed"),
            new DuckTorrentClient.UploadDetails("123", "aaa", 2, "completed")
            };*/
            Tasks = new List<Task>();
        }

        public void StartListening()
        {
            //UpdateList();
            this.thread = new Thread(new ThreadStart(() =>
             {

                 while (StopListeningFlag == false)
                 {
                     var tcpClient = this.TcpListener.AcceptTcpClient();
                     Task.Factory.StartNew(() => UploadHandler(tcpClient));
                 }

             }));
            this.thread.Start();
        }

        public void StopListening()
        {
            this.thread.Suspend();
        }

        private void UploadHandler(TcpClient tcpClient)
        {
            var ip = tcpClient.Client.RemoteEndPoint.ToString();
            using (var networkStream = tcpClient.GetStream())
            {
                StreamReader streamReader = new StreamReader(networkStream);

                var requestStr = streamReader.ReadLine();
                MessageBox.Show(ip + " " + requestStr);

                var requestChunk = XMLHandler.Deserialize<ChunkRequest>(requestStr);
                var uploadDetails = new UploadDetails(ip, requestChunk.FileName, requestChunk.ChunkSize, "Uploading");
                //this.UploadDetails.Add(uploadDetails);
                this.UpdateList();

                Byte[] fileAsChunk = new Byte[requestChunk.ChunkSize];

                using (FileStream fileStream = new FileStream(this.ConfigDetails.UploadPath + "\\" + requestChunk.FileName, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader binaryReader = new BinaryReader(fileStream))
                    {
                        binaryReader.BaseStream.Seek((long)requestChunk.Offset, SeekOrigin.Begin);
                        binaryReader.Read(fileAsChunk, requestChunk.Offset, fileAsChunk.Length);
                    }
                }

                networkStream.Write(fileAsChunk, 0, fileAsChunk.Length);
                networkStream.Flush();
                uploadDetails.Status = "Completed";
                this.UpdateList();
            }
        }
    }
}
