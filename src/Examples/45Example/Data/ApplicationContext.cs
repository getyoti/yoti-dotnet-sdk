using System.Data.Entity;
using Example.Models;

namespace Example.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("DefaultConnection")
        {
            Database.SetInitializer<ApplicationContext>(null);
        }

        public DbSet<User> Users { get; set; }
    }
}