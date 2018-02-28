using DuckTorrentClasses;
using DuckTorrentDB;
using DuckTorrentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost sh = new ServiceHost((typeof(DuckTorrentServerApi))))
            {

                sh.Open();
                ClientHandler clientHandler = new ClientHandler();
                Console.WriteLine(clientHandler.ShowUsers());
                while (1 == 1) { }
            }
        }
    }
}
