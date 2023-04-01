using Contracts.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Controllers;

//These controller used for handling companies requests
[ApiController]
[Route("api/companies")]
public class CompaniesController : ControllerBase
{
	private readonly IRepositoryManager _repositoryManager;
	private readonly ILoggerManager _loggerManager;

	public CompaniesController(IRepositoryManager repositoryManager,
							   ILoggerManager loggerManager)
	{
		_repositoryManager = repositoryManager;
		_loggerManager = _loggerManager;
	}

	[HttpGet]
	public IActionResult GetCompanies()
	{
		try
		{
			var companies = _repositoryManager.Company.GetAllCompanies(false);
			return Ok(companies);
		}
		catch(Exception ex)
		{
			_loggerManager.LogError($"Something went wrong in the {nameof(GetCompanies)} action \n{ex}");
			return StatusCode(500, "Internal server error");
		}
	}
}