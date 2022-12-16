using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using RestaurantAPI.Data;
using RestaurantAPI.Entities;
using RestaurantAPI.Helper;
using RestaurantAPI.Middleware;
using RestaurantAPI.Models;
using RestaurantAPI.Models.Validators;
using RestaurantAPI.Service;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Authorization;

namespace RestaurantAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            logger.Debug("init main");
            // Add services to the container.

            var authenticationSettings = new AuthenticationSettings();
            builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
            builder.Services.AddSingleton(authenticationSettings);
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authenticationSettings.JwtIssuer,
                    ValidAudience = authenticationSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
                };
            });

            builder.Services.AddControllers().AddFluentValidation();
            builder.Services.AddControllers();

            builder.Services.AddDbContext<RestaurantDbContext>
                (options => options.UseSqlServer(builder.Configuration.GetConnectionString("RestaurantDbConnection")));

            builder.Services.AddScoped<RestaurantSeeder>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddScoped<IRestaurantService, RestaurantService>();
            builder.Services.AddScoped<IDishService, DishService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("HasNationality", builder => builder.RequireClaim("Nationality", "German", "Poland"));
                options.AddPolicy("Atleast20", builder => builder.AddRequirements(new MinimumAgeRequirement(20)));
                options.AddPolicy("Atleast2restaurants", builder => builder.AddRequirements(new AmountOfCreatedRestaurantRequirement(2)));
            });
            builder.Services.AddScoped<IAuthorizationHandler, AmountOfCreatedRestaurantRequirementHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();
            builder.Services.AddScoped<IUserContextService, UserContextService>();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<IValidator<RestaurantQuery>, RestaurantQueryValidator>();
            builder.Services.AddScoped<ErrorHandlingMiddleware>();
            builder.Services.AddScoped<RequestTimeMiddleware>();
            builder.Services.AddSwaggerGen();

            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("FrontEndClient", policy =>
                    policy.AllowAnyMethod()
                        .AllowAnyHeader()
                        //.WithOrigins(builder.Configuration["AllowedOrigins"])
                        .AllowAnyOrigin()
                    );
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            SeedDatabase();

            app.UseResponseCaching();
            app.UseStaticFiles();
            app.UseCors("FrontEndClient");
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<RequestTimeMiddleware>();
            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant API");
            });

            app.UseAuthorization();
            app.UseStaticFiles();

            app.MapControllers();

            app.Run();

            void SeedDatabase() //can be placed at the very bottom under app.Run()
            {
                using (var scope = app.Services.CreateScope())
                {
                    var dbInitializer = scope.ServiceProvider.GetRequiredService<RestaurantSeeder>();
                    dbInitializer.Seed();
                }
            }
        }
    }
}