using Contracts.Interfaces;
using Entities.Models;
using Entities.DTO;

using CompanyEmployees.ActionFilters;

using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
namespace CompanyEmployees.Controllers;

[ApiController]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase
{
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IAuthenticationManager _authManager;

    public AuthenticationController(IAuthenticationManager authManager,
                                    ILoggerManager logger,
                                    IMapper mapper,
                                    UserManager<User> userManager)
    {
        _authManager = authManager;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }


    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> RegisterUser([FromBody]UserForRegistrationDto userForRegistration)
    {
        var user = _mapper.Map<User>(userForRegistration);

        var result = await _userManager.CreateAsync(user, userForRegistration.Password);
        if(!result.Succeeded)
        {
            foreach(var err in result.Errors)
                ModelState.TryAddModelError(err.Code, err.Description);

            return BadRequest(ModelState); 
        }

        await _userManager.AddToRolesAsync(user, userForRegistration.Roles);

        return StatusCode(201);
    }

    [HttpPost("login")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Authenticate([FromBody] UserAuthenticationDto userAuthentication)
    {
        if(!await _authManager.ValidateUser(userAuthentication))
        {
            Console.WriteLine("Not authenticated");
            _logger.LogWarn($"{nameof(Authenticate)} : Authentication failed. Wrong name or password.");
            return Unauthorized();
        }
        
        return Ok(new {Token = await _authManager.CreateToken() });
    }
}
