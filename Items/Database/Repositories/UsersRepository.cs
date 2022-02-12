using Dapper;
using Items.Database.Repositories.IRepositories;
using Items.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Items.Database.Repositories
{
    public class UsersRepository:IUsersRepository
    {
        private readonly DbContext _dbContext;
        private readonly IDbConnection DapperConnection;

        public UsersRepository(DbContext dbContext)
        {
            _dbContext = dbContext;

            DapperConnection = _dbContext.GetDbConnection();
        }

        public async Task<User> GetUser(string login, string password)
        {
            var query = @$"SELECT * FROM [Users] 
                            where Login = '{login}'";

            var user = (await DapperConnection.QueryAsync<User>(query)).FirstOrDefault();
            DapperConnection.Close();

            bool isPasswordEqual = BCrypt.Net.BCrypt.Verify(password, user.Password);

            if (!isPasswordEqual)
                return null;

            return user;
        }
    }
}
