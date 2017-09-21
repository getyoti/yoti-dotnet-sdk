using System.Linq;
using Example.Data;

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

        public static void SaveUser(User newUser)
        {
            using (ApplicationContext _db = new ApplicationContext())
            {
                if (newUser.Id == 0)
                {
                    _db.Users.Add(newUser);
                }

                User currentUser = _db.Users.Single(u => u.Id == newUser.Id);
                _db.Entry(currentUser).CurrentValues.SetValues(newUser);

                _db.SaveChanges();
            }
        }
    }
}