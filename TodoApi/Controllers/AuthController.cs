using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;
using Microsoft.EntityFrameworkCore;
using TodoApi.Contexts;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace TodoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;
    private readonly UserContext _context;
    public AuthController(TokenService tokenService, UserContext context) 
    {
        _tokenService = tokenService;
        _context = context;
    }

    public class LoginRequest
    {
        public string Email {get;set;}
        public string Password {get;set;}
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenService>> Login([FromBody] LoginRequest loginRequest)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email && u.Password == loginRequest.Password);

        if (user == null) 
        {
            return new JsonResult(new {message = "usuario nao encontrado no sistema!"}) {StatusCode = StatusCodes.Status400BadRequest};
        }

        return new JsonResult(_tokenService.GenerateToken(user)) { StatusCode = StatusCodes.Status200OK };
    }
}