using JobDone.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore.Metadata;
using JobDone.Models.Customer;
using JobDone.Models.SecurityQuestions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<JobDoneContext>(options =>
    options.UseSqlServer("Server=HP-LAB\\MSSQLSERVER02;initial catalog=JobDone; database=JobDone; trusted_connection=True; TrustServerCertificate=True"));

builder.Services.AddScoped<ICustomer, CustomerImplementation>();
builder.Services.AddScoped<ISecurityQuestion, SecurityQuestionsImplementation>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
