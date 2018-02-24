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
//using ObjectsLibrary;
using System.IO;
using System.Xml;
using System.ServiceModel;
using DuckTorrentService;
using DuckTorrentClasses;
//using Newtonsoft.Json;

namespace ClientApplication
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private static readonly string CONFIGFILE = "MyConfig.xml";
        private static readonly string BAD = "400";
        private XMLHandler xMLHandler = new XMLHandler();
        private ConfigDetails ConfigDetails;
        private IDuckTorrentServerApi ServerProxy;
        private TcpListener TcpListener;

        public Login()
        {
            InitializeComponent();
            try
            {
                /*{
                    var con = new ConfigDetails()
                    {
                        User = new UserDetails("assaf", "1234"),
                        ServerIP = "localhost",
                        Port = 8090
                    };*/
                //var str = xMLHandler.Serialize<ConfigDetails>(con);
                //System.IO.File.WriteAllText(CONFIGFILE, str);
                CheckXMLConfigValidation();
                OpenServerSocket();
                CheckUserValidtion();
                OpenTcpListener();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void OpenTcpListener()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, ConfigDetails.Port);
            TcpListener listener = new TcpListener(endPoint);
            listener.Start();
        }

        private void CheckUserValidtion()
        {
            string serverRespond = ServerProxy.CheckUserExists(this.xMLHandler.Serialize<UserDetails>(ConfigDetails.User));
            string answerCode = "";
            foreach (Char c in serverRespond.Take(3))
            {
                answerCode += c;
            }
            if (answerCode.Equals(BAD))
            {
                throw new Exception(serverRespond);
            }
        }

        private void OpenServerSocket()
        {
            EndpointAddress endpoint = new EndpointAddress(this.ConfigDetails.GenerateServerIPAdress());
            ChannelFactory<IDuckTorrentServerApi> factory = new ChannelFactory<IDuckTorrentServerApi>(new BasicHttpBinding(), endpoint);
            this.ServerProxy = factory.CreateChannel();
        }

        private void CheckXMLConfigValidation()
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

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Log_In_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void upBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void downBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
