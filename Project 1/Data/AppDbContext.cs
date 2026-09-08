using Microsoft.EntityFrameworkCore;
using Project_1.Models;



namespace Project_1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Plan> Plans { get; set; }
        public DbSet<Member> Members { get; set; }
    }
}
