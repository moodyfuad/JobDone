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
using JobDone.Models.Category;
using JobDone.Models.Seller;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<JobDoneContext>(options =>
    options.UseSqlServer("Server=DESKTOP-K50E369;initial catalog=JobDone; database=JobDone; trusted_connection=True; TrustServerCertificate=True"));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    option.LoginPath = "/Customer/Login";
    option.ExpireTimeSpan = TimeSpan.Zero;
});

string connectionString = builder.Configuration.GetConnectionString("con");
builder.Services.AddDbContext<JobDoneContext>(options =>
    options.UseSqlServer(connectionString));


//rejester the session for authentication
builder.Services.AddSession(x => x.IdleTimeout = TimeSpan.FromDays(1));



//interface regestration
builder.Services.AddTransient<IAdmin, AdminImplementation>();
builder.Services.AddTransient<ICustomer, CustomerImplementation>();
builder.Services.AddTransient<ISecurityQuestion, SecurityQuestionsImplementation>();
builder.Services.AddTransient<ISeller, SellerImplemntation>();
builder.Services.AddTransient<ICategory, CatgegoryImplementation>();


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
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Customer}/{action=SignUp}/{id?}");

app.Run();
