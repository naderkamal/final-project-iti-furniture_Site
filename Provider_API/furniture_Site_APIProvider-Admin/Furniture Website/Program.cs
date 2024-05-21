using Microsoft.AspNetCore.Identity;
using Furniture_Website.Models;
using Microsoft.EntityFrameworkCore;

namespace Furniture_Website
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<AdminDashboardMVC_DBContext>(Use =>
            Use.UseSqlServer(builder.Configuration.GetConnectionString("BaseConnection")));

    //        builder.Services.AddIdentityCore<AspNetUser>(options => options.SignIn.RequireConfirmedAccount = false)
    //.AddEntityFrameworkStores<AdminDashboardMVC_DBContext>();
            builder.Services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("PlanPolicy1", PolicyBuilder =>
                {
                    PolicyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("PlanPolicy1");
            //app.UseCors(option => option.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod());
            //app.UseCors(option => option.WithOrigins("http://localhost:52549").AllowAnyHeader().AllowAnyMethod());
            app.UseAuthorization();
            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}