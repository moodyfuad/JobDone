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
using JobDone.Models.SellerProfile;
using JobDone.Models.SellerOldWork;
using JobDone.Models.MessageModel;
using JobDone.Models.Withdraw;
using JobDone.Models.ForgetAndChangePassword;


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
         o.Cookie.Name = "JobDoneLogin";
         o.AccessDeniedPath= "/Home/Index";
        });

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(30);
    options.Cookie.IsEssential = true;
});

builder.Services.AddResponseCompression();

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
builder.Services.AddTransient<ISellerProfile, SellerProfileImplemntation>();
builder.Services.AddTransient<ISellerOldWork, SellerOldWorksImp>();
builder.Services.AddTransient<IMessage, MessageImplementation>();
builder.Services.AddTransient<IWithdraw, WithdrawImplementation>();
builder.Services.AddTransient<IForgetAndChanePassword, ForgetAndChangePasswordImplementaion>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

//This middleware enables response caching in the application.
//Caching allows the server to store the response of a request and reuse it for subsequent identical requests,
//reducing the processing time and improving performance.
app.UseResponseCaching();
//This middleware enables response compression in the application.
//Response compression reduces the size of the HTTP response by compressing the content,
//which can significantly improve the network transfer time and reduce bandwidth usage. 
app.UseResponseCompression();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}/{username?}");

app.Run();
