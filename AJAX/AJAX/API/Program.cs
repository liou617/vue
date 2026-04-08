
using API.Models;
using Microsoft.EntityFrameworkCore;
using API.Controllers;
using Microsoft.Extensions.FileProviders;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<NorthwindContext>(options => {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Northwind"));
            });

            builder.Services.AddCors(options => {
                options.AddPolicy("MVC", policy => { 
                    policy.AllowAnyHeader().WithOrigins("https://localhost:7080").AllowAnyMethod();
                });
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseStaticFiles(new StaticFileOptions {
            FileProvider=new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "StatiocFiles")),
            RequestPath= "/StatiocFiles"
			}); //指定放置靜態文件資料夾

            app.MapControllers();

            app.Run();
        }
    }
}
