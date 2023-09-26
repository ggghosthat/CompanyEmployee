using LoggerService;
using Contracts.Interfaces;
using Entities.Models;
using Repository;
using CompanyEmployees.Formatters;

using System.Text;
using System.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Marvin.Cache.Headers;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
namespace CompanyEmployees.Extensions;
//This Extension class provides speciall extenssion methods to work with some services
public static class ServiceExtensions
{

	//Here we allow every request from any domain
	public static void ConfigureCors(this IServiceCollection services) =>
		services.AddCors(options => 
		{
            options.AddPolicy("CorsPolicy", builder =>
				builder.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader());
		});

	//Here we can configure IIS properties
	public static void ConfigureIISIntegration(this IServiceCollection services) => 
		services.Configure<IISOptions>(options =>
		{

		});

	//Provides logger service
	public static void ConfigureLoggerService(this IServiceCollection services) =>
		services.AddTransient<ILoggerManager, LoggerManager>();

	//configuring sql connection for MsSql server
	public static void ConfigureSqlContext(this IServiceCollection services,
												IConfiguration configuration) =>
		services.AddDbContext<RepositoryContext>(opts =>
			opts.UseSqlServer(configuration.GetConnectionString("sqlConnection"), b => 
				b.MigrationsAssembly("CompanyEmployees")));

    public static void ConfigurePostgresContext(this IServiceCollection services,
                                                     IConfiguration configuration) =>
        services.AddDbContext<RepositoryContext>(opts => 
            opts.UseNpgsql(configuration.GetConnectionString("postgresConnection"), b =>
                b.MigrationsAssembly("CompanyEmployees")));


	public static void ConfigureRepositoryManager(this IServiceCollection services) =>
		services.AddScoped<IRepositoryManager, RepositoryManager>();

	public static IMvcBuilder AddCustomCSVFormatter(this IMvcBuilder builder) => 
		builder.AddMvcOptions(config => config.OutputFormatters.Add(new CsvOutputFormatter()));

    public static void ConfigureResponseCaching(this IServiceCollection services) =>
        services.AddResponseCaching();

    public static void ConfigureHttpCacheHeaders(this IServiceCollection services) =>
        services.AddHttpCacheHeaders(
            (expirationOpt) => 
            {
                expirationOpt.MaxAge = 60;
                expirationOpt.CacheLocation = CacheLocation.Private;
            },
            (validationOpt) =>
            {
                validationOpt.MustRevalidate = true;
            }
        );
    public static IServiceCollection ConfigureRateLimiting(this IServiceCollection services, 
                                            IConfiguration configuration) 
    {
        services.AddMemoryCache();

        services.Configure<IpRateLimitOptions>(opt => configuration.GetSection("IpRateLimitingSettings").Bind(opt) );

        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddInMemoryRateLimiting();

        return services;
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentityCore<User>(o =>
        {
            o.Password.RequireDigit = true;
            o.Password.RequireLowercase = false;
            o.Password.RequireUppercase = false;
            o.Password.RequireNonAlphanumeric = false;
            o.Password.RequiredLength = 10;
            o.User.RequireUniqueEmail = true;
        });

        builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
        builder.AddEntityFrameworkStores<RepositoryContext>()
            .AddDefaultTokenProviders();
    }

    public static void ConfigureJWT(this IServiceCollection services,
                                         IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = Environment.GetEnvironmentVariable("SECRET1");

        services.AddAuthentication(opt => 
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options => 
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
               ValidateIssuer = true,
               ValidateAudience = false,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,

               ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
               ValidAudience = jwtSettings.GetSection("valueAudience").Value,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };
        });
    }
}
