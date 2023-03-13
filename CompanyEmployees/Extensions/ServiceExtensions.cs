using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

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
}