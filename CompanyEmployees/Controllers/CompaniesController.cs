using Contracts.Interfaces;
using Entities.DTO;
using Entities.Models;
using CompanyEmployees.Mapper;

using Microsoft.AspNetCore.Mvc;
using AutoMapper;
namespace CompanyEmployees.Controllers;

//These controller used for handling companies requests
[ApiController]
[Route("api/companies")]
public class CompaniesController : ControllerBase
{
	private readonly IRepositoryManager _repositoryManager;
	private readonly ILoggerManager _loggerManager;
	private readonly IMapper _mapper;

	public CompaniesController(IRepositoryManager repositoryManager,
							   ILoggerManager loggerManager,
							   IMapper mapper)
	{
		_repositoryManager = repositoryManager;
		_loggerManager = loggerManager;
		_mapper = mapper;
	}

	[HttpGet]
	public IActionResult GetCompanies()
	{
		var companies = _repositoryManager.Company.GetAllCompanies(false);

		var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

		return Ok(companiesDto);
	}

	[HttpGet("{id}", Name = "CompanyById" )]
	public IActionResult GetCompany(Guid id)
	{
		var company = _repositoryManager.Company.GetCompany(id, false);

		if (company == null)
		{
			_loggerManager.LogInfo($"Company with id: {id} does not exists in the database");
			return NotFound();
		}

		var companyDto = _mapper.Map<CompanyDto>(company);

		return Ok(companyDto);
	}

	[HttpGet("collection/({ids})", Name = "CompanyCollection")]
	public IActionResult GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]
												IEnumerable<Guid> ids)
	{
		if(ids == null)
		{
			_loggerManager.LogError("Parameter ids is null");
			return BadRequest("Parameter ids is null");
		}

		var companyEntities = _repositoryManager.Company.GetByIds(ids, trackChanges: false);

		if(ids.Count() != companyEntities.Count())
		{
			_loggerManager.LogError("Some ids are not valid in a collection");
			return NotFound();
		}

		var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
		return Ok(companiesToReturn);
	}


	[HttpPost]
	public IActionResult CreateCompany([FromBody]CompanyForCreationDto company)
	{
		if (company is null)
		{
			_loggerManager.LogError("CompanyForCreationDto object sent from client is null");
			return BadRequest("CompanyForCreationDto object is null");
		}

		var companyEntity = _mapper.Map<Company>(company);

		_repositoryManager.Company.CreateCompany(companyEntity);
		_repositoryManager.Save();

		var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);

		return CreatedAtRoute("CompanyById", new { id = companyToReturn.Id}, companyToReturn);
	}

	[HttpPost("collection")]
	public IActionResult createCompanyCollection([FromBody]
						 IEnumerable<CompanyForCreationDto> companyCollection)
	{
		if(companyCollection == null)
		{
			_loggerManager.LogError("Company collection sent from client is null");
			return BadRequest("Company collection is null");
		}

		var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
		foreach(var company in companyEntities)
		{
			_repositoryManager.Company.CreateCompany(company);
		}

		_repositoryManager.Save();

		var companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
		var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));

		return CreatedAtRoute("CompanyCollection", new {ids}, companyCollectionToReturn);
	}

	[HttpDelete("{id}")]
	public IActionResult DeleteCompany(Guid id)
	{
		var company = _repositoryManager.Company.GetCompany(id, trackChanges: false);
		if(company == null)
		{
			_loggerManager.LogInfo($"Company with id: {id} doesn't exist in the database.");			
			return NotFound();
		}

		_repositoryManager.Company.DeleteCompany(company);
		_repositoryManager.Save();

		return NoContent();
	}

    [HttpPut("{id}")]
    public IActionResult UpdateCompany(Guid id, [FromBody]CompanyForUpdateDto company)
    {
        if(company == null)
        {
            _loggerManager.LogError("CompanyForUpdateDto object sent from client is null.");
            return BadRequest("CompanyForUpdateDto object is null.");
        }

        var companyEntity = _repositoryManager.Company.GetCompany(id, trackChanges: true);
        if(companyEntity == null)
        {
            _loggerManager.LogInfo("Company with id: {id} doesn't exist in the database.");
            return NotFound();
        }

        _mapper.Map(company, companyEntity);
        _repositoryManager.Save();

        return NoContent();
    }
}
