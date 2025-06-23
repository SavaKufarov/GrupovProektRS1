using Microsoft.EntityFrameworkCore;
using DataLayer;
using BusinessLayer;
using Microsoft.AspNetCore.Identity;
using ExpenseTracker.Controllers;

namespace ExpenseTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            builder.Services.AddControllersWithViews();

            
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                                ?? "Data Source=expense_tracker.sqlite";

            builder.Services.AddDbContext<ExpenseTrackerDbContext>(options =>
                options.UseSqlite(connectionString));

            
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ExpenseTrackerDbContext>()
                .AddDefaultTokenProviders();

            
            builder.Services.AddScoped<UserContext>();
            builder.Services.AddScoped<ExpenseContext>();
            builder.Services.AddScoped<CategoryContext>();


            builder.Services.AddSession();

            var app = builder.Build();

            
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            
            app.UseAuthentication();
            app.UseAuthorization();

            
            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ExpenseTrackerDbContext>();
                    context.Database.Migrate();
                    
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }

            app.Run();
        }
    }
}