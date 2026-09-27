using Microsoft.EntityFrameworkCore;
using Project_1.Models;



namespace Project_1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Subscribe> Subscriptions { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserFile> UserFiles { get; set; }


        public DbSet<PermissionRoles> PermissionRoles { get; set; }
        public DbSet<RoleUser> RoleUsers { get; set; }
        public DbSet<Job> Jobs { get; set; }

        public DbSet<Department> Departments { get; set; }

        

        //public DbSet<UserFile> userFiles { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<PermissionRoles>()
                .HasKey(pr => new { pr.PermissionsId, pr.RolesId });


            modelBuilder.Entity<RoleUser>()
               .HasKey(ru => new { ru.RoleId, ru.UserId });

        }

    }
}


