using dotnet__rpg.Data;
using dotnet__rpg.Dtos.User;
using dotnet__rpg.Services.CharacterService;
using Microsoft.AspNetCore.Mvc;

namespace dotnet__rpg.Controllers
{
    [ApiController]
    [Route("controller")]
    public class Authcontroller : ControllerBase
    {      
        private readonly IAuthRepository _authRepo;
        public Authcontroller(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register (UserRegisterDto request)
        {
            var response = await _authRepo.Register(
                new User {UserName = request.UserName},request.Passwrd
            );

            if(!response.Succes){
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login (UserLoginDto request)
        {
            var response = await _authRepo.Login(
                request.UserName,request.Password
            );

            if(!response.Succes){
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}