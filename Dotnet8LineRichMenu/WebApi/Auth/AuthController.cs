using Dotnet8LineRichMenu.Models;
using Dotnet8LineRichMenu.Services.UserService;
using Dotnet8LineRichMenu.WebApi.Auth.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet8LineRichMenu.WebApi.Auth;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;

    public AuthController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterUserModel model)
    {
        var apiResponse = new ApiResponse();
        if (await _userService.RegisterUser(model.Username, model.Email, model.Password))
        {
            apiResponse.Message = "User registered successfully";
            return Ok(apiResponse);
        }

        apiResponse.Message = "User already exists";
        apiResponse.IsSuccess = false;
        return BadRequest(apiResponse);
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginUserModel model)
    {
        var token = await _userService.Authenticate(model.Email, model.Password);

        var apiResponse = new ApiResponse();
        if (token == null)
        {
            apiResponse.IsSuccess = false;
            apiResponse.Message = "Invalid credentials or user is not verified";
            return Unauthorized(apiResponse);
        }

        apiResponse.Body = new { Token = token };
        apiResponse.Message = "Login Successfully";

        return Ok(apiResponse);
    }
}