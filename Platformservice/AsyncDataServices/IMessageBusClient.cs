using Platformservice.Dtos;

namespace Platformservice.AsyncDataServies
{
  public interface IMessageBusClient
  {
    void PublishNewPlatform(PlatformPublishedDto platformPublishedDto);
  }
}