using Project_1.Application.Dtos.CartDtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Project_1.Application.Dtos.GymDtos;
using Project_1.Application.Dtos.UsersDtos;
using Project_1.Application.Services;
using Project_1.Domain.Models;
using Project_1.Infrastructure.Data;
using Project_1.Infrastructure.Repositories.Base;

using var connection = new SqliteConnection("Data Source=:memory:");
connection.Open();
var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;
using var db = new AppDbContext(options);
db.Database.EnsureCreated();
var uow = new UnitOfWork(db);
var users = new UserService(uow, new TestEnvironment());
var categories = new CategoryService(uow);
var plans = new PlanService(uow);
var employees = new EmployeeService(uow, new TestEnvironment());
var subscriptions = new SubscribeService(uow);
var cart = new CartService(uow);
void Check(bool value, string name) { if (!value) throw new Exception(name); Console.WriteLine("PASS " + name); }
async Task RejectAsync<T>(Func<Task> action, string name) where T : Exception
{
    try { await action(); } catch (T) { Console.WriteLine("PASS " + name); return; }
    throw new Exception("Expected rejection: " + name);
}

Check(users.Register(new RegisterDto { Name = "Member A", Username = "member-a", Email = "a@example.test", Password = "Secret123" }), "registration");
Check(users.Register(new RegisterDto { Name = "Member B", Username = "member-b", Email = "b@example.test", Password = "Secret123" }), "second registration");
Check(!users.Register(new RegisterDto { Name = "Duplicate", Username = "member-a", Email = "c@example.test", Password = "Secret123" }), "duplicate username rejected");
Check(!users.Register(new RegisterDto { Name = "Duplicate", Username = "member-c", Email = "a@example.test", Password = "Secret123" }), "duplicate email rejected");
var a = db.Users.Single(user => user.Username == "member-a");
var b = db.Users.Single(user => user.Username == "member-b");
Check(a.Password == "" && BCrypt.Net.BCrypt.Verify("Secret123", a.HashPassword), "only password hash stored");
Check(db.Members.Count() == 2 && db.Members.Single(member => member.UserId == a.Id).Name == a.Name, "member linked to account");
Check(!users.GetSession(a.Username)!.Permissions.Any() && !db.RoleUsers.Any(), "registration grants no admin permissions");
Check(users.Login(new LoginDto { Username = a.Username, Password = "Secret123" }), "login");
Check(!users.Login(new LoginDto { Username = a.Username, Password = "Wrong" }), "wrong password rejected");
var department = new Department { Name = "Gym" };
var trainerJob = new Job { Name = "Coach", Department = department, IsTrainer = true };
var officeJob = new Job { Name = "Reception", Department = department };
var trainer = new Employee { Name = "Trainer", Email = "coach@example.test", Phone = "1", Jobs = trainerJob, Departments = department };
var receptionist = new Employee { Name = "Office", Email = "office@example.test", Phone = "2", Jobs = officeJob, Departments = department };
var plan = new Plan { Name = "Premium three months", CategoryId = 2, DurationMonths = 3, Price = 900 };
var express = new Plan { Name = "Express one month", CategoryId = 1, DurationMonths = 1, Price = 200 };
var legacy = new Plan { Name = "Old plan", Price = 100 };
db.AddRange(trainer, receptionist, plan, express, legacy);
db.SaveChanges();
Check((await categories.GetCategoriesAsync()).Count() == 2, "Express and Premium seeded");
Check((await plans.GetPlansAsync(2)).Single().Id == plan.Id && !(await plans.GetPlansAsync(null)).Any(item => item.Id == legacy.Id), "plans filtered by category and legacy plans preserved");
Check((await employees.GetTrainersAsync()).Single().EmployeeId == trainer.Id, "only trainer jobs exposed");
await RejectAsync<ArgumentException>(async () => await cart.AddToCartAsync(a.Id, new AddCartDto { PlanId = plan.Id, EmployeeId = receptionist.Id }), "office employee rejected as trainer");
await RejectAsync<KeyNotFoundException>(async () => await cart.AddToCartAsync(a.Id, new AddCartDto { PlanId = int.MaxValue, EmployeeId = trainer.Id }), "missing plan rejected");
await RejectAsync<ArgumentException>(async () => await cart.AddToCartAsync(a.Id, new AddCartDto { PlanId = legacy.Id, EmployeeId = trainer.Id }), "unconfigured plan rejected");
await cart.AddToCartAsync(a.Id, new AddCartDto { PlanId = express.Id, EmployeeId = trainer.Id });
var oldCart = (await cart.GetCartAsync(a.Id))!;
await cart.AddToCartAsync(a.Id, new AddCartDto { PlanId = plan.Id, EmployeeId = trainer.Id });
Check(db.CartItems.Count() == 1 && (await cart.GetCartAsync(a.Id))!.PlanId == plan.Id, "one selected plan per account");
Check((await cart.GetCartAsync(b.Id)) == null && !(await subscriptions.GetUserSubscriptionsAsync(b.Id)).Any(), "accounts isolated");
await RejectAsync<InvalidOperationException>(async () => await cart.ConfirmAsync(a.Id, new ConfirmCartDto { Version = oldCart.Version }), "stale selection rejected");
var selected = (await cart.GetCartAsync(a.Id))!;
var subscription = await cart.ConfirmAsync(a.Id, new ConfirmCartDto { Version = selected.Version });
Check(subscription.MemberId == db.Members.Single(member => member.UserId == a.Id).Id && subscription.EmployeeId == trainer.Id && subscription.PlanId == plan.Id, "subscription uses member, plan and employee");
Check(subscription.EndDate == subscription.StartDate.AddMonths(3), "duration calculated on server");
Check((await cart.GetCartAsync(a.Id)) == null && (await subscriptions.GetUserSubscriptionsAsync(a.Id)).Count() == 1, "confirmation creates subscription and empties cart");
await RejectAsync<KeyNotFoundException>(async () => await cart.ConfirmAsync(a.Id, new ConfirmCartDto { Version = selected.Version }), "repeated confirmation creates no duplicate");
await cart.AddToCartAsync(a.Id, new AddCartDto { PlanId = plan.Id, EmployeeId = trainer.Id });
await RejectAsync<InvalidOperationException>(async () => await cart.ConfirmAsync(a.Id, new ConfirmCartDto { Version = (await cart.GetCartAsync(a.Id))!.Version }), "second active subscription rejected");
Check((await cart.RemoveFromCartAsync(a.Id)) && !(await cart.RemoveFromCartAsync(a.Id)), "cart removal");
users.UpdateProfile(a.Id, new UpdateProfileDto { Name = "Updated member", Email = "updated@example.test", Phone = "123" });
Check(db.Members.Single(member => member.UserId == a.Id).Name == "Updated member" && users.GetProfile(a.Id)!.Phone == "123", "profile and member stay linked");
var roles = new Role { Name = "Admin", Permissions = new List<Permission> { new() { Name = "Read" }, new() { Name = "Create" }, new() { Name = "Edit" }, new() { Name = "Delete" } } };
db.Roles.Add(roles); db.SaveChanges();
uow.UserRepo.SetUserRoles(a.Id, new List<int> { roles.Id }); uow.Save();
Check(users.GetSession(a.Username)!.Permissions.Length == 4 && users.GetUserRoles(a.Id)!.Roles.Single().IsSelected, "existing roles and permissions preserved");
var hash = a.HashPassword;
users.Update(new UpdateUserDto { Id = a.Id, Name = a.Name, Email = a.Email, Phone = a.Phone, Username = a.Username, Password = "" });
Check(a.HashPassword == hash, "MVC edit with blank password preserves hash");
users.Update(new UpdateUserDto { Id = a.Id, Name = a.Name, Email = a.Email, Phone = a.Phone, Username = a.Username, Password = "Changed123" });
Check(users.Login(new LoginDto { Username = a.Username, Password = "Changed123" }) && a.Password == "", "MVC password change updates hash");
// A competing request changed or removed this cart after another request read it.
await cart.AddToCartAsync(b.Id, new AddCartDto { PlanId = express.Id, EmployeeId = trainer.Id });
using (var competing = new AppDbContext(options))
{
    var competingCart = competing.CartItems.Single(item => item.UserId == b.Id);
    competingCart.Version = Guid.NewGuid(); competing.SaveChanges();
}
await RejectAsync<DbUpdateConcurrencyException>(async () => await cart.ConfirmAsync(b.Id, new ConfirmCartDto { Version = (await cart.GetCartAsync(b.Id))!.Version }), "concurrent cart update rolls back confirmation");
db.ChangeTracker.Clear();
Check(!(await subscriptions.GetUserSubscriptionsAsync(b.Id)).Any() && (await cart.GetCartAsync(b.Id)) != null, "failed confirmation leaves no partial subscription");
Console.WriteLine("All mobile service integration checks passed.");

sealed class TestEnvironment : IWebHostEnvironment
{
    public string ApplicationName { get; set; } = "Tests";
    public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();
    public string WebRootPath { get; set; } = Path.GetTempPath();
    public string EnvironmentName { get; set; } = "Development";
    public string ContentRootPath { get; set; } = Path.GetTempPath();
    public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
}
