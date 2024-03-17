using JobDone.Data;
using JobDone.Models.Category;
using JobDone.Models.SecurityQuestions;
using JobDone.Models.Seller;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<JobDoneContext>(options =>
    options.UseSqlServer("Server=DESKTOP-K50E369;initial catalog=JobDone; database=JobDone; trusted_connection=True; TrustServerCertificate=True"));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ISeller,SellerImplemntation>();
builder.Services.AddTransient<ICategory,CatgegoryImplementation>();
builder.Services.AddTransient<ISecurityQuestion,SecurityQuestionsImplementation>();

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
