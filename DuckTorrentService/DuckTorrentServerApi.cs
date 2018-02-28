using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using DuckTorrentClasses;
using DuckTorrentDB;

namespace DuckTorrentService
{
    public class DuckTorrentServerApi : IDuckTorrentServerApi
    {
        public bool CheckEnable(string userByXML)
        {
            string toPrint = "";
            try
            {
                XMLHandler xMLHandler = new XMLHandler();
                var user = xMLHandler.Deserialize<UserDetails>(userByXML);
                toPrint += "User Name: " + user.UserName + " Check If User Enable..... ";
                var userHandler = new ClientHandler();
                var enable = userHandler.IsEnable(user.UserName);
                if (enable == true)
                {
                    toPrint += "User Enable ";
                }
                else
                {
                    toPrint += "User Disable ";
                }
                Console.WriteLine(toPrint);
                return enable;
            }
            catch (Exception ex)
            {
                toPrint += "400" + " " + ex.Message;
                Console.WriteLine(toPrint);
                return false;
            }

        }

        public string CheckUserExists(string userByXML)
        {
            string toPrint = "";
            try
            {
                XMLHandler xMLHandler = new XMLHandler();
                var user = xMLHandler.Deserialize<UserDetails>(userByXML);
                toPrint += "User Name: " + user.UserName + " Check If User Exists..... ";
                var userHandler = new ClientHandler();
                userHandler.CheckUser(user.UserName, user.Password);
                toPrint += "User Exists ";
                Console.WriteLine(toPrint);
                return "200";
            }
            catch (Exception ex)
            {
                toPrint += "400" + " " + ex.Message;
                Console.WriteLine(toPrint);
                return "400" + " " + ex.Message;
            }
        }

        public string RefreshFiles(string userByXML)
        {
            string toPrint = "";
            try
            {

                XMLHandler xMLHandler = new XMLHandler();
                var user = xMLHandler.Deserialize<DuckTorrentClasses.User>(userByXML);
                toPrint += "User Name: " + user.UserInfo.UserName + " Trying To Refresh Files.... ";
                ClientHandler clientHandler = new ClientHandler();
                FileHandler fileHandler = new FileHandler();
                clientHandler.CheckUser(user.UserInfo.UserName, user.UserInfo.Password);
                fileHandler.RefreshFiles(user.Ip, user.Port, user.UserInfo.UserName, user.Files);
                toPrint += "File's User Refreshed Succefully ";
                Console.WriteLine(toPrint);
                return "200";
            }
            catch (Exception ex)
            {
                toPrint += " 400" + " " + ex.Message;
                Console.WriteLine(toPrint);
                return " 400" + " " + ex.Message;
            }

        }

        public string SearchFile(string fileByXML)
        {
            string toPrint = "";
            try
            {
                XMLHandler xMLHandler = new XMLHandler();
                var fileSearch = xMLHandler.Deserialize<FileSearch>(fileByXML);
                toPrint += "User: " + fileSearch.UserInfo.UserName + " Searching File: " + fileSearch.FileName + ".....";
                FileHandler fileHandler = new FileHandler();
                ClientHandler clientHandler = new ClientHandler();
                if (clientHandler.CheckUser(fileSearch.UserInfo.UserName, fileSearch.UserInfo.Password) == true)
                {
                    var result = fileHandler.FindFile(fileSearch.FileName, fileSearch.UserInfo.UserName);
                    if (result != null)
                    {
                        var respondList = new List<FileSeed>();
                        foreach (var x in result)
                        {
                            respondList.Add(x.Value);
                        }
                        Console.WriteLine(toPrint + " Matches Found");
                        return xMLHandler.Serialize<List<FileSeed>>(respondList);
                    }
                }
                Console.WriteLine(toPrint + " Matches Not Found");
                return null;
            }
            catch (Exception ex)
            {
                toPrint += " 400" + " " + ex.Message;
                Console.WriteLine(toPrint);
                return " 400" + " " + ex.Message;
            }
        }

        public string SignIn(string userByXML)
        {
            string toPrint = "";
            try
            {

                XMLHandler xMLHandler = new XMLHandler();
                var user = xMLHandler.Deserialize<DuckTorrentClasses.User>(userByXML);
                toPrint += "User Name: " + user.UserInfo.UserName + " Trying To Sign In.... ";
                ClientHandler clientHandler = new ClientHandler();
                clientHandler.CheckUser(user.UserInfo.UserName, user.UserInfo.Password);
                var answer = clientHandler.ClientOn(user.UserInfo.UserName, user.Ip, user.Port, user.Files);
                if (answer == true)
                {
                    toPrint += "User Is Now Online And Signed In ";
                }
                else
                {
                    toPrint += "User Is Offline Because Its Disable";
                }
                Console.WriteLine(toPrint);
                return "200";
            }
            catch (Exception ex)
            {
                toPrint += " 400" + " " + ex.Message;
                Console.WriteLine(toPrint);
                return " 400" + " " + ex.Message;
            }

        }

        public string SignOut(string userByXML)
        {
            string toPrint = "";
            try
            {
                XMLHandler xMLHandler = new XMLHandler();
                var user = xMLHandler.Deserialize<DuckTorrentClasses.UserDetails>(userByXML);
                toPrint += "User Name: " + user.UserName + " Trying To Sign Out.... ";
                ClientHandler clientHandler = new ClientHandler();
                clientHandler.CheckUser(user.UserName, user.Password);
                clientHandler.ClientOff(user.UserName, user.Password);
                toPrint += "User Is Now Offline And Signed Out ";
                Console.WriteLine(toPrint);
                return "200";
            }
            catch (Exception ex)
            {
                toPrint += " 400" + " " + ex.Message;
                Console.WriteLine(toPrint);
                return " 400" + " " + ex.Message;
            }
        }
    }
}
