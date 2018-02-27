using DuckTorrentClasses;
using DuckTorrentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.ServiceModel;
using DuckTorrentClient;

namespace ClientApplication
{

    public delegate void CloseConnections();
    public delegate void StartDownloading(String FileName, String Size, String Sources);
    public delegate void FinishDownload(String Speed, String TimePassed, String fileName);
    public partial class MainWindow : Window
    {
        private XMLHandler xMLHandler = new XMLHandler();
        private ConfigDetails ConfigDetails;
        private TcpListener TcpListener;
        private IDuckTorrentServerApi ServerProxy;
        private Uploader Uploader;
        private List<UploadDetails> UploadList;
        private List<DownloadView> downloadView;
        public event CloseConnections CloseConnectionEvent;
        public Downloader downloader;

        List<UploadDetails> uploadDetails = new List<UploadDetails>()
        {
            new UploadDetails("123","aaa",2,"completed"),
            new UploadDetails("123","aaa",2,"completed")
        };

        public List<UploadDetails> UploadDetails { get => uploadDetails; set => uploadDetails = value; }

        public MainWindow(ConfigDetails configDetails, IDuckTorrentServerApi serverProxy, TcpListener tcpListener)
        {
            try
            {
                InitializeComponent();
                this.ConfigDetails = configDetails;
                this.ServerProxy = serverProxy;
                this.TcpListener = tcpListener;

                this.Uploader = new Uploader(this.TcpListener, this.xMLHandler, this.ConfigDetails);
                this.downloader = new Downloader(this.ConfigDetails, this.xMLHandler);

                this.Uploader.UpdateList += UpdateUploadList;
                this.downloader.DownloadStarted += StartDownloading;
                this.downloader.DownloadFinished += FinishDownlading;

                this.downloadView = new List<DownloadView>();
                this.listView_Downloads.ItemsSource = this.downloadView;
                this.listView_Uploads.ItemsSource = UploadDetails;//
                this.Uploader.StartListening();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FinishDownlading(string Speed, string TimePassed, String fileName)
        {
            foreach (var download in downloadView)
            {
                if (download.FileName.Equals(fileName) == true && download.Status.Equals("Downloading") == true)
                {
                    download.Status = "Completed";
                    download.Speed = Speed + " KBps";
                    download.TimePassed = TimePassed + " Seconds";
                }
                this.listView_Downloads.Dispatcher.Invoke(new CloseConnections(this.Test));
            }
        }

        private void UpdateUploadList()
        {
            //this.listView_Uploads.Dispatcher.Invoke(new CloseConnections(this.Test));
        }

        private void Test()
        {
            this.listView_Downloads.Items.Refresh();
            //this.listView_Uploads.Items.Refresh();
        }

        private void listViewResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FileSeed fileSeed = (FileSeed)this.listView_Results.SelectedItem;
            Task.Factory.StartNew(() => this.downloader.StartDownloading(fileSeed));
        }

        private void listView_Downloads_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void search_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (this.file_name_textBox.Text.Trim().Count() > 0)
            {
                var search = new FileSearch(this.file_name_textBox.Text, this.ConfigDetails.User);
                var filesAsString = this.ServerProxy.SearchFile(this.xMLHandler.Serialize<FileSearch>(search));
                if (filesAsString != null)
                {
                    var seeds = this.xMLHandler.Deserialize<List<FileSeed>>(filesAsString);
                    this.listView_Results.ItemsSource = seeds;
                }
                else
                {
                    this.listView_Results.ItemsSource = null;
                }
            }
        }

        private void logOut_Btn_Click(object sender, RoutedEventArgs e)
        {
            System.IO.File.Delete(Login.CONFIGFILE);
            LogOutUser();

        }

        private void LogOutUser()
        {
            try
            {
                var respond = ServerProxy.SignOut(this.xMLHandler.Serialize<UserDetails>(this.ConfigDetails.User));
                if (CheckIfErrorFromServer(respond) == false)
                {
                    throw new Exception(respond);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Close();
            }
        }

        private Boolean CheckIfErrorFromServer(string serverRespond)
        {
            string answerCode = "";
            foreach (Char c in serverRespond.Take(3))
            {
                answerCode += c;
            }
            if (answerCode.Equals(Login.BAD))
            {
                return false;
            }
            return true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Uploader.StopListening();
            this.CloseConnectionEvent();
        }

        private void StartDownloading(String fileName, String size, String sources)
        {
            this.downloadView.Add(new DownloadView()
            {
                FileName = fileName,
                Status = "Downloading",
                Size = size + " Bytes",
                Sources = sources + " Seeds"
            });
            this.listView_Downloads.Dispatcher.Invoke(new CloseConnections(this.Test));
        }

        private void listView_Uploads_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(sender.ToString());
        }
    }
}
