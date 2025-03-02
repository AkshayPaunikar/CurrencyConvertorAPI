using CurrencyConvertorAPI.DTOS;
using CurrencyConvertorAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConvertorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto login)
        {
            if (_userService.ValidateUser(login.Username, login.Password, out var user))
            {
                var token = _tokenService.GenerateToken(user);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }
    }
}
