using ProjectMap.WebApi.Models;

namespace ProjectMap.WebApi.Repositories
{
    public interface IObjectRepository
    {
        Task<IEnumerable<Object2D>> GetAll();
        Task<IEnumerable<Object2D>> GetByEnvironmentId(string EnvironmentId);
        Task<Object2D?> GetById(Guid Id);
        Task Add(Object2D object2D);
        Task UpdateAsync(Guid Id, Object2D updatedObject);
        Task DeleteAsync(Guid Id);
    }
}