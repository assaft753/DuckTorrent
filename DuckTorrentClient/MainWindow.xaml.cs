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
using System.Net;
using System.Reflection;
using Animals;

namespace ClientApplication
{

    public delegate void RefreshDownloadList();
    public delegate void StartDownloading(String FileName, String Size, String Sources);
    public delegate void FinishDownloading(String Speed, String TimePassed, String fileName);
    public delegate void ErrorDownloading(String fileName);

    public delegate void RefreshUploadList();
    public delegate void StartUploading(String fileName, String size, String iP, int id);
    public delegate void FinishUploading(int id);
    public delegate void ErrorUploading(int id);

    public delegate void CloseConnection();

    public delegate void ReflectDLL(string fileName);


    public partial class MainWindow : Window
    {
        private XMLHandler xMLHandler = new XMLHandler();
        private ConfigDetails ConfigDetails;
        private TcpListener TcpListener;
        private IDuckTorrentServerApi ServerProxy;
        private List<UploadView> uploadView;
        private List<DownloadView> downloadView;
        public Downloader downloader;
        private Uploader Uploader;
        private Boolean IsEnable;
        public event CloseConnection CloseConnectionEvent;

        public MainWindow(ConfigDetails configDetails, IDuckTorrentServerApi serverProxy, TcpListener tcpListener, Boolean isEnable)
        {
            try
            {
                InitializeComponent();
                this.IsEnable = isEnable;
                this.ConfigDetails = configDetails;
                this.ServerProxy = serverProxy;
                this.TcpListener = tcpListener;

                this.Uploader = new Uploader(this.TcpListener, this.xMLHandler, this.ConfigDetails);
                this.downloader = new Downloader(this.ConfigDetails, this.xMLHandler);

                this.downloader.DownloadStarted += StartDownloading;
                this.downloader.DownloadFinished += FinishDownlading;
                this.downloader.DownloadError += ErrorDownloading;
                this.downloader.ReflectDLL += DllHandler;

                this.Uploader.UploadStarted += StartUploading;
                this.Uploader.UploadFinished += FinishUploading;
                this.Uploader.UploadError += ErrorUploading;

                this.downloadView = new List<DownloadView>();
                this.uploadView = new List<UploadView>();
                this.listView_Downloads.ItemsSource = this.downloadView;
                this.listView_Uploads.ItemsSource = uploadView;
                if (isEnable == true)
                {
                    this.Uploader.StartListening();
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void DllHandler(string fileName)
        {
            Assembly assembly = Assembly.LoadFrom(this.ConfigDetails.DownloadPath + "\\" + fileName);
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                AnimalAttribute animal = (AnimalAttribute)Attribute.GetCustomAttribute(type, typeof(AnimalAttribute));
                if (animal != null)
                {
                    if (animal.Kind.Equals("Dog"))
                    {
                        object[] parameters = { "DogiDogo", "Black" };
                        object obj = Activator.CreateInstance(type, parameters);
                        MethodInfo m = type.GetMethod("Print");
                        MessageBox.Show((m.Invoke(obj, null)).ToString());
                    }

                    else if (animal.Kind.Equals("Duck"))
                    {
                        object[] parameters = { "DonaldDuck", "White" };
                        object obj = Activator.CreateInstance(type, parameters);
                        MethodInfo m = type.GetMethod("Print");
                        MessageBox.Show((m.Invoke(obj, null).ToString()));
                    }
                }
            }
        }

        private void ErrorUploading(int id)
        {
            foreach (var upload in uploadView)
            {
                if (upload.Id == id && upload.Status.Equals("Uploading") == true)
                {
                    upload.Status = "Error";
                    break;
                }
            }
            this.listView_Uploads.Dispatcher.Invoke(new RefreshUploadList(this.RefreshUploadListView));
        }

        private void StartUploading(String fileName, String size, String iP, int id)
        {
            this.uploadView.Add(new UploadView()
            {
                FileName = fileName,
                Status = "Uploading",
                ChunkSize = size + " Bytes",
                IP = iP,
                Id = id
            });
            this.listView_Uploads.Dispatcher.Invoke(new RefreshUploadList(this.RefreshUploadListView));

        }

        private void FinishUploading(int id)
        {
            foreach (var upload in uploadView)
            {
                if (upload.Id == id && upload.Status.Equals("Uploading") == true)
                {
                    upload.Status = "Completed";
                    break;
                }
            }
            this.listView_Uploads.Dispatcher.Invoke(new RefreshUploadList(this.RefreshUploadListView));
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
                this.listView_Downloads.Dispatcher.Invoke(new RefreshDownloadList(this.RefreshDownloadListView));
                RefreshFiles();
            }
        }

        private void ErrorDownloading(String fileName)
        {
            foreach (var download in downloadView)
            {
                if (download.FileName.Equals(fileName) == true && download.Status.Equals("Downloading") == true)
                {
                    download.Status = "Error";
                }
                this.listView_Downloads.Dispatcher.Invoke(new RefreshDownloadList(this.RefreshDownloadListView));
            }
        }

        private void RefreshFiles()
        {
            string ip = GetIP();
            var files = GetFiles();
            User user = new User(this.ConfigDetails.User, files, this.ConfigDetails.Port, ip);
            var serverRespond = ServerProxy.RefreshFiles(this.xMLHandler.Serialize<User>(user));
            if (CheckIfErrorFromServer(serverRespond) == false)
            {
                throw new Exception(serverRespond);
            }
        }

        private string GetIP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "";
        }

        private List<DuckTorrentClasses.File> GetFiles()
        {
            var paths = Directory.GetFiles(this.ConfigDetails.UploadPath);
            List<DuckTorrentClasses.File> files = new List<DuckTorrentClasses.File>();
            foreach (var path in paths)
            {
                DuckTorrentClasses.File file = new DuckTorrentClasses.File();
                file.FileSize = new System.IO.FileInfo(path).Length;
                file.FileName = new System.IO.FileInfo(path).Name;
                files.Add(file);
            }
            return files;
        }

        private void RefreshDownloadListView()
        {
            this.listView_Downloads.Items.Refresh();
        }

        private void RefreshUploadListView()
        {
            this.listView_Uploads.Items.Refresh();
        }

        private void listViewResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.IsEnable == true)
            {
                FileSeed fileSeed = (FileSeed)this.listView_Results.SelectedItem;
                Task.Factory.StartNew(() => this.downloader.StartDownloading(fileSeed));
            }
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
            //System.IO.File.Delete(Login.CONFIGFILE);
            Close();

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
            LogOutUser();
            this.Uploader.StopListening();
            this.downloader.StopDownloading();
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
            this.listView_Downloads.Dispatcher.Invoke(new RefreshDownloadList(this.RefreshDownloadListView));
        }
    }
}
