using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Platformservice.Data;
using Platformservice.Dtos;
using Platformservice.Models;

namespace Platformservice.Controllers
{
  [ApiController]
  [Route("api/platforms")]
  public class PlatformsController : ControllerBase
  {
    private readonly IPlatformRepo _repository;
    private readonly IMapper _mapper;

    public PlatformsController(IPlatformRepo repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
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
    public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto platform)
    {
      var platformModel = _mapper.Map<Platform>(platform);
      _repository.CreatePlatform(platformModel);
      _repository.SaveChanges();

      var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

      return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto);
    }

  }
}