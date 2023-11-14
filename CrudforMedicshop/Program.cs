
using CrudforMedicshop.Application.Interfaces;
using CrudforMedicshop.Domain.Entities;
using CrudforMedicshop.infrastructure.Dbcontext;
using CrudforMedicshop.infrastructure.Repositories;
using CrudforMedicshop.infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using CrudforMedicshop.Application;
using CrudforMedicshop.Application.mapping;

namespace CrudforMedicshop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<Irepository<Medicine>, Repository>();
            builder.Services.AddScoped<Iservice<Medicine>, Service>();
            builder.Services.AddDbContext<Mydbcontext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.addmapping();
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}