using Microsoft.EntityFrameworkCore;

namespace iDeal.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<User> User {get; set; }

        public DbSet<Games> Games {get; set; }


    }
}
