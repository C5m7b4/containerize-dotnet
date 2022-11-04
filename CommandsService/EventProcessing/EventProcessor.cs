using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;

namespace CommandsService.EventProcessing
{
  public class EventProcessor : IEventProcessor
  {
    private readonly IMapper _mapper;
    private readonly IServiceScopeFactory _scopeFactory;

    public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
    {
      _scopeFactory = scopeFactory;
      _mapper = mapper;
    }

    public void ProcessEvent(string message)
    {
      var eventType = DetermineEvent(message);

      switch (eventType)
      {
        case EventType.PlatformPublished:
          addPlatform(message);
          break;
        default:
          break;
      }
    }

    private EventType DetermineEvent(string notificationMessage)
    {
      Console.WriteLine("--> Determining Event Type");
      var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

      switch (eventType.Event)
      {
        case "Platform Published":
          Console.WriteLine("Platform Published Event Detected");
          return EventType.PlatformPublished;
        default:
          Console.WriteLine("Could not determine type");
          return EventType.Undetermined;
      }
    }

    private void addPlatform(string platformPublishedMessage)
    {
      using (var scope = _scopeFactory.CreateScope())
      {
        var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

        var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

        try
        {
          var plat = _mapper.Map<Models.Platform>(platformPublishedDto);
          if (!repo.ExternalPlatformExists(plat.ExternalId))
          {
            repo.CreatePlatform(plat);
            repo.SaveChanges();
            Console.WriteLine($"--> Platform Added:{plat.ExternalId}");
          }
          else
          {
            Console.WriteLine($"--> Platform already exists: {plat.ExternalId}");
          }
        }
        catch (System.Exception ex)
        {
          Console.WriteLine($"--> Could not add platform to db: {ex.Message}");
        }
      }
    }
  }

  enum EventType
  {
    PlatformPublished,
    Undetermined
  }
}