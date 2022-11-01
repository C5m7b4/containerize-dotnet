using System.Collections.Generic;
using Platformservice.Models;

namespace Platformservice.Data
{
  public interface IPlatformRepo
  {
    bool SaveChanges();

    IEnumerable<Platform> GetAllPlatforms();

    Platform GetPlatformById(int id);

    void CreatePlatform(Platform plat);
  }
}