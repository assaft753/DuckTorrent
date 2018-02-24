using DuckTorrentClasses;
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
                Console.WriteLine("SSS");
                while (1 == 1) { }
            }
        }
    }
}
