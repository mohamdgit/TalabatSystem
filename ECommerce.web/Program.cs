
using Azure;
using ECommerce.Abstraction.IServices;
using ECommerce.Domain.Contracts.Repos;
using ECommerce.Domain.Contracts.Seeding;
using ECommerce.Domain.Contracts.UOW;
using ECommerce.Domain.Models.Identity.Models;
using ECommerce.Persistence.Contexts;
using ECommerce.Persistence.Repos;
using ECommerce.Persistence.UOW;
using ECommerce.Service.MappingProfiles;
using ECommerce.Service.Services;
using ECommerce.Shared.ErrorModels;
using ECommerce.web.CustomMiddlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

namespace ECommerce.web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            #region DataBases Connection
            builder.Services.AddDbContext<StoreDbContext>(options =>
            { options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); });

            builder.Services.AddDbContext<StoreIdentityDbContext>(options =>
            { options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")); });

            builder.Services.AddSingleton<IConnectionMultiplexer>((_) =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection"));
            });
            #endregion

            #region Scoped Service
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IDataSeeding, DataSeeding>();
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            builder.Services.AddScoped<IBasketRepository, BasketRepository>(); 
            #endregion

            builder.Services.AddAutoMapper(M => M.AddProfile(new ProjectProfile(builder.Configuration)));
            builder.Services.Configure<ApiBehaviorOptions>(option =>
            {
                option.InvalidModelStateResponseFactory = (context) =>
                {
                    var Errors = context.ModelState.Where(m => m.Value.Errors.Any())
                                                   .Select(M => new ValidationError()
                                                   {
                                                       Field = M.Key,
                                                       Errors = M.Value.Errors.Select(e => e.ErrorMessage)
                                                   });
                    var Response = new ValidationErrorToReturn()
                    {
                        ValidationErrors = Errors
                    };

                    return new BadRequestObjectResult(Response);
                };

            });
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<StoreIdentityDbContext>();
            #region Identity
            builder.Services.AddAuthentication(config =>
                {
                    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration.GetSection("SecurityKey")["Issuer"],
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration.GetSection("JWTOptions")["Audience"],
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWTOptions")["SecurityKey"])),

                    };

                });
            #endregion


            //  Swagger  services
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "E-Commerce API",
                    Version = "v1",
                    Description = "API for ECommerce"
                });
            });


            var app = builder.Build();

            var scope = app.Services.CreateScope();
            var objectSeeding = scope.ServiceProvider.GetRequiredService<IDataSeeding>();
            objectSeeding.DataSeedAsync();
            objectSeeding.IdentityDataSeedAsync();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Commerce API v1");
                    c.RoutePrefix = string.Empty;
                });
            }
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseMiddleware<CustomExceptionMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.MapControllers();

            app.Run();
        }
    }
}
