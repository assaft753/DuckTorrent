using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckTorrentDB
{
    public class ClientHandler
    {
        public ClientHandler() { }

        public bool FindUser(string userName)
        {
            DuckTorrentDBEntities db = new DuckTorrentDBEntities();

            var account = from ac in db.Users
                          where ac.UserName == userName
                          select ac;

            if (account.Count() == 0)
                return false;

            return true;

        }

        public void AddUser(string userName, string password)
        {
            if (!FindUser(userName))
            {

                using (DuckTorrentDBEntities db = new DuckTorrentDBEntities())
                {
                    User user = new User();
                    user.UserName = userName;
                    user.Password = password;
                    user.IsOnline = 1;
                    db.Users.Add(user);
                    db.SaveChanges();
                }
            }
            else
                throw new Exception("account exist");
        }

        public String ShowUsers()
        {
            using (DuckTorrentDBEntities db = new DuckTorrentDBEntities())
            {
                String s = null;
                var users = from user in db.Users select user;

                foreach (var item in users)
                {
                    s += "#" + item.UserName + " " + item.Password + " " + item.IsOnline + "\n";
                }
                return s;
            }
        }

        public List<User> GetUsers()
        {
            DuckTorrentDBEntities db = new DuckTorrentDBEntities();

            var users = from user in db.Users select user;

            List<User> clientsList = new List<User>();

            foreach (var item in users)
                clientsList.Add(item);
            return clientsList;
        }

        public int GetTotalNumberOfUsers()
        {
            DuckTorrentDBEntities db = new DuckTorrentDBEntities();

            var users = from user in db.Users select user;

            return users.Count();
        }

        public int getNumberOfOnlineUsers()
        {
            DuckTorrentDBEntities db = new DuckTorrentDBEntities();

            var users = from user in db.Users
                        where user.IsOnline == 1
                        select user;

            return users.Count();
        }

        public Boolean CheckUser(String userName, string password)
        {
            if (!FindUser(userName))
                throw new Exception("User name does not exiest");

            DuckTorrentDBEntities db = new DuckTorrentDBEntities();

            var users = from user in db.Users
                        where user.UserName == userName
                        && user.Password == password
                        select user;

            if (users.Count() != 1)
                throw new Exception("Not valid password");
            return true;
        }

        public void ClientOn(string userName, String ip, int port, List<DuckTorrentClasses.File> files)
        {
            using (DuckTorrentDBEntities db = new DuckTorrentDBEntities())
            {
                User user = db.Users.Single(c => c.UserName == userName);
                user.IsOnline = 1;

                db.SaveChanges();
            }
            var fileHandler = new FileHandler();
            fileHandler.RemoveFiles(userName);
            fileHandler.AddFiles(ip, port, userName, files);
        }

        public void ClientOff(string userName, string password)
        {
            using (DuckTorrentDBEntities db = new DuckTorrentDBEntities())
            {
                User user = db.Users.Single(c => c.UserName == userName && c.Password == password);
                user.IsOnline = 0;
                db.SaveChanges();
            }
            new FileHandler().RemoveFiles(userName);
        }


    }
}

