using System.Threading.Tasks;
using datingApp.API.Data;
using datingApp.API.Dtos;
using datingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace datingApp.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _repo;
        public AuthController(IAuthRepository repo){
            this._repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto){
           userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if(await this._repo.UserExists(userForRegisterDto.Username)){
                ModelState.AddModelError("Username", "Username already exists");
            }
           
            // validate request
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var userToCreate = new User{
                Username = userForRegisterDto.Username
            };

            var createUser = await this._repo.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201); 
        }
    }
}