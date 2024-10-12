using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Contexts;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly UserContext _userContext;
        private readonly TodoItemValidationService _validate;

        public TodoItemsController(TodoContext context, UserContext userContext, TodoItemValidationService validateService)
        {
            _context = context;
            _validate = validateService;
            _userContext = userContext;
        }

        private async Task<User> GetLoggedUser()
        {
            string userEmail = User.Claims.First(c => c.Type == "Email").Value;
            return await _userContext.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            var todoItems = await _context.TodoItems.ToListAsync();
            return new JsonResult(todoItems) {StatusCode = StatusCodes.Status200OK};
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var user = GetLoggedUser();
            if (user == null) {
                return new JsonResult(new { message = "O usuário não foi encontrado"}) { StatusCode = StatusCodes.Status404NotFound };
            }

            var todoItem = await _context.TodoItems.FirstOrDefaultAsync(t => t.Id == id && t.UserId == user.Id);

            if (todoItem == null)
            {
                return new JsonResult(new { message = "Nenhum item foi encontrado"}) { StatusCode = StatusCodes.Status404NotFound };
            }

            return new JsonResult(todoItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return new JsonResult(new { message = "ID passado não corresponde ao item a ser atualizado, tente novamente!"}) { StatusCode = StatusCodes.Status400BadRequest };
            }

            var errors = _validate.ValidateTodoItem(todoItem);
            if (errors.Any()) 
            {
                return new JsonResult(new {errors}) {StatusCode = StatusCodes.Status400BadRequest};
            }

            try
            {
                var user = GetLoggedUser();
                if (user == null) {
                    throw new NullReferenceException("O usuário não foi encontrado para este item");
                }

                todoItem.UserId = user.Id;
                _context.TodoItems.Update(todoItem);
                await _context.SaveChangesAsync();
                return new JsonResult(new { message = "Item atualizado com sucesso!"}) {StatusCode = StatusCodes.Status200OK};
            }
            catch (Exception ex)
            {
                return new JsonResult(new {message = $"Aconteceu algum erro inesperado\nErro: {ex.Message}"}) {StatusCode = StatusCodes.Status400BadRequest};
            }
        }

        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            var errors = _validate.ValidateTodoItem(todoItem);

            if (errors.Any()) 
            {
                return new JsonResult(new {errors}) {StatusCode = StatusCodes.Status400BadRequest};
            }

            try
            {
                var user = GetLoggedUser();
                if (user == null) {
                    throw new NullReferenceException("O usuário não foi encontrado para este item");
                }

                todoItem.UserId = user.Id;
                _context.TodoItems.Add(todoItem);
                await _context.SaveChangesAsync();
                return new JsonResult(new {message = "Item criado com sucesso!"}) { StatusCode = StatusCodes.Status201Created};
            }
            catch (Exception ex)
            {
                return new JsonResult(new {message = $"Aconteceu algum erro inesperado\nErro: {ex.Message}"}) { StatusCode = StatusCodes.Status400BadRequest};
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return new JsonResult(new { message = "Item não encontrado" }) { StatusCode = StatusCodes.Status404NotFound };
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return new JsonResult(new { message = "Item deletado com sucesso!" }) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
