using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Contexts;
using TodoApi.Models;

namespace TodoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserContext _context;

    public UserController(UserContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();
        return new JsonResult(users) {StatusCode = StatusCodes.Status200OK};
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(long id)
    {
         var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return new JsonResult(new { message = "Nenhum usuario foi encontrado" }) { StatusCode = StatusCodes.Status404NotFound };
            }

            return new JsonResult(user) {StatusCode = StatusCodes.Status200OK};
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        try
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new JsonResult(new {message = "Item criado com sucesso!"}) { StatusCode = StatusCodes.Status201Created };
        }
        catch (Exception ex)
        {
            return new JsonResult(new {message = $"Aconteceu algum erro inesperado\nErro: {ex.Message}"}) { StatusCode = StatusCodes.Status201Created };
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<User>> EditUser(long id, User user)
    {
        if (id != user.Id)
        {
            return new JsonResult(new { message = "ID passado não corresponde ao usuario a ser atualizado, tente novamente!" }) { StatusCode = StatusCodes.Status400BadRequest };
        }

        try
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return new JsonResult(new { message = "Item atualizado com sucesso!"}) {StatusCode = StatusCodes.Status200OK};
        }
        catch (Exception ex)
        {
            return new JsonResult(new { message = $"Aconteceu algum erro inesperado.\nErro: {ex.Message}"}) {StatusCode = StatusCodes.Status400BadRequest};
        }
        
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<User>> DeleteUser(long id)
    {
         var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return new JsonResult(new { message = "Item não encontrado" }) { StatusCode = StatusCodes.Status404NotFound };
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return new JsonResult(new { message = "Item deletado com sucesso!" }) { StatusCode = StatusCodes.Status200OK };
    }
}
