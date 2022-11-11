using System.ComponentModel.DataAnnotations;

namespace Planner.core.Models;

public class Tag
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? Name { get; set; }
    public string? ColorHex { get; set; }
}