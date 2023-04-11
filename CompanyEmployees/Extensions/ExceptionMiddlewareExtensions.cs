using LoggerService;
using Contracts.Interfaces;
using Entities.Models;
using Entities.ErrorModel;
using Repository;

using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
namespace CompanyEmployees.Extensions;
public static class ExceptionMiddlewareExtensions
{
	 public static void ConfigureExceptionHandler(this IApplicationBuilder app,	ILoggerManager logger)
	 {
		app.UseExceptionHandler(appError =>
		{
	 		appError.Run(async context =>
	 		{
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
	 			context.Response.ContentType = "application/json";
	 			var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
	 			if (contextFeature != null)
	 			{
	 				logger.LogError($"Something went wrong: {contextFeature.Error}");
	 				await context.Response.WriteAsync(new ErrorDetails()
	 				{
	 					StatusCode = context.Response.StatusCode,
						Message = "Internal Server Error."
	 				}.ToString());
	 			}
	 		});
	 	});
	}
}