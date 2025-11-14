using ApiProj.Infrastructure;
using ApiProj.Interfaces;
using ApiProj.Repositories;
using ApiProj.Services;
using Microsoft.EntityFrameworkCore;

namespace ApiProj
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString =
                builder.Configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Connection string"
                    + "'DefaultConnection' not found.");


            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();

            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection(); //redirect to https for safe 

            app.UseAuthorization();


            app.MapControllers(); //map controller routes to the request pipeline

            app.Run();
        }
    }
}
