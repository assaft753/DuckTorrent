using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Xml;
using System.ServiceModel;
using DuckTorrentService;
using DuckTorrentClasses;
using System.Windows.Forms;

namespace ClientApplication
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public static readonly string CONFIGFILE = "MyConfig.xml";
        public static readonly string BAD = "400";
        private XMLHandler xMLHandler = new XMLHandler();
        private ConfigDetails ConfigDetails;
        private IDuckTorrentServerApi ServerProxy;
        private ChannelFactory<IDuckTorrentServerApi> Factory;
        private TcpListener TcpListener;

        public Login()
        {
            InitializeComponent();
            try
            {
                /*var con = new ConfigDetails()
                {
                    User = new UserDetails("assaf", "1234"),
                    ServerIP = "127.0.0.1",
                    Port = 8005,
                    DownloadPath = @"C:\Users\assaftayouri\source\repos\DuckTorrent\DuckTorrentClient\bin\Debug",
                    UploadPath = @"C:\Users\assaftayouri\source\repos\DuckTorrent\DuckTorrentClient\bin\Debug"
                };
                var str = xMLHandler.Serialize<ConfigDetails>(con);
                System.IO.File.WriteAllText(CONFIGFILE, str);*/
                CheckXMLConfigValidation();
                InitDetailsAndConnections();
                MoveToMainWindows();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                PutData();
            }
            /*XMLHandler xMLHandler = new XMLHandler();
            var con = new ConfigDetails()
            {
                User = new UserDetails("assaf", "1234"),
                ServerIP = "localhost",
                Port = 8090
            };
            EndpointAddress ep = new EndpointAddress(con.GenerateServerIPAdress());
            ChannelFactory<IDuckTorrentServerApi> factory = new ChannelFactory<IDuckTorrentServerApi>(new BasicHttpBinding(), ep);
            IDuckTorrentServerApi proxy = factory.CreateChannel();
            string answer = proxy.SearchFile(xMLHandler.Serialize<FileSearch>(new FileSearch("u", con.User)));
            MessageBox.Show(answer);*/

        }

        private void MoveToMainWindows()
        {
            SignInUser();
            MainWindow mainWindow = new MainWindow(this.ConfigDetails, this.ServerProxy, this.TcpListener);
            mainWindow.CloseConnectionEvent += this.CloseConnection;
            PutData();
            mainWindow.Show();
            this.Hide();
        }

        private void InitDetailsAndConnections()
        {
            CheckInputDetails();
            OpenServerSocket();
            CheckUserValidtion();
            OpenTcpListener();
        }

        private void PutData()
        {
            this.username_textBox.Text = ConfigDetails.User.UserName;
            this.uploadPath_Textbox.Text = ConfigDetails.UploadPath;
            this.downloadPath_Textbox.Text = ConfigDetails.DownloadPath;
            this.serverip_Textbox.Text = ConfigDetails.ServerIP;
            this.serverport_Textbox.Text = ConfigDetails.Port.ToString();

        }

        private void CheckInputDetails()
        {
            if (this.ConfigDetails.Port == 0)
            {
                throw new Exception("Port Cant Be 0");
            }
            IPAddress iPAddress = null;
            if (this.ConfigDetails.ServerIP == "" || IPAddress.TryParse(this.ConfigDetails.ServerIP, out iPAddress) == false)
            {
                throw new Exception("Not Valid IP Address");
            }
            if (this.ConfigDetails.User.Password == "" || this.ConfigDetails.User.UserName == "")
            {
                throw new Exception("Not Valid User Login");
            }
            if (Directory.Exists(this.ConfigDetails.DownloadPath) == false)
            {
                throw new Exception("No Such Download Directory");
            }
            if (Directory.Exists(this.ConfigDetails.UploadPath) == false)
            {
                throw new Exception("No Such Upload Directory");
            }

        }

        private void OpenTcpListener()
        {
            if (this.TcpListener != null && this.TcpListener.Server.IsBound == true)
            {
                this.TcpListener.Stop();
            }
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, ConfigDetails.Port);
            this.TcpListener = new TcpListener(endPoint);
            this.TcpListener.Start();
        }

        private void CheckUserValidtion()
        {
            string serverRespond = ServerProxy.CheckUserExists(this.xMLHandler.Serialize<UserDetails>(ConfigDetails.User));
            if (CheckIfErrorFromServer(serverRespond) == false)
            {
                throw new Exception(serverRespond);
            }
        }

        private Boolean CheckIfErrorFromServer(string serverRespond)
        {
            string answerCode = "";
            foreach (Char c in serverRespond.Take(3))
            {
                answerCode += c;
            }
            if (answerCode.Equals(BAD))
            {
                return false;
            }
            return true;
        }

        private void OpenServerSocket()
        {
            if (this.Factory != null && this.Factory.State == CommunicationState.Opened)
            {
                this.Factory.Close();
            }
            EndpointAddress endpoint = new EndpointAddress(this.ConfigDetails.GenerateServerIPAdress());
            ChannelFactory<IDuckTorrentServerApi> factory = new ChannelFactory<IDuckTorrentServerApi>(new BasicHttpBinding(), endpoint);
            this.Factory = factory;
            this.ServerProxy = factory.CreateChannel();
        }

        private void CheckXMLConfigValidation()
        {
            try
            {
                if (System.IO.File.Exists(CONFIGFILE) == false)
                {
                    throw new Exception("Config File Not Found In " + Directory.GetCurrentDirectory());
                }
                String xmlfilestring = "";
                string line;
                using (StreamReader sr = new StreamReader(CONFIGFILE))
                {

                    while ((line = sr.ReadLine()) != null)
                    {
                        xmlfilestring += line;
                    }
                }
                this.ConfigDetails = xMLHandler.Deserialize<ConfigDetails>(xmlfilestring);
            }
            catch (Exception ex)
            {
                this.ConfigDetails = new ConfigDetails()
                {
                    User = new UserDetails()
                };
                throw ex;
            }
        }

        private void UploadBtn_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog uploadDirectoryBrowser = new FolderBrowserDialog();
            DialogResult answer = uploadDirectoryBrowser.ShowDialog();
            if (answer == System.Windows.Forms.DialogResult.OK)
            {
                this.uploadPath_Textbox.Text = uploadDirectoryBrowser.SelectedPath;
            }
        }

        private void DownloadBtn_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog downloadDirectoryBrowser = new FolderBrowserDialog();
            DialogResult answer = downloadDirectoryBrowser.ShowDialog();
            if (answer == System.Windows.Forms.DialogResult.OK)
            {
                this.downloadPath_Textbox.Text = downloadDirectoryBrowser.SelectedPath;
            }
        }

        private void LogInBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateConfiguration();
                InitDetailsAndConnections();
                var str = xMLHandler.Serialize<ConfigDetails>(this.ConfigDetails);
                System.IO.File.WriteAllText(CONFIGFILE, str);
                MoveToMainWindows();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void SignInUser()
        {
            string ip = GetIP();
            var files = GetFiles();
            User user = new User(this.ConfigDetails.User, files, this.ConfigDetails.Port, ip);
            var serverRespond = ServerProxy.SignIn(this.xMLHandler.Serialize<User>(user));
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

        private void UpdateConfiguration()
        {
            this.ConfigDetails.User.UserName = username_textBox.Text;
            this.ConfigDetails.User.Password = password_textBox.Password;
            this.ConfigDetails.Port = int.Parse(this.serverport_Textbox.Text);
            this.ConfigDetails.ServerIP = this.serverip_Textbox.Text;
            this.ConfigDetails.DownloadPath = this.downloadPath_Textbox.Text;
            this.ConfigDetails.UploadPath = this.uploadPath_Textbox.Text;
        }

        private void CloseConnection()
        {
            this.Factory.Close();
            this.TcpListener.Stop();
            Show();
        }
    }
}
