using Contracts.Interfaces;
using Entities.DTO;
using Entities.Models;
using Entities.RequestFeatures;

using CompanyEmployees.Mapper;
using CompanyEmployees.ActionFilters;

using System.Threading.Tasks;
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
	public async Task<IActionResult> GetCompanies()
	{
		var companies = await _repositoryManager.Company.GetAllCompaniesAsync(false);

		var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

		return Ok(companiesDto);
	}

	[HttpGet("dive")]
	public async Task<IActionResult> GetCompanies([FromQuery] CompanyParameters companyParameters)
	{		
		var companies = await _repositoryManager.Company.GetCompaniesAsync(companyParameters, false);

		var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

		return Ok(companiesDto);
	}

	[HttpGet("{id}", Name = "CompanyById" )]
	public async Task<IActionResult> GetCompany(Guid id)
	{
		var company = await _repositoryManager.Company.GetCompanyAsync(id, false);

		if (company == null)
		{
			_loggerManager.LogInfo($"Company with id: {id} does not exists in the database");
			return NotFound();
		}

		var companyDto = _mapper.Map<CompanyDto>(company);

		return Ok(companyDto);
	}

	[HttpGet("collection/({ids})", Name = "CompanyCollection")]
	public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]
												IEnumerable<Guid> ids)
	{
		if(ids == null)
		{
			_loggerManager.LogError("Parameter ids is null");
			return BadRequest("Parameter ids is null");
		}

		var companyEntities = await _repositoryManager.Company.GetByIdsAsync(ids, trackChanges: false);

		if(ids.Count() != companyEntities.Count())
		{
			_loggerManager.LogError("Some ids are not valid in a collection");
			return NotFound();
		}

		var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
		return Ok(companiesToReturn);
	}


	[HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
	public async Task<IActionResult> CreateCompany([FromBody]CompanyForCreationDto company)
	{
		var companyEntity = _mapper.Map<Company>(company);

		_repositoryManager.Company.CreateCompany(companyEntity);
		await _repositoryManager.SaveAsync();

		var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);

		return CreatedAtRoute("CompanyById", new { id = companyToReturn.Id}, companyToReturn);
	}

	[HttpPost("collection")]
	public async Task<IActionResult> CreateCompanyCollection([FromBody]
						 IEnumerable<CompanyForCreationDto> companyCollection)
	{
		if(companyCollection == null)
		{
			_loggerManager.LogError("Company collection sent from client is null");
			return BadRequest("Company collection is null");
		}

        if(!ModelState.IsValid)
        {
            _loggerManager.LogError("Invalid model data for CompanyForCreationDto object");
            return UnprocessableEntity(ModelState);
        }


		var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
		foreach(var company in companyEntities)
		{
			_repositoryManager.Company.CreateCompany(company);
		}

		await _repositoryManager.SaveAsync();

		var companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
		var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));

		return CreatedAtRoute("CompanyCollection", new {ids}, companyCollectionToReturn);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteCompany(Guid id)
	{
		var company = await _repositoryManager.Company.GetCompanyAsync(id, trackChanges: false);
		if(company == null)
		{
			_loggerManager.LogInfo($"Company with id: {id} doesn't exist in the database.");			
			return NotFound();
		}

		_repositoryManager.Company.DeleteCompany(company);
		await _repositoryManager.SaveAsync();

		return NoContent();
	}

    [HttpPut("{id}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateCompany(Guid id, [FromBody]CompanyForUpdateDto company)
    {
        var companyEntity = await _repositoryManager.Company.GetCompanyAsync(id, trackChanges: true);
        if(companyEntity == null)
        {
            _loggerManager.LogInfo("Company with id: {id} doesn't exist in the database.");
            return NotFound();
        }

        _mapper.Map(company, companyEntity);
        await _repositoryManager.SaveAsync();

        return NoContent();
    }
}
