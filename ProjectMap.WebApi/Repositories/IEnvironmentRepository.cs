using ProjectMap.WebApi.Models;

namespace ProjectMap.WebApi.Repositories
{
    public interface IEnvironmentRepository
    {
        Task<IEnumerable<Environment2D>> GetAll();
        //Task<Environment?> GetByName(string name);
        Task<IEnumerable<Environment2D?>> GetByUserId(string OwnerUserId);
        Task<IEnumerable<Environment2D?>> GetByName(string name);

        //Task Add(Environment environment);
        Task Add(Environment2D environment);
        Task UpdateAsync(string name, Environment2D updatedEnvironment);
        Task DeleteAsync(string name);
    }
}