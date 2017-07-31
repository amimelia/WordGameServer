using System;
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
                    UserName = userName,
                    UniqueId = userUid,
                    LastLoginDate = DateTime.Now
                });
                dbContext.SaveChanges();
                return 0;
            }
        }
    }
}