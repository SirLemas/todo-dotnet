namespace TodoTestProject;
using TodoApi.Models;
using TodoApi.Services;

public class TodoItemTest
{
    [Fact]
    public void Title_Should_Not_Be_Empty()
    {
        var todoItem = new TodoItem {Title = "", Description = "Teste de titulo inválido", isDone = false};

        TodoItemValidationService _validate = new TodoItemValidationService();
        var errors = _validate.ValidateTodoItem(todoItem);
        Assert.Contains("O campo Titulo e obrigatorio.", errors);
    } 

    [Fact]
    public void Title_Should_Have_More_Than_Five_Characters()
    {
        var todoItem = new TodoItem {Title = "1234", Description = "Teste de titulo com menos de 5 caracteres", isDone = false};
        TodoItemValidationService _validate = new TodoItemValidationService();
        var errors = _validate.ValidateTodoItem(todoItem);

        Assert.Contains("O campo Titulo deve ter pelo menos 5 caracteres.", errors);
    }

    [Fact]
    public void Todoitem_Created_Successfully_When_Title_Is_Filled_Correctly()
    {
        var todoItem = new TodoItem {Title = "Titulo Preenchido", Description = "Teste de titulo inválido", isDone = false};
        TodoItemValidationService _validate = new TodoItemValidationService();
        var errors = _validate.ValidateTodoItem(todoItem);

        Assert.True(!errors.Any());
    }

    [Fact]
    public void Description_Should_Not_Be_Empty()
    {
        var todoItem = new TodoItem {Title = "123423", Description = "", isDone = false};
        TodoItemValidationService _validate = new TodoItemValidationService();
        var errors = _validate.ValidateTodoItem(todoItem);

        Assert.Contains("O campo Descricao e obrigatorio", errors);
    }

    [Fact]
    public void Description_Should_Have_More_Than_Five_Characters()
    {
        var todoItem = new TodoItem {Title = "123423", Description = "Tes", isDone = false};
        TodoItemValidationService _validate = new TodoItemValidationService();
        var errors = _validate.ValidateTodoItem(todoItem);

        Assert.Contains("O campo Descricao deve ter pelo menos 5 caracteres.", errors);
    }

    [Fact]
    public void Todoitem_Created_Successfully_When_Description_Is_Filled_Correctly()
    {
        var todoItem = new TodoItem {Title = "Vamos fazer acontecer", Description = "Teste de titulo inválido", isDone = false};
        TodoItemValidationService _validate = new TodoItemValidationService();
        var errors = _validate.ValidateTodoItem(todoItem);

        Assert.True(!errors.Any());
    }
        
}