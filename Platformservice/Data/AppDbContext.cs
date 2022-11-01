using Microsoft.EntityFrameworkCore;
using Platformservice.Models;

namespace Platformservice.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
    {

    }

    public DbSet<Platform> Platforms { get; set; }
  }
}