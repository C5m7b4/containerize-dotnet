using AutoMapper;
using Platformservice.Dtos;
using Platformservice.Models;

namespace Platformservice.Profiles
{
  public class PlatformsProfile : Profile
  {
    public PlatformsProfile()
    {
      // source -> Target
      CreateMap<Platform, PlatformReadDto>();
      CreateMap<PlatformCreateDto, Platform>();
      CreateMap<PlatformReadDto, PlatformPublishedDto>();
    }
  }
}