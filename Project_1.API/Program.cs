using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Project_1.Application.Services;
using Project_1.Application.Services.Base;
using Project_1.Infrastructure.Data;
using Project_1.Infrastructure.Repositories.Base;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultDatabase");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IPlanService, PlanService>();
builder.Services.AddScoped<ISubscribeService, SubscribeService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

var mvcImagesPath = Path.GetFullPath(Path.Combine(
    app.Environment.ContentRootPath,
    app.Configuration["MvcImagesPath"] ?? "../Project 1/wwwroot/images"));

if (Directory.Exists(mvcImagesPath))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(mvcImagesPath),
        RequestPath = "/images"
    });
}

app.MapControllers();
app.Run();
