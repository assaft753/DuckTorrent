using DuckTorrentClasses;
using DuckTorrentClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient tcpClient = new TcpClient("127.0.0.1", 1234);
            using (NetworkStream networkStream = tcpClient.GetStream())
            {
                using (StreamWriter sw = new StreamWriter(networkStream))
                {
                    ChunkRequest chunkRequest = new ChunkRequest("1.txt", 0, 15);
                    var x = new XMLHandler();
                    var y = x.Serialize<ChunkRequest>(chunkRequest);
                    var z = y.Replace('\n', ' ').Replace('\r', ' ').Trim();
                    sw.WriteLine(z);
                    sw.Flush();
                    var bytes = new byte[chunkRequest.ChunkSize];

                    networkStream.Read(bytes, 0, bytes.Length);
                    /*var bytes = new byte[1024];
                    networkStream.Read(bytes, 0, bytes.Length);
                    Console.WriteLine(bytes[0]);*/

                    foreach (var bytea in bytes)
                    {
                        Console.WriteLine(bytea);
                    }
                    /*networkStream.Write(System.Text.Encoding.Default.GetBytes("hello0"), 0, System.Text.Encoding.Default.GetBytes("hello1").Length);
                    networkStream.Flush();
                    var bytes = new byte[1024];
                    networkStream.Read(bytes, 0, bytes.Length);
                    Console.WriteLine(bytes[0]);
                    networkStream.Close();*/

                    //StreamWriter streamWriter = new StreamWriter(networkStream);
                    //streamWriter.write();
                    //streamWriter.Flush();
                }
            }
        }
    }
}
