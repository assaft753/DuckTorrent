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
        private ConfigDetails ConfigDetails;
        private List<Task> Tasks;
        private Thread thread;
        private int uploaderId;
        private Dictionary<int, TcpClient> ActivateUploads;

        public event StartUploading UploadStarted;
        public event FinishUploading UploadFinished;
        public event ErrorUploading UploadError;

        public Uploader(TcpListener tcpListener, XMLHandler xMLHandler, ConfigDetails configDetails)
        {
            TcpListener = tcpListener;
            XMLHandler = xMLHandler;
            ConfigDetails = configDetails;
            Tasks = new List<Task>();
            uploaderId = 0;
            ActivateUploads = new Dictionary<int, TcpClient>();
        }

        public void StartListening()
        {
            this.thread = new Thread(new ThreadStart(() =>
             {

                 while (1 == 1)
                 {
                     var tcpClient = this.TcpListener.AcceptTcpClient();
                     Task.Factory.StartNew(() => UploadHandler(tcpClient));
                 }

             }));
            this.thread.Start();
        }

        public void StopListening()
        {
            foreach (var keyval in this.ActivateUploads)
            {
                keyval.Value.Client.Close();
            }
            if (this.thread != null && this.thread.ThreadState == ThreadState.Running)
            {
                this.thread.Suspend();
            }
        }

        private void UploadHandler(TcpClient tcpClient)
        {
            int id = uploaderId;
            uploaderId++;
            try
            {
                ActivateUploads.Add(id, tcpClient);
                tcpClient.ReceiveTimeout = 5000;
                tcpClient.SendTimeout = 5000;
                var ip = tcpClient.Client.RemoteEndPoint.ToString();
                var networkStream = tcpClient.GetStream();

                StreamReader streamReader = new StreamReader(networkStream);

                var requestStr = streamReader.ReadLine();

                var requestChunk = XMLHandler.Deserialize<ChunkRequest>(requestStr);
                this.UploadStarted(requestChunk.FileName, requestChunk.ChunkSize.ToString(), ip, id);

                //Byte[] fileAsChunk = new Byte[requestChunk.ChunkSize];
                List<Byte> fileas = new List<byte>();
                var bytes = System.IO.File.ReadAllBytes(this.ConfigDetails.UploadPath + "\\" + requestChunk.FileName);
                for (int j = requestChunk.Offset; j < requestChunk.ChunkSize + requestChunk.Offset; j++)
                {
                    fileas.Add(bytes[j]);
                }

                /* using (BinaryReader reader = new BinaryReader(new FileStream(this.ConfigDetails.UploadPath + "\\" + requestChunk.FileName, FileMode.Open)))
                 {
                     reader.BaseStream.Seek((long)requestChunk.Offset, SeekOrigin.Begin);
                     reader.Read(fileAsChunk, 0, fileAsChunk.Length);
                 }
                 networkStream.Write(fileAsChunk, 0, fileAsChunk.Length);*/

                /* using (FileStream fileStream = new FileStream(this.ConfigDetails.UploadPath + "\\" + requestChunk.FileName, FileMode.Open, FileAccess.Read))
                 {
                     using (BinaryReader binaryReader = new BinaryReader(fileStream))
                     {
                         binaryReader.BaseStream.Seek((long)requestChunk.Offset, SeekOrigin.Begin);
                         binaryReader.Read(fileAsChunk, 0, fileAsChunk.Length);
                     }
                 }*/
                //networkStream.SetLength(bytes.Length);
                string result = Convert.ToBase64String(fileas.ToArray<Byte>());
                BinaryWriter writer = new BinaryWriter(networkStream);
                writer.Write(result);
                this.UploadFinished(id);
            }
            catch (Exception ex)
            {
                this.UploadError(id);
            }
            finally
            {
                this.ActivateUploads.Remove(id);
            }
        }
    }
}
