using Contracts.Interfaces;
using Entities.DTO;
using Entities.Models;

using Microsoft.AspNetCore.Mvc;
using AutoMapper;
namespace CompanyEmployees.Controllers;
//These controller used for handling employees requests
[ApiController]
[Route("api/companies/{companyId}/employees")]
public class EmployeesController : ControllerBase
{
	private readonly IRepositoryManager _repositoryManager;
	private readonly ILoggerManager _loggerManager;
	private readonly IMapper _mapper;

	public EmployeesController(IRepositoryManager repositoryManager,
							   ILoggerManager loggerManager,
							   IMapper mapper)
	{
		_repositoryManager = repositoryManager;
		_loggerManager = loggerManager;
		_mapper = mapper;
	}

	// [HttpGet("{id}", Name="GetEmployeeForCompany")]
	[HttpGet]
	public IActionResult GetEmployeesForCompany(Guid companyId)
	{
		var company = _repositoryManager.Company.GetCompany(companyId, false);

		if(company == null)
		{
			_loggerManager.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
			return NotFound();
		}

		var employees = _repositoryManager.Employee.GetEmployees(companyId, false);

		var employeeDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

		return Ok(employeeDto);
	}

	[HttpGet("{id}", Name="GetEmployeeForCompany")]
	public IActionResult GetEmployee(Guid companyId, Guid id)
	{
		var company = _repositoryManager.Company.GetCompany(companyId, trackChanges: false);
		if(company == null)
		{
			_loggerManager.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
			return NotFound();
		}

		var employee = _repositoryManager.Employee.GetEmployee(companyId, id, false);
		if(employee == null)
		{
			_loggerManager.LogInfo($"Employee with id: {id} doesn't exist in the database.");
			return NotFound();
		}

		var employeeDto = _mapper.Map<EmployeeDto>(employee);

		return Ok(employeeDto);
	}


	public IActionResult CreateEmployeeForCompany(Guid companyId, [FromBody]EmployeeForCreationDto employee)
	{
		if(employee == null)
		{
			_loggerManager.LogError("EmployeeForCreationDto object sent from client is null");
			return BadRequest("EmployeeForCreationDto object is null");
		}

		var company = _repositoryManager.Company.GetCompany(companyId, trackChanges: false);
		if(company == null)
		{
			_loggerManager.LogInfo($"Company with id: {companyId} doesn't exist int the database.");
			return NotFound();
		}

		var employeeEntity = _mapper.Map<Employee>(employee);

		_repositoryManager.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
		_repositoryManager.Save();

		var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

		return CreatedAtRoute("GetEmployeeForCompany", new {companyId, id = employeeToReturn.Id}, employeeToReturn);
	}
}