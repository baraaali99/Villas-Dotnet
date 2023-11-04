using System.Diagnostics;
using System.Net;
using firstDotnetProject.Repository.iRepository;
using Microsoft.AspNetCore.Mvc;

namespace firstDotnetProject.Controllers;

    [Route("Api/UserAuth")]
    [ApiController]
public class UserController : Controller
{
    private readonly IUserRepository _userRepo;
    private readonly ApiResponse _response;
    public UserController(IUserRepository userRepo)
    {
        _userRepo = userRepo;
        this._response = new();
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequestDto model)
    {
        var loginResponse = await _userRepo.Login(model);
        if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>(){"Username or Password is incorrect"};
            return BadRequest(_response);
        }

        _response.StatusCode = HttpStatusCode.OK;
        _response.IsSuccess = true;
        _response.Result = loginResponse;
        
        return Ok(_response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterationRequestDto model)
    {
        _userRepo.IsUniqueUser(model.UserName);
        if (_userRepo.IsUniqueUser(model.UserName) == false)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessages.Add("User already exists!");
            
            return BadRequest(_response);
        }

        var user = await _userRepo.Register(model);

        if (user == null)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessages.Add("Error while registering!");
            return BadRequest(_response);
        }
        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }
}