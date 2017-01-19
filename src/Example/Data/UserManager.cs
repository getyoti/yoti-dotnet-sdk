using Example.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Example.Models
{
    public static class UserManager
    {
        public static User GetUserByYotiId(string yotiId)
        {
            using (ApplicationContext _db = new ApplicationContext())
            {
                return _db.Users.FirstOrDefault(u => u.YotiId == yotiId);
            }
        }
        public static User GetUserById(int id)
        {
            using (ApplicationContext _db = new ApplicationContext())
            {
                return _db.Users.FirstOrDefault(u => u.Id == id);
            }
        }

        public static void SaveUser(User user)
        {
            using (ApplicationContext _db = new ApplicationContext())
            {
                if (user.Id == 0)
                {
                    _db.Users.Add(user);
                }

                _db.SaveChanges();
            }
        }
    }
}