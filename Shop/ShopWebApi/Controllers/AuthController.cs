using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopDbLibrary.DTOs;
using ShopDbLibrary.Services;

namespace ShopWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(AuthService service) : ControllerBase
    {
        private readonly AuthService _authService = service;

        [HttpPost("login")]
        public async Task<ActionResult<string>> Post(LoginReqest reqest)
        {
            //авторизация и получением данных
            var token = await _authService.AuthUserAsync(reqest);

            if (token is not null)
            {
                return Ok(token); //возврат токена, если всё хорошо
            }
            return BadRequest(); //ошибка запроса
        }
    }
}
