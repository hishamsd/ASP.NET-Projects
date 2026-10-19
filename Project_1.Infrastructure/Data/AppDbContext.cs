using Microsoft.EntityFrameworkCore;
using Project_1.Domain.Models;



namespace Project_1.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
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

            modelBuilder.Entity<Member>().HasOne(member => member.User)
                .WithOne().HasForeignKey<Member>(member => member.UserId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Plan>().HasOne(plan => plan.Category)
                .WithMany(category => category.Plans).HasForeignKey(plan => plan.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Plan>().Property(plan => plan.DurationMonths).HasDefaultValue(1);
            modelBuilder.Entity<CartItem>().HasIndex(item => item.UserId).IsUnique();
            modelBuilder.Entity<CartItem>().HasOne(item => item.User).WithMany()
                .HasForeignKey(item => item.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CartItem>().HasOne(item => item.Plan).WithMany()
                .HasForeignKey(item => item.PlanId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<CartItem>().HasOne(item => item.Employee).WithMany()
                .HasForeignKey(item => item.EmployeeId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Express", Description = "فرع أصغر، أجهزة أقل ومزايا محدودة، بدون مسبح", HasPool = false },
                new Category { Id = 2, Name = "Premium", Description = "فرع أكبر، أجهزة أكثر ومزايا أكثر، مع مسبح", HasPool = true });


            modelBuilder.Entity<PermissionRoles>()
                .HasKey(pr => new { pr.PermissionsId, pr.RolesId });


            modelBuilder.Entity<RoleUser>()
               .HasKey(ru => new { ru.RoleId, ru.UserId });

        }

    }
}


