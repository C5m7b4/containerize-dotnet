using Platformservice.Dtos;

namespace Platformservice.SyncDataServices.Http
{
  public interface ICommandDataClient
  {
    Task SendPlatformToCommand(PlatformReadDto plat);
  }
}