using DataAccess.DataManipulation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Models.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Utility;
using DataAccess.Repository.IRepository;
using DataAccess.Repository;
using DataAccess.BackgroundProcess;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Builder;
using DataAccess;

var builder = WebApplication.CreateBuilder(args);
//
//	TRANSALTE
//	EMAILS
//	LOGS
//
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => 
													options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddSingleton<ZoomMenager, ZoomMenager>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddHostedService<StatusUpdateService>();
builder.Services.AddSignalR();
builder.Services.AddRazorPages();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
	options.SignIn.RequireConfirmedAccount = false;
	options.Password.RequireDigit = false;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;

}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapRazorPages();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<TokenHub>("/tokenHub");
	endpoints.MapControllerRoute(
    name: "default",
    pattern: "{area=Control}/{controller=Home}/{action=Index}/{id?}");
});
//app.MapControllerRoute(
//	name: "default",
//	pattern: "{area=Control}/{controller=Home}/{action=Index}/{id?}");


app.Run();
