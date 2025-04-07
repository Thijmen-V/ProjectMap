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
