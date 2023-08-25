using LoggerService;
using Contracts.Interfaces;
using Entities.Models;
using Repository;
using CompanyEmployees.Formatters;

using System.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Marvin.Cache.Headers;
using AspNetCoreRateLimit;
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
}
