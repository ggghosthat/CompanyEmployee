using Microsoft.AspNetCore.Mvc;
using Contracts.Interfaces;

namespace CompanyEmployees.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IRepositoryManager _repository;

    public WeatherForecastController(IRepositoryManager repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<string>> Get()
    {

        return new string[] { "value1", "value2" };
    }
}
