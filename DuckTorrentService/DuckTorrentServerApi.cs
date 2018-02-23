using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DuckTorrentClasses;
using DuckTorrentDB;

namespace DuckTorrentService
{
    public class DuckTorrentServerApi : IDuckTorrentServerApi
    {
        public string SearchFile(string fileByXML)
        {
            try
            {
                XMLHandler xMLHandler = new XMLHandler();
                var fileSearch = xMLHandler.Deserialize<FileSearch>(fileByXML);
                FileHandler fileHandler = new FileHandler();
                ClientHandler clientHandler = new ClientHandler();
                if (clientHandler.CheckUser(fileSearch.UserInfo.UserName, fileSearch.UserInfo.Password) == true)
                {
                    var result = fileHandler.FindFile(fileSearch.FileName);
                    if (result != null)
                    {
                        var respondList = new List<FileSeed>();
                        foreach (var x in result)
                        {
                            respondList.Add(x.Value);
                        }
                        return xMLHandler.Serialize<List<FileSeed>>(respondList);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return "400" + " " + ex.Message;
            }
        }

        public string SignIn(string userByXML)
        {
            /*try
            {
                XMLHandler xMLHandler = new XMLHandler();
                var user = xMLHandler.Deserialize<DuckTorrentClasses.User>(userByXML);
                ClientHandler clientHandler = new ClientHandler();
                if (clientHandler.CheckUser(user.UserInfo.UserName, user.UserInfo.Password) == true)
                {
                    clientHandler.ClientOn(user.UserInfo.UserName, user.Ip, user.Port, user.Files);
                    return "200";
                }
                return "300";
            }
            catch (Exception ex)
            {
                return "400" + " " + ex.Message;
            }*/
            Console.WriteLine("Enter2");
            return userByXML;
        }

        public string SignOut(string userByXML)
        {
            try
            {
                XMLHandler xMLHandler = new XMLHandler();
                var user = xMLHandler.Deserialize<DuckTorrentClasses.UserDetails>(userByXML);
                ClientHandler clientHandler = new ClientHandler();
                if (clientHandler.CheckUser(user.UserName, user.Password) == true)
                {
                    clientHandler.ClientOff(user.UserName, user.Password);
                    return "200";
                }
                return "300";
            }
            catch (Exception ex)
            {
                return "400" + " " + ex.Message;
            }

        }
    }
}
