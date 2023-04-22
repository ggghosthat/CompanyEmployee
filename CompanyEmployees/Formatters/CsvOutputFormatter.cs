using Entities.DTO;

using System;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
namespace CompanyEmployees.Formatters;
public class CsvOutputFormatter : TextOutputFormatter
{
	//define output format & encoding types
	public CsvOutputFormatter()
	{
		SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
		SupportedEncodings.Add(Encoding.UTF8);
		SupportedEncodings.Add(Encoding.Unicode);
	}

	//define types which can write out
	protected override bool CanWriteType(Type type)
	{
		if (typeof(CompanyDto).IsAssignableFrom(type) || 
			typeof(IEnumerable<CompanyDto>).IsAssignableFrom(type))
		{
			return base.CanWriteType(type);
		}

		return false;
	}

	//handling response & writting out the staff according predefined format
	public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context,
													  Encoding selectedEncoding)
	{
		var response = context.HttpContext.Response;
		var buffer = new StringBuilder();

		if (context.Object is IEnumerable<CompanyDto> companies)
		{
			foreach (var company in companies)
			{
				FormatCsv(buffer, company);
			}
		}
		else if (context.Object is CompanyDto company)
		{
			FormatCsv(buffer, company);
		}

		await response.WriteAsync(buffer.ToString());
	}

	//writting csv of type
	private static void FormatCsv(StringBuilder buffer, CompanyDto company)
	{
		buffer.Append($"{company.Id},\"{company.Name},\"{company.FullAddress}\"");
	}
}