
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckTorrentClasses
{
    public class FileSeed
    {
        public string FileName { get; set; }
        public long Size { get; set; }
        public List<IP> Seeds { get; set; }

        public FileSeed(string fileName, long size, List<IP> seeds)
        {
            FileName = fileName;
            Size = size;
            Seeds = seeds;
        }
        public FileSeed(string fileName)
        {
            FileName = fileName;
            Seeds = new List<IP>();

        }
        public FileSeed() { }
    }
}
