using Microsoft.Data.SqlClient;
using Dapper;
using ProjectMap.WebApi;

namespace ProjectMap.WebApi
{
    public class EnvironmentRepository : IEnvironmentRepository
    {
        private string _sqlConnectionString;
        public EnvironmentRepository(string sqlConnectionString)
        {
            _sqlConnectionString = sqlConnectionString;
        }

        private readonly List<EnvironmentEntity> _environments = new();

        public IEnumerable<EnvironmentEntity> GetAll()
        {
            return _environments;
        }

        public EnvironmentEntity? GetByName(string name)
        {
            return _environments.FirstOrDefault(e => e.Name == name);
        }

        public void Add(EnvironmentEntity environment)
        {
            _environments.Add(environment);
        }

        public void Update(string name, EnvironmentEntity updatedEnvironment)
        {
            var environment = GetByName(name);
            if (environment != null)
            {
                environment.Id = updatedEnvironment.Id;
                environment.Name = updatedEnvironment.Name;
                environment.MaxLength = updatedEnvironment.MaxLength;
                environment.MaxHeight = updatedEnvironment.MaxHeight;
            }
        }

        public void Delete(string name)
        {
            var environment = GetByName(name);
            if (environment != null)
            {
                _environments.Remove(environment);
            }
        }

        //public async Task<Environment?> ReadAsync(int id)
        //{
        //    using (var sqlConnection = new SqlConnection(_sqlConnectionString))
        //    {
        //        return await sqlConnection.QuerySingleOrDefaultAsync<Environment>("SELECT * FROM [Environment2D] WHERE Id = @Id", new { Id = id });
        //    }
        //}
    }
}