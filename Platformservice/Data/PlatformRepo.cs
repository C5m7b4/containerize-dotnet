using System.Collections.Generic;
using System.Linq;
using Platformservice.Models;

namespace Platformservice.Data
{
  public class PlatformRepo : IPlatformRepo
  {
    private readonly AppDbContext _context;

    public PlatformRepo(AppDbContext context)
    {
      _context = context;
    }
    public void CreatePlatform(Platform plat)
    {
      if (plat == null)
      {
        throw new ArgumentNullException(nameof(plat));
      }

      _context.Platforms.Add(plat);
    }

    public IEnumerable<Platform> GetAllPlatforms()
    {
      return _context.Platforms.ToList();
    }

    public Platform GetPlatformById(int id)
    {
      var platform = _context.Platforms.FirstOrDefault(p => p.Id == id);
      if (platform != null)
      {
        return platform;
      }
      else
      {
        return new Platform { };
      }
    }

    public bool SaveChanges()
    {
      return (_context.SaveChanges() >= 0);
    }
  }
}