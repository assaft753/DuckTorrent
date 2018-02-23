using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckTorrentClasses
{
    public class File
    {
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public File(string fileName, long fileSize)
        {
            FileName = fileName;
            FileSize = fileSize;
        }
        public File() { }
    }
}
