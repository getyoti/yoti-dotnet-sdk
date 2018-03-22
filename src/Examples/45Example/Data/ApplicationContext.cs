using System.Data.Entity;
using Example.Models;

namespace Example.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }
    }
}