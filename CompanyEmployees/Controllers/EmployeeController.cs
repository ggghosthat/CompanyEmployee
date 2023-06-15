using Contracts.Interfaces;
using Entities.DTO;
using Entities.Models;
using Entities.RequestFeatures;
using CompanyEmployees.ActionFilters;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
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

	[HttpGet]
	public async Task<IActionResult> GetEmployeesForCompany(Guid companyId, [FromQuery] EmployeeParameters employeeParameters)
	{
		var company = await _repositoryManager.Company.GetCompanyAsync(companyId, false);

		if(company == null)
		{
			_loggerManager.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
			return NotFound();
		}

		var employees = await _repositoryManager.Employee.GetEmployeesAsync(companyId, employeeParameters, false);

        Response.Headers.Add("X-Pagination",
                             JsonConvert.SerializeObject(employees.MetaData));

		var employeeDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

		return Ok(employeeDto);
	}

	[HttpGet("{id}", Name="GetEmployeeForCompany")]
	public async Task<IActionResult> GetEmployee(Guid companyId, Guid id)
	{
		var company = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges: false);
		if(company == null)
		{
			_loggerManager.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
			return NotFound();
		}

		var employee = await _repositoryManager.Employee.GetEmployeeAsync(companyId, id, false);
		if(employee == null)
		{
			_loggerManager.LogInfo($"Employee with id: {id} doesn't exist in the database.");
			return NotFound();
		}

		var employeeDto = _mapper.Map<EmployeeDto>(employee);

		return Ok(employeeDto);
	}

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
	public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody]EmployeeForCreationDto employee)
	{
		var company = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges: false);
		if(company == null)
		{
			_loggerManager.LogInfo($"Company with id: {companyId} doesn't exist int the database.");
			return NotFound();
		}

		var employeeEntity = _mapper.Map<Employee>(employee);

		_repositoryManager.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
		await _repositoryManager.SaveAsync();

		var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

		return CreatedAtRoute("GetEmployeeForCompany", new {companyId, id = employeeToReturn.Id}, employeeToReturn);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid id)
	{
		var company = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges: false);
		if (company == null)
		{
			_loggerManager.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
			return NotFound();
		}

		var employeeForCompany = await _repositoryManager.Employee.GetEmployeeAsync(companyId, id, trackChanges: false);
		if(employeeForCompany == null)
		{
			_loggerManager.LogInfo($"Employee with id: {id} doesn't exist in the database.");
			return NotFound();
		}

		_repositoryManager.Employee.DeleteEmployee(employeeForCompany);
		await _repositoryManager.SaveAsync();

		return NoContent();
	}

    [HttpPut]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody]EmployeeForUpdateDto employee)
    {
        var company = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges: false);
        if(company == null)
        {
            _loggerManager.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
            return NotFound();
        }

        var employeeEntity = await _repositoryManager.Employee.GetEmployeeAsync(companyId, id, trackChanges: true);
        if(employeeEntity == null)
        {
            _loggerManager.LogInfo($"Employee with id: {id} doesn't exist in the database.");
            return NotFound();
        }

        _mapper.Map(employee, employeeEntity);
        await _repositoryManager.SaveAsync();

        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody]JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
    {
        if(patchDoc == null)
        {
            _loggerManager.LogError("patchDoc sent from client side is null value");
            return BadRequest("patchDoc is null value");
        }

        var company = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges: false);
        if(company == null)
        {
            _loggerManager.LogInfo($"Company with id: {companyId} not found in database.");
            return NotFound();
        }

        var employeeEntity = await _repositoryManager.Employee.GetEmployeeAsync(companyId, id, trackChanges: true);
        if(employeeEntity == null)
        {
            _loggerManager.LogInfo($"Employee with id: {id} not found in database.");
            return NotFound();
        }

        var employee2Patch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);
        
        patchDoc.ApplyTo(employee2Patch, ModelState);

        TryValidateModel(employee2Patch);

        if(!ModelState.IsValid)
        {
            _loggerManager.LogError("Invalid model state for the patch document.");
            return UnprocessableEntity(ModelState);
        }

        _mapper.Map(employee2Patch, employeeEntity);
        await _repositoryManager.SaveAsync();

        return NoContent();
    }
}
