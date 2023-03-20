using LoggerService;
using Contracts.Interfaces;
using Entities.Models;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace CompanyEmployees.Extensions;
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

	public static void ConfigureLoggerService(this IServiceCollection services) =>
		services.AddScoped<ILoggerManager, LoggerManager>();

	public static void ConfigureSqlContext(this IServiceCollection services,
												IConfiguration configuration) =>
		services.AddDbContext<RepositoryContext>(opts =>
			opts.UseSqlServer(configuration.GetConnectionString("sqlConnection"), b => 
				b.MigrationsAssembly("CompanyEmployees")));
}