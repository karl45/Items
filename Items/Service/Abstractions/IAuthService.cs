using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Items.Service.Abstractions
{
    public interface IAuthService
    {
       Task<ClaimsIdentity> GetUser(string login, string password);
    }
}
