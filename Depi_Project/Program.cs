using Depi_Project.Data;
using Depi_Project.Models;
using Depi_Project.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Depi_Project
{
    public class Program
    {
        public static  async Task  Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            //                .AddEntityFrameworkStores<ApplicationDbContext>()
            //                .AddDefaultTokenProviders();



            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();

            builder.Services.AddControllersWithViews();

            // Register services
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            //builder.Services.AddScoped<INotificationService, NotificationService>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

			// Configure Stripe
			StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];


			var app = builder.Build();


            // 3. Seed Roles
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await Seeder.SeedRoles(roleManager);
            }


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            //app.Run();
            await app.RunAsync();
        }
    }
}
