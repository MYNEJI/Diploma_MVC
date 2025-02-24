using Diploma.DataAccess.Data;
using Diploma.DataAccess.Repository;
using Diploma.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Diploma.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;


//////Add-Migration AddCategoryToDbAndSeedTable
//////Update-database
namespace DiplomaWork
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllersWithViews();
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
					options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
			builder.Services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = $"/Identity/Account/Login";
				options.LogoutPath = $"/Identity/Account/Logout";
				options.AccessDeniedPath = $"/Identity/Account/AccessDeniedPath";
			});
			builder.Services.AddRazorPages();
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
			builder.Services.AddScoped<IEmailSender, EmailSender>();
			var app = builder.Build();

			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
			app.MapRazorPages();
			app.MapStaticAssets();
			app.MapControllerRoute(
				name: "default",
				pattern: "{area=Manager}/{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}
