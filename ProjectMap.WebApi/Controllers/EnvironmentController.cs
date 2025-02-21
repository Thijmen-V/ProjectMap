using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProjectMap.WebApi;

namespace ProjectMap.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class EnvironmentController : ControllerBase
{

    private readonly IEnvironmentRepository _repository;

    private readonly ILogger<EnvironmentController> _logger;

    public EnvironmentController(IEnvironmentRepository repository, ILogger<EnvironmentController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet(Name = "GetEnvironment")]
    public IEnumerable<EnvironmentEntity> Get()
    {
        return _repository.GetAll();
    }

    [HttpPost(Name = "AddEnvironment")]
    public EnvironmentEntity Post(EnvironmentEntity environment)
    {
        _repository.Add(environment);
        return environment;
    }

    [HttpPut("{name}", Name = "UpdateEnvironment")]
    public IActionResult Put(string name, EnvironmentEntity updatedEnvironment)
    {
        var environment = _repository.GetByName(name);
        if (environment == null)
        {
            return NotFound(new { message = "Environment not found" });
        }

        // Update the name
        _repository.Update(name, updatedEnvironment);
        return Ok(updatedEnvironment);
    }

    [HttpDelete("{name}", Name = "DeleteEnvironment")]
    public IActionResult Delete(string name)
    {
        var environment = _repository.GetByName(name);
        if (environment == null)
        {
            return NotFound(new { message = "Environment not found" });
        }

        _repository.Delete(name);
        return NoContent();
    }
}