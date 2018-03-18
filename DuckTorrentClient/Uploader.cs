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

        public event StartUploading UploadStarted;//EVENT FOR GUI FOR STARTED UPLOADING
        public event FinishUploading UploadFinished;//EVENT FOR GUI FOR FINISHED UPLOADING
        public event ErrorUploading UploadError;//EVENT FOR GUI FOR ERROR UPLOADING

        public Uploader(TcpListener tcpListener, XMLHandler xMLHandler, ConfigDetails configDetails)
        {
            TcpListener = tcpListener;
            XMLHandler = xMLHandler;
            ConfigDetails = configDetails;
            Tasks = new List<Task>();
            uploaderId = 0;
            ActivateUploads = new Dictionary<int, TcpClient>();
        }


        //OPEN THREAD FOR LISTENING TO CLIENTS
        public void StartListening()
        {
            this.thread = new Thread(new ThreadStart(() =>
             {
                 try
                 {
                     while (1 == 1)
                     {
                         var tcpClient = this.TcpListener.AcceptTcpClient();
                         Task.Factory.StartNew(() => UploadHandler(tcpClient));
                     }
                 }
                 catch
                 {

                 }

             }));
            this.thread.IsBackground = true;
            this.thread.Start();
        }


        //CLOSE THREAD THAT LISTENING TO CLIENTS
        public void StopListening()
        {
            foreach (var keyval in this.ActivateUploads)
            {
                keyval.Value.Client.Close();
            }
            if (this.thread != null && this.thread.ThreadState == ThreadState.Running)
            {
                this.thread.Abort();
            }
        }

        //FUNCTION THAT HANDLES THE UPLOAD TASKS
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

                List<Byte> fileas = new List<byte>();
                var bytes = System.IO.File.ReadAllBytes(this.ConfigDetails.UploadPath + "\\" + requestChunk.FileName);

                for (int j = requestChunk.Offset; j < requestChunk.ChunkSize + requestChunk.Offset; j++)
                {
                    fileas.Add(bytes[j]);
                }

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
