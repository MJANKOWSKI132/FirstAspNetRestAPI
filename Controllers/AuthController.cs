using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenHandler _tokenHandler;
    
    public AuthController(IUserRepository userRepository, ITokenHandler tokenHandler)
    {
        this._userRepository = userRepository;
        this._tokenHandler = tokenHandler;
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto loginRequest)
    {
        var user = await _userRepository.AuthenticateUserAsync(loginRequest.Username, loginRequest.Password);
        if (user == null)
        {
            return BadRequest("Username or Password is incorrect");
        }

        var token = await _tokenHandler.CreateTokenAsync(user);
        return Ok(token);
    }
}