
using DuckTorrentClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckTorrentDB
{
    public class FileHandler
    {

        public FileHandler() { }

        public int GetNumberOfFiles()
        {
            using (DuckTorrentDBEntities db = new DuckTorrentDBEntities())
            {

                var files = from file in db.Files
                            select file;
                return files.Count();
            }
        }

        public List<File> GetFilesByName(String name)
        {
            using (DuckTorrentDBEntities db = new DuckTorrentDBEntities())
            {

                var files = from file in db.Files
                            where file.FIleName == name
                            select file;

                return files.ToList<File>();
            }
        }

        public void AddFiles(String ip, int port, String userName, List<DuckTorrentClasses.File> files)
        {
            using (DuckTorrentDBEntities db = new DuckTorrentDBEntities())
            {
                foreach (var file in files)
                {
                    File f = new File();
                    f.FIleName = file.FileName;
                    f.FileSize = file.FileSize;
                    f.IP = ip;
                    f.UserName = userName;
                    f.Port = port;
                    db.Files.Add(f);
                    db.SaveChanges();
                }
            }
        }

        public void RemoveFiles(String userName)
        {
            using (DuckTorrentDBEntities db = new DuckTorrentDBEntities())
            {

                var files = from f in db.Files
                            where f.UserName == userName
                            select f;
                var l = files.ToList();
                db.Files.RemoveRange(files);
                db.SaveChanges();
            }

        }

        public Dictionary<String, FileSeed> FindFile(string fileName, string userName)
        {
            using (DuckTorrentDBEntities db = new DuckTorrentDBEntities())
            {
                var files = from f in db.Files
                            where f.FIleName.Contains(fileName) && f.UserName != userName
                            group f by f.FIleName into GroupFiles
                            select GroupFiles;

                if (files.Count() == 0)
                {
                    return null;
                }
                var result = new Dictionary<String, FileSeed>();
                Boolean modify = false;
                foreach (var nameGroup in files)
                {
                    if (result.ContainsKey(nameGroup.Key) == false)
                    {
                        result.Add(nameGroup.Key, new FileSeed(nameGroup.Key));
                        modify = true;
                    }
                    else
                    {
                        modify = false;
                    }
                    foreach (var file in nameGroup)
                    {
                        if (modify == true)
                        {
                            result[nameGroup.Key].Size = file.FileSize;
                        }
                        result[nameGroup.Key].Seeds.Add(new IP(file.IP, file.Port));
                    }
                }

                return result;
            }
        }
    }
}
