//using Microsoft.Data.SqlClient;
//using Dapper;
//using ProjectMap.WebApi;
//using Microsoft.AspNetCore.Http.HttpResults;

//namespace ProjectMap.WebApi
//{
//    public class EnvironmentRepository : IEnvironmentRepository
//    {
//        private string _sqlConnectionString;
//        public EnvironmentRepository(string sqlConnectionString)
//        {
//            _sqlConnectionString = sqlConnectionString;
//        }

//        private readonly List<EnvironmentEntity> _environments = new();

//        public IEnumerable<EnvironmentEntity> GetAll()
//        {
//            return _environments;
//        }

//        public EnvironmentEntity? GetByName(string name)
//        {
//            return _environments.FirstOrDefault(e => e.Name == name);
//        }

//        public void Insert(EnvironmentEntity environment)
//        {
//            _environments.Add(environment);
//            return;
//        }

//        public void Update(string name, EnvironmentEntity updatedEnvironment)
//        {
//            var environment = GetByName(name);
//            if (environment != null)
//            {
//                environment.Id = updatedEnvironment.Id;
//                environment.Name = updatedEnvironment.Name;
//                environment.MaxLength = updatedEnvironment.MaxLength;
//                environment.MaxHeight = updatedEnvironment.MaxHeight;
//            }
//        }

//        public async Task DeleteAsync(Guid id)
//        {
//            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
//            {
//                await sqlConnection.ExecuteAsync("DELETE FROM [EnvironmentEntity] WHERE Id = @Id", new { id });
//            }
//        }

//        //public void Delete(string name)
//        //{
//        //    var environment = GetByName(name);
//        //    if (environment != null)
//        //    {
//        //        _environments.Remove(environment);
//        //    }
//        //}

//        public async Task<IEnumerable<EnvironmentEntity?>> ReadByOwnerUserId(string ownerUserId)
//        {
//            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
//            {
//                return await sqlConnection.QueryAsync<EnvironmentEntity>("SELECT * FROM [Environment2D] WHERE Id = @Id", new { Id = ownerUserId });
//            }
//        }
//    }
//}