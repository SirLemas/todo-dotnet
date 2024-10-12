using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;
using Microsoft.EntityFrameworkCore;
using TodoApi.Contexts;

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

    [HttpPost("login")]
    public async Task<ActionResult<TokenService>> Login(User user)
    {
        //procura usuário no banco de dados
        bool userExist = await getUser(user);

        if (!userExist) 
        {
            return new JsonResult(new {message = "usuario nao encontrado no sistema!"}) {StatusCode = StatusCodes.Status400BadRequest};
        }

        return new JsonResult(_tokenService.GenerateToken(user)) { StatusCode = StatusCodes.Status200OK };
    }

    private async Task<bool> getUser(User user)
    {
        //procura usuário no banco de dados e retorna true ou false;
        //usar email e password
        var userFound = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);

        if (userFound != null) {
            return true;
        } else {
            return false;
        }
    }
}