using TodoApi.Models;

namespace TodoApi.Services;

public class TodoItemValidationService
{
    public List<string> ValidateTodoItem(TodoItem todoItem)
    {
        List<string> errors = new List<string>();

        if (string.IsNullOrWhiteSpace(todoItem.Title)) 
        {
            errors.Add("O campo Titulo e obrigatorio.");
        }

        if (!string.IsNullOrWhiteSpace(todoItem.Title) && todoItem.Title.Length < 5) 
        {
            errors.Add("O campo Titulo deve ter pelo menos 5 caractere.");
        }

        if (string.IsNullOrWhiteSpace(todoItem.Description))
        {
            errors.Add("O campo Descricao e obrigatorio");
        }

        if (!string.IsNullOrWhiteSpace(todoItem.Description) && todoItem.Description.Length < 5) {
            errors.Add("O campo Descricao deve ter pelo menos 5 caracteres.");
        }

        return errors;
    }
}