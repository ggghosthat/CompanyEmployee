using CompanyEmployees.Extensions;
using CompanyEmployees.ActionFilters;
using Contracts.Interfaces;
using Entities.DTO;
using LoggerService;
using Repository.Shapper;

using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using AspNetCoreRateLimit;
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
var builder = WebApplication.CreateBuilder(args);

//store configuration file
IConfiguration configuration = builder.Configuration;

// Add services to the container.

builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureResponseCaching();

//builder.Services.ConfigureSqlContext(configuration);
builder.Services.ConfigurePostgresContext(configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureHttpCacheHeaders();
builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies() );
builder.Services.ConfigureRateLimiting(configuration);

builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();

builder.Services.AddControllers(config =>
{
	config.RespectBrowserAcceptHeader = true;
	config.ReturnHttpNotAcceptable = true;
    config.CacheProfiles.Add("120SecondsDuration", new CacheProfile {Duration = 120});
}).AddNewtonsoftJson()
  .AddXmlDataContractSerializerFormatters()
  .AddCustomCSVFormatter();

var app = builder.Build();
ILoggerManager logger = app.Services.GetRequiredService<ILoggerManager>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler(app.Services.GetService<ILoggerManager>());
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseForwardedHeaders(new ForwardedHeadersOptions()
{
	ForwardedHeaders = ForwardedHeaders.All
});

app.UseResponseCaching();
app.UseHttpCacheHeaders();

app.UseIpRateLimiting();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
