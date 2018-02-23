using DuckTorrentClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckTorrentDB
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileHandler = new FileHandler();
            var dic = fileHandler.FindFile("y");
            var l = new List<FileSeed>();
            foreach (var x in fileHandler.FindFile("y"))
            {
                l.Add(x.Value);
            }
            var xml = new XMLHandler();
            Console.WriteLine(xml.Serialize<List<FileSeed>>(l));

        }
    }
}
