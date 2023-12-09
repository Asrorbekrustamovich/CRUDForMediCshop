
using CrudforMedicshop.Application.Interfaces;
using CrudforMedicshop.Domain.Entities;
using CrudforMedicshop.infrastructure.Dbcontext;
using CrudforMedicshop.infrastructure.Repositories;
using CrudforMedicshop.infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using CrudforMedicshop.Application;
using CrudforMedicshop.Application.mapping;
using Microsoft.AspNetCore.RateLimiting;
using CrudforMedicshop.Application.Validation;
using FluentValidation.AspNetCore;
using FluentValidation;
using System.Threading.RateLimiting;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using CrudforMedicshop.infrastructure.ExceptionHandler;

namespace CrudforMedicshop
{
    public class Program
    {
        public static  void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<Irepository<Medicine>, Repository>();
            builder.Services.AddScoped<Iservice<Medicine>, Service>();
            //builder.Services.AddDbContext<Mydbcontext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddDbContext<ApplicationDbcontext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.addmapping();
            builder.Services.AddIdentity<ApplicationUser,IdentityRole>().AddEntityFrameworkStores<ApplicationDbcontext>().AddDefaultTokenProviders();
            builder.Services.AddFluentValidation();
            builder.Services.AddScoped<IAuthService,AuthService>();
            builder.Services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();
            builder.Services.Configure<RateLimiterOptions>(o => o
                 .AddFixedWindowLimiter(policyName: "fixed", options =>
                 {
                     options.PermitLimit = 1;
                     options.QueueLimit = 4;
                     options.Window=TimeSpan.FromSeconds(30);
                     options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                 }));
            builder.Services.AddRateLimiter(_ => _
       .AddSlidingWindowLimiter(policyName: "sliding", options =>
         {
          options.PermitLimit = 5;
          options.Window = TimeSpan.FromSeconds(1);
          options.SegmentsPerWindow = 4;
          options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
          options.QueueLimit = 5;
         }));
            builder.Services.AddRateLimiter(_ => _.AddTokenBucketLimiter(policyName: "Tokenbox", opt =>
            {
                opt.ReplenishmentPeriod = TimeSpan.FromSeconds(30);
                opt.QueueLimit = 5;
                opt.TokenLimit = 1;
                opt.TokensPerPeriod = 8; 
                opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            }
            ));
            builder.Services.AddRateLimiter(_ => _
    .AddConcurrencyLimiter(policyName:" concurrencyPolicy", options =>
    {
        options.PermitLimit = 4;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 5;
    }));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWTKey:ValidIssuer"],
                    ValidAudience = builder.Configuration["JWTKey:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTKey:Secret"]))
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("istokentExpired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseAuthentication();
            app.UseMiddleware<ExceptionMiddlerWare>();
            app.UseRateLimiter();
            app.MapControllers();

            app.Run();
        }
    }
}