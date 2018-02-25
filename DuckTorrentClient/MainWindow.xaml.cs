﻿using DuckTorrentClasses;
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

namespace ClientApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public delegate void CloseConnections();
    public partial class MainWindow : Window
    {
        private XMLHandler xMLHandler = new XMLHandler();
        private ConfigDetails ConfigDetails;
        private TcpListener TcpListener;
        private IDuckTorrentServerApi ServerProxy;
        public event CloseConnections CloseConnectionEvent;
        public MainWindow(ConfigDetails configDetails, IDuckTorrentServerApi serverProxy, TcpListener tcpListener)
        {
            try
            {
                InitializeComponent();
                this.ConfigDetails = configDetails;
                this.ServerProxy = serverProxy;
                this.TcpListener = tcpListener;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listViewResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void listViewResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var s = (ListView)sender;
            foreach (var x in s.ItemsSource)
            {
                MessageBox.Show(x.GetType().ToString());
            }

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
            this.CloseConnectionEvent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // do nothing

        }

    }
}
