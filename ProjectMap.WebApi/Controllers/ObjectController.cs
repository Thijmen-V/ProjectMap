using Microsoft.AspNetCore.Mvc;
using ProjectMap.WebApi.Models;
using ProjectMap.WebApi.Repositories;

namespace ProjectMap.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ObjectController : ControllerBase
{
    private readonly IObjectRepository _repository;
    private readonly ILogger<ObjectController> _logger;

    public ObjectController(IObjectRepository repository, ILogger<ObjectController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    //[HttpGet(Name = "GetObjects")]
    //public async Task<IEnumerable<Object2D>>Get([FromBody] string EnvironmentId)
    //{

    //    return await _repository.GetByEnvironmentId(EnvironmentId);

    //}


    [HttpGet(Name = "GetObjects")]
    public async Task<IEnumerable<Object2D>> Get([FromQuery] string EnvironmentId)
    {

        return await _repository.GetByEnvironmentId(EnvironmentId);

    }

    [HttpGet("{Id}", Name = "GetObjectById")]
    public async Task<ActionResult<Object2D>> GetById(Guid Id)
    {
        var object2D = await _repository.GetById(Id);
        if (object2D == null)
        {
            return NotFound(new { message = "Object not found" });
        }
        return Ok(object2D);
    }

    [HttpPost(Name = "AddObject")]
    public async Task<ActionResult<Object2D>> Post([FromBody] Object2D object2D)
    {
        object2D.Id = Guid.NewGuid();
        await _repository.Add(object2D);
        return object2D;
    }

    [HttpPut("{Id}", Name = "UpdateObject")]
    public async Task<IActionResult> Put(Guid Id, [FromBody] Object2D updatedObject)
    {
        var existingObject = await _repository.GetById(Id);
        if (existingObject == null)
        {
            return NotFound(new { message = "Object not found" });
        }

        await _repository.UpdateAsync(Id, updatedObject);
        return Ok(updatedObject);
    }

    [HttpDelete("{Id}", Name = "DeleteObject")]
    public async Task<IActionResult> Delete(Guid Id)
    {
        var object2D = await _repository.GetById(Id);
        if (object2D == null)
        {
            return NotFound(new { message = "Object not found" });
        }

        await _repository.DeleteAsync(Id);
        return NoContent();
    }
}

//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Http.HttpResults;
//using Microsoft.AspNetCore.Mvc;
//using ProjectMap.WebApi;
//using ProjectMap.WebApi.Services;

//namespace ProjectMap.WebApi.Controllers;

//[ApiController]
//[Route("[controller]")]
//public class ObjectController : ControllerBase
//{

//    private readonly IEnvironmentRepository _environmentRepository;
//    private readonly AspNetIdentityAuthenticationService _authenticationService;
//    private readonly ObjectRepository _objectRepository;
//    private readonly ILogger<ObjectController> _objectLogger;

//    public ObjectController(IEnvironmentRepository repository, ILogger<ObjectController> objectLogger, AspNetIdentityAuthenticationService authenticationService, ObjectRepository objectRepository)
//    {
//        _environmentRepository = repository;
//        _objectLogger = objectLogger;
//        _authenticationService = authenticationService;
//        _objectRepository = objectRepository;
//    }

//    [HttpGet(Name = "GetObject")]
//    public IEnumerable<Object> Get()
//    {
//        return _objectRepository.GetAll();
//    }

//    [HttpPost(Name = "AddObject")]
//    public Object Post(Object @object)
//    {
//        _objectRepository.Add(@object);
//        return @object;
//    }

//    [HttpPut("{name}", Name = "UpdateObject")]
//    public IActionResult Put(int prefabId, Object updatedObject)
//    {
//        var @object = _objectRepository.GetByPrefabId(prefabId);
//        if (@object == null)
//        {
//            return NotFound(new { message = "Object not found" });
//        }

//        // Update the name
//        _objectRepository.Update(prefabId, updatedObject);
//        return Ok(updatedObject);
//    }



//    [HttpDelete("{environmentId}/[controller]/{objectId}", Name = "DeleteObject")]
//    public async Task<ActionResult> Delete(Guid environmentId, Guid objectId)
//    {
//        var existingObject = await _objectRepository.ReadAsync(objectId);
//        var existingEnvironment = await _environmentRepository.ReadAsync(environmentId);
//        var currentUserId = _authenticationService.GetCurrentAuthenticatedUserId();

//        if (existingObject == null || existingEnvironment.OwnerUserId != currentUserId)
//        {
//            return NotFound(new ProblemDetails { Detail = $"Environment {environmentId} not found" });
//        }
//        if (existingObject != null || existingObject.EnvironmentId != environmentId)
//        {
//            return NotFound(new ProblemDetails { Detail = $"Object {objectId} not found in environment {environmentId}" });
//        }
//        if (existingObject.EnvironmentId != environmentId)
//        {
//            return Conflict(new ProblemDetails { Detail = "The id of the environment dit not match the id of the existing environment" });
//        }

//        await _objectRepository.DeleteAsync(objectId);

//        return Ok();

//    }


//    //[HttpDelete("{name}", Name = "DeleteObject")]
//    //public IActionResult Delete(int prefabId)
//    //{
//    //    var @object = _object.GetByPrefabId(prefabId);
//    //    if (@object == null)
//    //    {
//    //        return NotFound(new { message = "Object not found" });
//    //    }

//    //    _object.Delete(prefabId);
//    //    return NoContent();
//    //}
//}