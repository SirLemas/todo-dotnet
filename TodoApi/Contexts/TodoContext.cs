using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Contexts;

public class TodoContext : DbContext
{
    public TodoContext (DbContextOptions<TodoContext> options) : base(options) {}
    public DbSet<TodoItem> TodoItems {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoItem>()
            .HasOne(t => t.User)
            .WithMany() 
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}