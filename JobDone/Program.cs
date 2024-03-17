using JobDone.Data;
using JobDone.Models.Admin;
using JobDone.Models.Customer;
using JobDone.Models.SecurityQuestions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore.Metadata;
using JobDone.Models.Customer;
using JobDone.Models.SecurityQuestions;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    option.LoginPath = "/Customer/Login";
    option.ExpireTimeSpan = TimeSpan.Zero;
});

builder.Services.AddDbContext<JobDoneContext>(options =>
    options.UseSqlServer("Server=HP-LAB\\MSSQLSERVER02;initial catalog=JobDone; database=JobDone; trusted_connection=True; TrustServerCertificate=True"));



//interface regestration
builder.Services.AddTransient<IAdmin, AdminImplementation>();
builder.Services.AddTransient<ICustomer, CustomerImplementation>();
builder.Services.AddTransient<ISecurityQuestion, SecurityQuestionsImplementation>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Customer}/{action=SignUp}/{id?}");

app.Run();
