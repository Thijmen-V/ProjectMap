using ProjectMap.WebApi;

namespace ProjectMap.WebApi
{
    public interface IEnvironmentRepository
    {
        IEnumerable<EnvironmentEntity> GetAll();
        EnvironmentEntity? GetByName(string name);
        void Add(EnvironmentEntity environment);
        void Update(string name, EnvironmentEntity updatedEnvironment);
        void Delete(string name);
    }
}
