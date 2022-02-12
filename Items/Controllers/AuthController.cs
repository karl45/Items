using Items.Service;
using Items.Service.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Items.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        /// <summary>
        /// Логин это слово "user-" и в конце любое число от 0 до 20. Пароль это слово "password-" и в конце любое число от 0 до 20
        /// </summary>
        /// <param name="login">user-[0...20]</param>
        /// <param name="password">password-[0...20]</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Authorize(string login,string password)
        {
            var user = await _authService.GetUser(login, password);

            if (user != null)
            {
                var token = new JwtSecurityToken(
                    claims: user.Claims,
                    issuer: "ITEMISSUER",
                    audience: "ITEMAUDIENCE",
                    expires: DateTime.Now.AddMinutes(TimeSpan.FromMinutes(60).TotalMinutes),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ITEMSECRETWITH16SYMBOLS")), SecurityAlgorithms.HmacSha256)
                    );

                var hashToken = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new { token = hashToken });
            }
            return BadRequest(new { errorText = "Неверное имя пользователя и пароль" });
        }
        
    }
}
