using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

    public DbSet<Movie?> Movies { get; set; }
    public DbSet<User> Users { get; set; }

    protected void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
    
}