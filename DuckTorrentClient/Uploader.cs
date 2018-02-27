using ClientApplication;
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
        private List<Task> Tasks;
        private Thread thread;
        private int uploaderId;

        public event StartUploading UploadStarted;
        public event FinishUploading UploadFinished;
        public event ErrorUploading UploadError;

        public Uploader(TcpListener tcpListener, XMLHandler xMLHandler, ConfigDetails configDetails)
        {
            TcpListener = tcpListener;
            XMLHandler = xMLHandler;
            StopListeningFlag = false;
            ConfigDetails = configDetails;
            Tasks = new List<Task>();
            uploaderId = 0;
        }

        public void StartListening()
        {
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
            int id = uploaderId;
            uploaderId++;
            try
            {
                var ip = tcpClient.Client.RemoteEndPoint.ToString();
                using (var networkStream = tcpClient.GetStream())
                {
                    StreamReader streamReader = new StreamReader(networkStream);

                    var requestStr = streamReader.ReadLine();

                    var requestChunk = XMLHandler.Deserialize<ChunkRequest>(requestStr);
                    this.UploadStarted(requestChunk.FileName, requestChunk.ChunkSize.ToString(), ip, id);

                    Byte[] fileAsChunk = new Byte[requestChunk.ChunkSize];

                    using (BinaryReader reader = new BinaryReader(new FileStream(this.ConfigDetails.UploadPath + "\\" + requestChunk.FileName, FileMode.Open)))
                    {
                        reader.BaseStream.Seek((long)requestChunk.Offset, SeekOrigin.Begin);
                        reader.Read(fileAsChunk, 0, fileAsChunk.Length);
                    }
                    networkStream.Write(fileAsChunk, 0, fileAsChunk.Length);

                    /* using (FileStream fileStream = new FileStream(this.ConfigDetails.UploadPath + "\\" + requestChunk.FileName, FileMode.Open, FileAccess.Read))
                     {
                         using (BinaryReader binaryReader = new BinaryReader(fileStream))
                         {
                             binaryReader.BaseStream.Seek((long)requestChunk.Offset, SeekOrigin.Begin);
                             binaryReader.Read(fileAsChunk, 0, fileAsChunk.Length);
                         }
                     }

                     networkStream.Write(fileAsChunk, 0, fileAsChunk.Length);
                     networkStream.Flush();*/
                    this.UploadFinished(id);
                }
            }
            catch
            {
                MessageBox.Show("error");
                this.UploadError(id);
            }
        }
    }
}
