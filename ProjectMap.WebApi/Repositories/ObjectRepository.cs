using Microsoft.Data.SqlClient;
using Dapper;
using ProjectMap.WebApi.Models;
using ProjectMap.WebApi.Repositories;

namespace ProjectMap.WebApi.Repositories
{
    public class ObjectRepository : IObjectRepository
    {
        private string _sqlConnectionString;
        public ObjectRepository(string sqlConnectionString)
        {
            _sqlConnectionString = sqlConnectionString;
        }

        public async Task<IEnumerable<Object2D>> GetAll()
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Object2D>("SELECT * FROM [Object2D]");
            }
        }

        public async Task<IEnumerable<Object2D>> GetByEnvironmentId(string EnvironmentId)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                var query = "SELECT * FROM [Object2D] WHERE EnvironmentId = @Id";
                return await sqlConnection.QueryAsync<Object2D>(query, new { Id = EnvironmentId });
            }
        }


        public async Task<Object2D?> GetById(Guid Id)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                var query = "SELECT * FROM [Object2D] WHERE Id = @Id";
                return await sqlConnection.QuerySingleOrDefaultAsync<Object2D>(query, new { Id });
            }
        }

        public async Task Add(Object2D object2D)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                var query = "INSERT INTO [Object2D](Id, EnvironmentId, PrefabId, PositionX, PositionY, ScaleX, ScaleY, RotationZ, SortingLayer) " +
                            "VALUES (@Id, @EnvironmentId, @PrefabId, @PositionX, @PositionY, @ScaleX, @ScaleY, @RotationZ, @SortingLayer)";
                await sqlConnection.ExecuteAsync(query, object2D);
            }
        }


        public async Task UpdateAsync(Guid Id, Object2D updatedObject)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                var query = @"UPDATE [Object2D] 
                              SET PrefabId = @PrefabId, EnvironmentId = @EnvironmentId, 
                                  PositionX = @PositionX, PositionY = @PositionY, 
                                  ScaleX = @ScaleX, ScaleY = @ScaleY, RotationZ = @RotationZ, 
                                  SortingLayer = @SortingLayer
                              WHERE Id = @Id";

                await sqlConnection.ExecuteAsync(query, new
                {
                    Id,
                    updatedObject.PrefabId,
                    updatedObject.EnvironmentId,
                    updatedObject.PositionX,
                    updatedObject.PositionY,
                    updatedObject.ScaleX,
                    updatedObject.ScaleY,
                    updatedObject.RotationZ,
                    updatedObject.SortingLayer
                });
            }
        }

        public async Task DeleteAsync(Guid Id)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                var query = "DELETE FROM [Object2D] WHERE Id = @Id";
                await sqlConnection.ExecuteAsync(query, new { Id });
            }
        }
    }
}

//public async Task<Environment> CreateEnvironment(string name)
//{
//    var sql = "INSERT INTO Environment2D (Name, MaxHeight, MaxLength) VALUES (@Name, @MaxHeight, @MaxLength)";
//    return await sqlConnection.QuerySingleOrDefaultAsync<Environment>(sql, new { Name = name, MaxHeight = 15, MaxLength = 15 });
//}

//public async Task<Environment?> ReadAsync(int id)
//{
//    using (var sqlConnection = new SqlConnection(_sqlConnectionString))
//    {
//        return await sqlConnection.QuerySingleOrDefaultAsync<Environment>("SELECT * FROM [Environment2D] WHERE Id = @Id", new { Id = id });
//    }
//}