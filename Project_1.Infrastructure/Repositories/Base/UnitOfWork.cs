using Project_1.Infrastructure.Data;

namespace Project_1.Infrastructure.Repositories.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;

        public UnitOfWork(AppDbContext db)
        {
            _db = db;

            CartRepo = new CartRepository(db);
            CategoryRepo = new CategoryRepository(db);
            DepartmentRepo = new DepartmentRepository(db);
            EmployeeRepo = new EmployeeRepository(db);
            JobRepo = new JobRepository(db);
            MemberRepo = new MemberRepository(db);
            PermissionRepo = new PermissionRepository(db);
            PlanRepo = new PlanRepository(db);
            RoleRepo = new RoleRepository(db);
            SubscribeRepo = new SubscribeRepository(db);
            UserRepo = new UserRepository(db);
        }

        public ICartRepository CartRepo { get; }
        public ICategoryRepository CategoryRepo { get; }
        public IDepartmentRepository DepartmentRepo { get; }
        public IEmployeeRepository EmployeeRepo { get; }
        public IJobRepository JobRepo { get; }
        public IMemberRepository MemberRepo { get; }
        public IPermissionRepository PermissionRepo { get; }
        public IPlanRepository PlanRepo { get; }
        public IRoleRepository RoleRepo { get; }
        public ISubscribeRepository SubscribeRepo { get; }
        public IUserRepository UserRepo { get; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}