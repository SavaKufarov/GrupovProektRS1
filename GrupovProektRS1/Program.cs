using GrupovProektRS1.Data;
using Microsoft.EntityFrameworkCore;

namespace GrupovProektRS1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Добавяме контролери с изгледи (MVC)
            builder.Services.AddControllersWithViews();

            // Връзка към базата данни (чете от appsettings.json)
            builder.Services.AddDbContext<ExpenseContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // Използваме developer exception page при разработка
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Грешки и HTTPS в production
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // Дефинираме default route – ще отвори /Expenses по подразбиране
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Expenses}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
