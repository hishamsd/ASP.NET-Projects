using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Project_1.Infrastructure.Data;
using Project_1.Infrastructure.Repositories;
using Project_1.Infrastructure.Repositories.Base;
using Project_1.Application.Services;
using Project_1.Application.Services.Base;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultDatabase")));

builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IPlanRepository, PlanRepository>();
builder.Services.AddScoped<ISubscribeRepository, SubscribeRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IPlanService, PlanService>();
builder.Services.AddScoped<ISubscribeService, SubscribeService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Accounts/Login";
        options.AccessDeniedPath = "/Accounts/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
    });

builder.Services.AddAuthorization(options =>
{
    var permissionPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireAssertion(context =>
        {
            if (context.Resource is not HttpContext httpContext)
            {
                return false;
            }

            var action = httpContext.GetEndpoint()?
                .Metadata.GetMetadata<ControllerActionDescriptor>();

            // الطلب لا يشير إلى أكشن MVC.
            if (action == null)
            {
                return true;
            }

            var controllerName = action.ControllerName;
            var actionName = action.ActionName.ToLowerInvariant();

            // السماح بالخروج وعرض رسالة منع الوصول.
            if (controllerName == "Accounts")
            {
                return actionName is "logout" or "accessdenied";
            }

            // صفحة معالجة الخطأ لا تحتاج صلاحية قراءة.
            if (controllerName == "Home" && actionName == "error")
            {
                return true;
            }

            bool canRead =
                context.User.HasClaim("Permission", "Read");

            bool canEdit =
                context.User.HasClaim("Permission", "Edit");

            bool canCreate =
                context.User.HasClaim("Permission", "Create");

            bool canDelete =
                context.User.HasClaim("Permission", "Delete");

            bool hasAllPermissions =
                canRead && canEdit && canCreate && canDelete;

            if (!canRead)
            {
                return false;
            }

            // تغيير الرولز والصلاحيات المخصصة يتطلب الأربع صلاحيات.
            if (actionName is "manageroles" or "assignpermissions")
            {
                return hasAllPermissions;
            }

            // تعديل الحسابات والرولز والصلاحيات يخصص للأدمن.
            bool isAdministrationController =
                controllerName is "Users" or "Roles" or "Permissions";

            bool isWriteAction =
                actionName is "create"
                    or "edit"
                    or "delete"
                    or "deleteconfirmed"
                || !HttpMethods.IsGet(httpContext.Request.Method)
                   && !HttpMethods.IsHead(httpContext.Request.Method);

            if (isAdministrationController && isWriteAction)
            {
                return hasAllPermissions;
            }

            return actionName switch
            {
                "create" => canCreate,

                "edit" => canEdit,

                "delete" or "deleteconfirmed" => canDelete,

                "index" or "index2" or "getall" or "getbyid"
                    or "details" or "privacy" => canRead,

                "managefiles" =>
                    HttpMethods.IsGet(httpContext.Request.Method)
                    || HttpMethods.IsHead(httpContext.Request.Method)
                        ? canRead
                        : hasAllPermissions,

                // أي أكشن جديد يحتاج إضافته هنا بصلاحيته.
                _ => false
            };
        })
        .Build();

    options.DefaultPolicy = permissionPolicy;
    options.FallbackPolicy = permissionPolicy;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Accounts}/{action=Login}/{id?}");

app.Run();