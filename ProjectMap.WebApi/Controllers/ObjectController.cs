using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProjectMap.WebApi;

namespace ProjectMap.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ObjectController : ControllerBase
{

    private readonly IObjectRepository _object;

    private readonly ILogger<ObjectController> _logger;

    public ObjectController(IObjectRepository repository, ILogger<ObjectController> logger)
    {
        _object = repository;
        _logger = logger;
    }

    [HttpGet(Name = "GetObject")]
    public IEnumerable<Object> Get()
    {
        return _object.GetAll();
    }

    [HttpPost(Name = "AddObject")]
    public Object Post(Object @object)
    {
        _object.Add(@object);
        return @object;
    }

    [HttpPut("{name}", Name = "UpdateObject")]
    public IActionResult Put(int prefabId, Object updatedObject)
    {
        var @object = _object.GetByPrefabId(prefabId);
        if (@object == null)
        {
            return NotFound(new { message = "Object not found" });
        }

        // Update the name
        _object.Update(prefabId, updatedObject);
        return Ok(updatedObject);
    }

    [HttpDelete("{name}", Name = "DeleteObject")]
    public IActionResult Delete(int prefabId)
    {
        var @object = _object.GetByPrefabId(prefabId);
        if (@object == null)
        {
            return NotFound(new { message = "Object not found" });
        }

        _object.Delete(prefabId);
        return NoContent();
    }
}