using NLog;
using NLog.Web;
using RestaurantAPI.Data;
using RestaurantAPI.Service;

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

            builder.Services.AddControllers();
            builder.Services.AddDbContext<RestaurantDbContext>();
            builder.Services.AddScoped<RestaurantSeeder>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddScoped<IRestaurantService, RestaurantService>();

            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            //SeedDatabase();
            

            app.UseHttpsRedirection();

            app.UseAuthorization();


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