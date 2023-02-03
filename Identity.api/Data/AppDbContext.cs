using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.api.Data;

public class AppDbContext : DbContext
{
    public DbSet<AppUser>? Users { get; set; }
    public AppDbContext(DbContextOptions options) : base(options) { }
}
