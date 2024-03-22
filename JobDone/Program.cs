using JobDone.Data;
using JobDone.Models.Admin;
using JobDone.Models.Customer;
using JobDone.Models.SecurityQuestions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore.Metadata;
using JobDone.Models.Category;
using JobDone.Models.Seller;
using JobDone.Models.Service;
using JobDone.Models.OrderByCustomer;
using Microsoft.AspNetCore.Mvc;
using JobDone.Models.Order;
using JobDone.ViewModels;
using JobDone.Models.Banners;
using JobDone.Models.SellerAcceptRequest;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connectionString = builder.Configuration.GetConnectionString("con");
builder.Services.AddDbContext<JobDoneContext>(options =>
    options.UseSqlServer(connectionString));

//Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
     o => {
         o.ExpireTimeSpan = TimeSpan.FromDays(364);
         o.LoginPath = "/Customer/LogIn";
         o.Cookie.Name = "JobDoneLogin";
        });
     
//interface regestration
builder.Services.AddTransient<IBanner, BannerImplementation>();
builder.Services.AddTransient<IAdmin, AdminImplementation>();
builder.Services.AddTransient<ICustomer, CustomerImplementation>();
builder.Services.AddTransient<ISecurityQuestion, SecurityQuestionsImplementation>();
builder.Services.AddTransient<ISeller, SellerImplemntation>();
builder.Services.AddTransient<ICategory, CatgegoryImplementation>();
builder.Services.AddTransient<IServies, ServiesImplemntation>();
builder.Services.AddTransient<IOrderByCustomer, OrderByCustomerImplementation>();
builder.Services.AddTransient<IOrder, OrderImplementation>();
builder.Services.AddTransient<ISellerAcceptRequest, SellerAcceptRequestImp>();

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
    pattern: "{controller=CustomerRequestWork}/{action=RequestedList}/{id?}");

app.Run();
