using Microsoft.Data.SqlClient;
using Dapper;
using ProjectMap.WebApi.Models;
// using Statements

namespace ProjectMap.WebApi.Repositories
{
    public class EnvironmentRepository : IEnvironmentRepository
    {
        private string _sqlConnectionString;
        public EnvironmentRepository(string sqlConnectionString)
        {
            _sqlConnectionString = sqlConnectionString;
        }

        public async Task<IEnumerable<Environment2D>> GetAll()
        {
            return await ReadAsync();
        }

        public async Task<IEnumerable<Environment2D?>> GetByName(string name)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                var query = "SELECT * FROM [Environment2D] WHERE Name = @Name";
                return await sqlConnection.QueryAsync<Environment2D>(query, new { Name = name });
            }
        }

        public async Task<IEnumerable<Environment2D?>> GetByUserId(string OwnerUserId)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                var query = "SELECT * FROM [Environment2D] WHERE OwnerUserId = @Id";
                return await sqlConnection.QueryAsync<Environment2D>(query, new { Id = OwnerUserId });
            }
        }

        public async Task Add(Environment2D environment2D)
        {
            //_environments.Add(environment);
            await InsertAsync(environment2D.id, environment2D.Name, environment2D.OwnerUserId.ToString(), environment2D.MaxHeight, environment2D.MaxLength);
        }

        public async Task Update(string name, Environment2D updatedEnvironment)
        {
            var environment = await GetByName(name);
            if (environment != null)
            {
                await UpdateAsync(name, updatedEnvironment);
            }
        }

        public async Task Delete(string name)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                var query = "DELETE FROM [Environment2D] WHERE Name = @Name";
                await sqlConnection.ExecuteAsync(query, new { Name = name });
            }
        }

        public async Task<IEnumerable<Environment2D>> ReadAsync()
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Environment2D>("SELECT * FROM [Environment2D]");
            }
        }

        public async Task InsertAsync(Guid id, string Name, string OwnerUserId, float MaxHeight, float MaxLength)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                var query = "INSERT INTO [Environment2D](id, Name, OwnerUserId, MaxHeight, MaxLength) VALUES (@id, @Name, @OwnerUserId, @MaxHeight, @MaxLength)";

                await sqlConnection.ExecuteAsync(query, new { id, Name, OwnerUserId, MaxHeight, MaxLength });
            }
        }

        public async Task UpdateAsync(string name, Environment2D updatedEnvironment)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                var query = @"UPDATE [Environment2D] 
                              SET Name = @NewName, MaxHeight = @MaxHeight, MaxLength = @MaxLength
                              WHERE Name = @OldName";

                await sqlConnection.ExecuteAsync(query, new { NewName = updatedEnvironment.Name, updatedEnvironment.MaxHeight, updatedEnvironment.MaxLength, OldName = name });
            }
        }

        //public async Task DeleteAsync(string name)
        //{
        //    using (var sqlConnection = new SqlConnection(_sqlConnectionString))
        //    {
        //        var query = "DELETE FROM [Environment2D] WHERE Name = @Name";
        //        await sqlConnection.ExecuteAsync(query, new { Name = name });
        //    }
        //}

        //Nieuwe delete functie waarij alle objecten (childeren) van de environment (parent) ook worden verwijderd
        public async Task DeleteAsync(string name)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                // Begin een transactie om beide deletes in één veilige stap te doen
                await sqlConnection.OpenAsync();
                using (var transaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        // Stap 1: Verwijder alle gekoppelde objecten
                        var deleteObjectsQuery = @"
                    DELETE FROM [Object2D] 
                    WHERE EnvironmentId = (SELECT Id FROM [Environment2D] WHERE Name = @Name)";
                        await sqlConnection.ExecuteAsync(deleteObjectsQuery, new { Name = name }, transaction);

                        // Stap 2: Verwijder het environment zelf
                        var deleteEnvironmentQuery = "DELETE FROM [Environment2D] WHERE Name = @Name";
                        await sqlConnection.ExecuteAsync(deleteEnvironmentQuery, new { Name = name }, transaction);

                        // Commit de transactie
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Fout bij verwijderen environment + objecten", ex);
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
    }
}