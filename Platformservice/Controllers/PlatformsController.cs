using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Platformservice.AsyncDataServies;
using Platformservice.Data;
using Platformservice.Dtos;
using Platformservice.Models;
using Platformservice.SyncDataServices.Http;

namespace Platformservice.Controllers
{
  [ApiController]
  [Route("api/platforms")]
  public class PlatformsController : ControllerBase
  {
    private readonly IPlatformRepo _repository;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;
    private readonly IMessageBusClient _messageBusClient;

    public PlatformsController(IPlatformRepo repository, IMapper mapper, ICommandDataClient commandDataClient, IMessageBusClient messageBusClient)
    {
      _repository = repository;
      _mapper = mapper;
      _commandDataClient = commandDataClient;
      _messageBusClient = messageBusClient;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
      Console.WriteLine("--> Getting Platforms...");
      var platformItems = _repository.GetAllPlatforms();

      return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
    }

    [HttpGet("{id}", Name = "GetPlatformById")]
    public ActionResult<PlatformReadDto> GetPlatformById(int id)
    {
      var platformItem = _repository.GetPlatformById(id);
      if (platformItem != null)
      {
        return Ok(_mapper.Map<PlatformReadDto>(platformItem));
      }
      else
      {
        return NotFound();
      }
    }

    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platform)
    {
      var platformModel = _mapper.Map<Platform>(platform);
      _repository.CreatePlatform(platformModel);
      _repository.SaveChanges();

      var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

      // send sync message
      try
      {
        await _commandDataClient.SendPlatformToCommand(platformReadDto);
      }
      catch (System.Exception ex)
      {
        Console.WriteLine($"--> Could  not send synchronusly, {ex.Message}");
      }

      // send async message
      try
      {
        var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformReadDto);
        platformPublishedDto.Event = "Platform Published";
        _messageBusClient.PublishNewPlatform(platformPublishedDto);
      }
      catch (Exception ex)
      {
        Console.WriteLine("async send error: " + ex.Message);
      }

      return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto);
    }

  }
}