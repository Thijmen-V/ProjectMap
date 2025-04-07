//using Microsoft.Data.SqlClient;
//using Dapper;
//using ProjectMap.WebApi;

//namespace ProjectMap.WebApi
//{
//    public class ObjectRepository : IObjectRepository
//    {
//        private string _sqlConnectionString;
//        public ObjectRepository(string sqlConnectionString)
//        {
//            _sqlConnectionString = sqlConnectionString;
//        }

//        private readonly List<Object> _objects = new();

//        public IEnumerable<Object> GetAll()
//        {
//            return _objects;
//        }

//        public Object? GetByPrefabId(int prefabId)
//        {
//            return _objects.FirstOrDefault(e => e.PrefabId == prefabId);
//        }

//        public void Add(Object @object)
//        {
//            _objects.Add(@object);
//        }

//        public void Update(int prefabId, Object updatedObject)
//        {
//            var @object = GetByPrefabId(prefabId);
//            if (@object != null)
//            {
//                @object.Id = updatedObject.Id;
//                @object.EnvironmentId = updatedObject.EnvironmentId;
//                @object.PrefabId = updatedObject.PrefabId;
//                @object.PosX = updatedObject.PosX;
//                @object.PosY = updatedObject.PosY;
//                @object.ScaleX = updatedObject.ScaleX;
//                @object.ScaleY = updatedObject.ScaleY;
//                @object.RotationZ = updatedObject.RotationZ;
//                @object.SortingLayer = updatedObject.SortingLayer;
//            }
//        }

//        //public void DeleteAsync(Guid objectId, int prefabId)
//        //{
//        //    var @object = GetByPrefabId(prefabId);
//        //    if (@object != null)
//        //    {
//        //        _objects.Remove(@object);
//        //    }
//        //}

//        public async Task DeleteAsync(Guid id)
//        {
//            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
//            {
//                var query = "DELETE FROM [Object2D] WHERE Id = @Id";
//                await sqlConnection.ExecuteAsync(query, new { id });
//            }
//        }
//    }
//}