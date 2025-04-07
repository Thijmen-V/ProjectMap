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





//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http.HttpResults;
//using Microsoft.AspNetCore.Mvc;
//using ProjectMap.WebApi;
//using ProjectMap.WebApi.Services;


//namespace ProjectMap.WebApi.Controllers;

//[ApiController]
//[Route("[controller]")]
//public class EnvironmentController : ControllerBase
//{

//    private readonly EnvironmentRepository _environmentRepository;
//    private readonly AspNetIdentityAuthenticationService _authenticationService;
//    private readonly IObjectRepository _objectRepository;
//    private readonly ILogger<EnvironmentController> _environmentLogger;

//    public EnvironmentController(EnvironmentRepository environmentRepository, ILogger<EnvironmentController> EnvironmenLogger, AspNetIdentityAuthenticationService authenticationService, IObjectRepository objectRepository)
//    {
//        _environmentRepository = environmentRepository;
//        _environmentLogger = EnvironmenLogger;
//        _authenticationService = authenticationService;
//        _objectRepository = objectRepository;
//    }

//    [HttpGet(Name = "GetEnvironment")]
//    public async Task<ActionResult<List<EnvironmentEntity>>> GetAsync()
//    {
//        var currentUserId = _authenticationService.GetCurrentAuthenticatedUserId();
//        var userEnvironments = await _environmentRepository.ReadByOwnerUserId(currentUserId!);

//        return Ok(userEnvironments);
//    }

//    //[HttpGet(Name = "GetEnvironment")]
//    //public IEnumerable<EnvironmentEntity> Get()
//    //{
//    //    return _repository.GetAll();
//    //}

//    [HttpPost(Name = "AddEnvironment")]

//    public async Task<ActionResult<EnvironmentEntity>> AddAsync(EnvironmentEntity environment)
//    {
//        var currentUser = _authenticationService.GetCurrentAuthenticatedUserId();
//        var userEnvironments = await _environmentRepository.ReadByOwnerUserId(currentUser!);

//        if(userEnvironments.Count() >= EnvironmentEntity.MAX_NUMBER_OF_USER_ENVIRONMENTS)
//        {
//            return BadRequest(new ProblemDetails { Detail = "Number of environments exceed" });
//        }
//        if (userEnvironments.Any(x => x!.Name == environment.Name))
//        {
//            return BadRequest(new ProblemDetails { Detail = $"An environment with the name {environment.Name} already exist" });
//        }

//        environment.Id = Guid.NewGuid();
//        environment.OwnerUserId = currentUser;

//        await _environmentRepository.InsertAsync(environment.Name);

//        return CreatedAtRoute("GetEnvironmentById", new { environmentId = environment.Id });
//    }




//    //[HttpPost(Name = "AddEnvironment")]
//    //public EnvironmentEntity Post(EnvironmentEntity environment)
//    //{
//    //    _repository.Add(environment);
//    //    return environment;
//    //}



//    [HttpPut("{name}", Name = "UpdateEnvironment")]
//    public IActionResult Put(string name, EnvironmentEntity updatedEnvironment)
//    {
//        var environment = _environmentRepository.GetByName(name);
//        if (environment == null)
//        {
//            return NotFound(new { message = "Environment not found" });
//        }

//        // Update the name
//        _environmentRepository.Update(name, updatedEnvironment);
//        return Ok(updatedEnvironment);
//    }

//    [HttpDelete("{name}", Name = "DeleteEnvironment")]
//    public IActionResult Delete(string name)
//    {
//        var environment = _environmentRepository.GetByName(name);
//        if (environment == null)
//        {
//            return NotFound(new { message = "Environment not found" });
//        }

//        _environmentRepository.DeleteAsync(name);
//        return NoContent();
//    }

//}