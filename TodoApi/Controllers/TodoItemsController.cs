using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Contexts;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly TodoItemValidationService _validate;

        public TodoItemsController(TodoContext context, TodoItemValidationService validateService)
        {
            _context = context;
            _validate = validateService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            var todoItems = await _context.TodoItems.ToListAsync();
            return new JsonResult(todoItems);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return new JsonResult(new { message = "Nenhum item foi encontrado" }) { StatusCode = StatusCodes.Status404NotFound };
                // return NotFound("Nenhum item foi encontrado");
            }

            return new JsonResult(todoItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return new JsonResult(new { message = "ID passado não corresponde ao item a ser atualizado, tente novamente!" }) { StatusCode = StatusCodes.Status400BadRequest };
            }

            var errors = _validate.ValidateTodoItem(todoItem);
            if (errors.Any()) 
            {
                return new JsonResult(new {errors}) {StatusCode = StatusCodes.Status400BadRequest};
            }

            _context.TodoItems.Update(todoItem);
            await _context.SaveChangesAsync();
            return new JsonResult(new { message = "Item atualizado com sucesso!" });
        }

        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            var errors = _validate.ValidateTodoItem(todoItem);

            if (errors.Any()) 
            {
                return new JsonResult(new {errors}) {StatusCode = StatusCodes.Status400BadRequest};
            }

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return new JsonResult(new {message = "Item criado com sucesso!"}) { StatusCode = StatusCodes.Status201Created };
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
