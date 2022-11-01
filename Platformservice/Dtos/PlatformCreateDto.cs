using System.ComponentModel.DataAnnotations;

namespace Platformservice.Dtos
{
  public class PlatformCreateDto
  {
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Publisher { get; set; } = string.Empty;
    [Required]
    public string Cost { get; set; } = string.Empty;
  }
}