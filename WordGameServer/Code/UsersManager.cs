using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WordGameServer.DbModel;

namespace WordGameServer.Code
{
    public class UsersManager
    {
        #region Singleton

        private static UsersManager _instance = new UsersManager();
        public static UsersManager Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion


        public ConcurrentDictionary<string, DateTime> LiveUsers = new ConcurrentDictionary<string, DateTime>();

        public user GetUserByUId(string userId)
        {
            using(var dbContext = new DB_A28BF5_WordGameDbEntities())
            {
                var curUserList = dbContext.users.Where(user => user.UniqueId == userId);
                if (curUserList.Any())
                {
                    return curUserList.FirstOrDefault();
                }
                else
                {
                   return user.USER_NOT_FOUND;
                }
            }
        }

        public user GetUserByName(string userName)
        {
            using (var dbContext = new DB_A28BF5_WordGameDbEntities())
            {
                var curUserList = dbContext.users.Where(user => user.UserName == userName);
                if (curUserList.Any())
                {
                    return curUserList.FirstOrDefault();
                }
                else
                {
                    return user.USER_NOT_FOUND;
                }
            }
        }

        public void RemoveUserByName(string userName)
        {
            using (var dbContext = new DB_A28BF5_WordGameDbEntities())
            {
                var userToRemove = dbContext.users.Where(x => userName == x.UserName).FirstOrDefault();
                if (userToRemove == null)
                    return;
                dbContext.users.Remove(userToRemove);
                dbContext.SaveChanges();
            }
        }

        internal int AddUserName(string userUid, string userName)
        {
            using (var dbContext = new DB_A28BF5_WordGameDbEntities())
            {
                if (dbContext.users.Any(user=> user.UserName == userName))
                {
                    return -1;
                }
                dbContext.users.Add(new user
                {
                    IconId = new Random().Next(1, 10),
                    UserName = userName,
                    UniqueId = userUid,
                    LastLoginDate = DateTime.Now
                });
                dbContext.SaveChanges();
                return 0;
            }
        }

        internal void UserIsLive(string eventAuthor)
        {
            var timeNow = DateTime.Now;
            LiveUsers.AddOrUpdate(eventAuthor, timeNow, (key, oldValue) => timeNow);
        }
    }
}