using CommandsService.Models;

namespace CommandsService.Data
{
  public interface ICommandRepo
  {
    bool SaveChanges();

    #region Platforms
    IEnumerable<Platform> GetAllPlatforms();

    void CreatePlatform(Platform plat);

    bool PlatformExists(int platformId);
    #endregion

    #region Commands
    IEnumerable<Command> GetCommandsForPlatform(int platformid);

    Command GetCommand(int platformId, int commandId);

    void CreateCommand(int platformId, Command command);
    #endregion
  }
}