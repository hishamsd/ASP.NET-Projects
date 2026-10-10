namespace Project_1.Infrastructure.Repositories.Base
{
    public interface IUnitOfWork
    {
        IDepartmentRepository DepartmentRepo { get; }
        IEmployeeRepository EmployeeRepo { get; }
        IJobRepository JobRepo { get; }
        IMemberRepository MemberRepo { get; }
        IPermissionRepository PermissionRepo { get; }
        IPlanRepository PlanRepo { get; }
        IRoleRepository RoleRepo { get; }
        ISubscribeRepository SubscribeRepo { get; }
        IUserRepository UserRepo { get; }

        void Save();
    }
}