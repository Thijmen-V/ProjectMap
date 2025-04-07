using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProjectMap.WebApi;
using ProjectMap.WebApi.Models;
using ProjectMap.WebApi.Repositories;

namespace ProjectMap.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class EnvironmentController : ControllerBase
{

    private readonly IEnvironmentRepository _environmentRepository;
    private readonly IObjectRepository _objectRepository;
    private readonly IAuthenticationService _authenticationService;

    private readonly ILogger<EnvironmentController> _logger;

    public EnvironmentController(IEnvironmentRepository environmentRepository, IObjectRepository objectRepository, IAuthenticationService authenticationService, ILogger<EnvironmentController> logger)
    {
        _environmentRepository = environmentRepository;
        _objectRepository = objectRepository;
        _authenticationService = authenticationService;
        _logger = logger;
    }

    [HttpGet(Name = "GetEnvironment")]
    public async Task<IEnumerable<Environment2D>> Get()
    {
        var currentUserId = _authenticationService.GetCurrentAuthenticatedUserId();
        return await _environmentRepository.GetByUserId(currentUserId);
    }

    [HttpPost(Name = "AddEnvironment")]
    public Environment2D Post([FromBody] Environment2D environment)
    {
        var currentUserId = _authenticationService.GetCurrentAuthenticatedUserId();

        environment.id = Guid.NewGuid();
        environment.OwnerUserId = Guid.Parse(currentUserId);
        _environmentRepository.Add(environment);
        return environment;
    }

    [HttpPut("{name}", Name = "UpdateEnvironment")]
    public async Task<IActionResult> Put(string name, [FromBody] Environment2D updatedEnvironment)
    {
        var environment = await _environmentRepository.GetByName(name);
        if (environment == null)
        {
            return NotFound(new { message = "Environment not found" });
        }
        await _environmentRepository.UpdateAsync(name, updatedEnvironment);
        return Ok(updatedEnvironment);
    }


    [HttpDelete("{name}", Name = "DeleteEnvironment")]
    public async Task<IActionResult> Delete(string name)
    {
        var environment = await _environmentRepository.GetByName(name);
        if (environment == null)
        {
            return NotFound(new { message = "Environment not found" });
        }
        await _environmentRepository.DeleteAsync(name);
        return NoContent();
    }
}


