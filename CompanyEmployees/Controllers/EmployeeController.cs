using Contracts.Interfaces;
using Entities.DTO;
using Entities.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
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

        if(!ModelState.IsValid)
        {
            _loggerManager.LogError("Invalid data for model state of EmployeeForCreationDto object.");
            return UnprocessableEntity(ModelState);
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

	[HttpDelete("{id}")]
	public IActionResult DeleteEmployeeForCompany(Guid companyId, Guid id)
	{
		var company = _repositoryManager.Company.GetCompany(companyId, trackChanges: false);
		if (company == null)
		{
			_loggerManager.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
			return NotFound();
		}

		var employeeForCompany = _repositoryManager.Employee.GetEmployee(companyId, id, trackChanges: false);
		if(employeeForCompany == null)
		{
			_loggerManager.LogInfo($"Employee with id: {id} doesn't exist in the database.");
			return NotFound();
		}

		_repositoryManager.Employee.DeleteEmployee(employeeForCompany);
		_repositoryManager.Save();

		return NoContent();
	}

    [HttpPut("{id}")]
    public IActionResult UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody]EmployeeForUpdateDto employee)
    {
        if(employee == null)
        {
            _loggerManager.LogError("EmployeeForUpdateDto object sent from client is null.");
            return BadRequest("EmployeeForUpdateDto object is null");
        }

        if(!ModelState.IsValid)
        {
            _loggerManager.LogError("Invalid model state for the EmployeeForUpdateDto object.");
            return UnprocessableEntity(ModelState);
        }

        var company = _repositoryManager.Company.GetCompany(companyId, trackChanges: false);
        if(company == null)
        {
            _loggerManager.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
            return NotFound();
        }

        var employeeEntity = _repositoryManager.Employee.GetEmployee(companyId, id, trackChanges: true);
        if(employeeEntity == null)
        {
            _loggerManager.LogInfo($"Employee with id: {id} doesn't exist in the database.");
            return NotFound();
        }

        _mapper.Map(employee, employeeEntity);
        _repositoryManager.Save();

        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody]JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
    {
        if(patchDoc == null)
        {
            _loggerManager.LogError("patchDoc sent from client side is null value");
            return BadRequest("patchDoc is null value");
        }

        var company = _repositoryManager.Company.GetCompany(companyId, trackChanges: false);
        if(company == null)
        {
            _loggerManager.LogInfo($"Company with id: {companyId} not found in database.");
            return NotFound();
        }

        var employeeEntity = _repositoryManager.Employee.GetEmployee(companyId, id, trackChanges: true);
        if(employeeEntity == null)
        {
            _loggerManager.LogInfo($"Employee with id: {id} not found in database.");
            return NotFound();
        }

        var employee2Patch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);
        
        patchDoc.ApplyTo(employee2Patch);

        _mapper.Map(employee2Patch, employeeEntity);
        _repositoryManager.Save();

        return NoContent();
    }
}
