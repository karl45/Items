using Items.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Items.Database.Repositories.IRepositories
{
    public interface IUsersRepository
    {
        Task<User> GetUser(string login, string password);
    }
}
