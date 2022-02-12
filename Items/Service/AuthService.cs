using Items.Database.Repositories.IRepositories;
using Items.Service.Abstractions;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Items.Service
{
    public class AuthService:IAuthService
    {
        private readonly IUsersRepository _usersRepository;

        public AuthService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<ClaimsIdentity> GetUser(string login,string password)
        {
            var user = await _usersRepository.GetUser(login, password);

            if (user != null)
            {
                var authClaims = new List<Claim>()
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType,user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType,user.RoleId == 1 ? "Customer":"Administrator")
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(authClaims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }
    }
}
