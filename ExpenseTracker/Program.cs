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

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Configure SQLite database
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                                ?? "Data Source=expense_tracker.sqlite";

            builder.Services.AddDbContext<ExpenseTrackerDbContext>(options =>
                options.UseSqlite(connectionString));

            // Configure Identity with custom User and Role types
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                // Configure identity options if needed
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ExpenseTrackerDbContext>()
                .AddDefaultTokenProviders();

            // Register your custom UserContext service
            builder.Services.AddScoped<UserContext>();

            // Add session support if needed
            builder.Services.AddSession();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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

            // Important: Authentication before Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            // Add session middleware if needed
            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Apply database migrations automatically
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ExpenseTrackerDbContext>();
                    context.Database.Migrate(); // Apply pending migrations

                    // Seed initial data if needed
                    // await SeedData.Initialize(services);
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