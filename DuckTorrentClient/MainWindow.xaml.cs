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

namespace ClientApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private XMLHandler xMLHandler = new XMLHandler();
        private ConfigDetails ConfigDetails;
        private IDuckTorrentServerApi ServerProxy;
        private TcpListener TcpListener;
        public MainWindow(ConfigDetails configDetails, IDuckTorrentServerApi serverProxy, TcpListener tcpListener)
        {
            InitializeComponent();
            this.ConfigDetails = configDetails;
            this.ServerProxy = serverProxy;
            this.TcpListener = tcpListener;
        }

        private void listViewResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void listViewResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }


        private void listView_Downloads_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void listView_Downloads_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void listView_Uploads_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void search_Btn_Click(object sender, RoutedEventArgs e)
        {



        }

        //When logout button is clicked it will delete the info and open log in screen
        private void logOut_Btn_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (File.Exists(Main.XML_FILE_NAME))
            {
                File.Delete(Main.XML_FILE_NAME);
            }
            new Login();
            Close();
            */
        }

        // this method can be removed
        private void file_name_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        //when window is closing update the server that user is out
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /*
            s.Send(Encoding.Default.GetBytes(JsonConvert.SerializeObject(new Details())));

            s.Close();
            s.Dispose();
        */
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // do nothing

        }

    }
}
