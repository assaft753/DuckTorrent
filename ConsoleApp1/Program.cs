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
            Console.WriteLine("client 1");
            EndpointAddress ep = new EndpointAddress("http://localhost:8006/ServerApi");
            ChannelFactory<IDuckTorrentServerApi> factory = new ChannelFactory<IDuckTorrentServerApi>(new BasicHttpBinding(), ep);
            IDuckTorrentServerApi proxy = factory.CreateChannel();
            FileSearch fs = new FileSearch("f", new UserDetails("sss", "dddd"));
            XMLHandler xMLHandler = new XMLHandler();
            string name = proxy.SearchFile(xMLHandler.Serialize(fs));
            Console.WriteLine(name);
            //Console.WriteLine(count);
        }
    }
}
