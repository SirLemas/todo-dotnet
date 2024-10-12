using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
namespace TodoApi.Models;

public class TodoItem 
{
    public long Id {get; set;}

    public string? Title {get; set;}

    public string? Description {get; set;}

    public bool isDone {get; set;} = false;


    public long UserId { get; set; }
    public User? User { get; set; } 
}