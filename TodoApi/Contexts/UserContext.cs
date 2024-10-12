using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Contexts;

public class UserContext : DbContext
{
    public UserContext (DbContextOptions<UserContext> options) : base(options) {}
    public DbSet<User> Users { get; set; }
}