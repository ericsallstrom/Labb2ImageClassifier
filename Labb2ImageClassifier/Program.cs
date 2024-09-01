using Labb2ImageClassifier.Controllers;
using Labb2ImageClassifier.Models;
using Labb2ImageClassifier.Services;

namespace Labb2ImageClassifier
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add User Secrets
            builder.Configuration.AddUserSecrets<Program>();

            // Configure CustomVisionSettings from appsettings.json
            builder.Services.Configure<CustomVisionSettings>(builder.Configuration.GetSection("CustomVision"));

            builder.Services.AddSingleton<MushroomService>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient(); // Register HttpClient

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

            app.UseRouting();

            app.UseAuthorization();

            app.UseStatusCodePages(async context =>
            {
                if (context.HttpContext.Response.StatusCode == 404)
                {
                    context.HttpContext.Response.Redirect("/Home/NotFound");
                }
            });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Image}/{action=Index}/{fileName?}");

            app.Run();
        }
    }
}
