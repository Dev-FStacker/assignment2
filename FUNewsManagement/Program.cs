using BLL.Hubs;
using BLL.Interface;
using BLL.Services;
using DAL.Interface;
using DAL.Repo;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddSignalR();
            builder.Services.AddDbContext<DAL.Models.FunewsManagementContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<INewsArticleService, NewsArticleService>();
            builder.Services.AddScoped<INewsArticleRepo, NewsArticleRepo>();
            builder.Services.AddScoped<IUserRepo, UserRepo>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<ICateRepo, CateRepo>();
            builder.Services.AddScoped<ICateService, CateService>();
            builder.Services.AddScoped<ITagRepo, TagRepo>();
            builder.Services.AddScoped<ITagService, TagService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();
            app.MapHub<CategoryHub>("/categoryHub");
            app.MapHub<NewsHub>("/newsHub");
            app.Run();
        }
    }
}
