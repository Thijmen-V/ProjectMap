using ProjectMap.WebApi;

namespace ProjectMap.WebApi
{
    public interface IObjectRepository
    {
        IEnumerable<Object> GetAll();
        Object? GetByPrefabId(int prefabId);
        void Add(Object @object);
        void Update(int prefabId, Object updatedObject);
        void Delete(int prefabId);
    }
}
