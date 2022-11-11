using System.ComponentModel.DataAnnotations;

namespace Planner.core.Models;

public class Collaborator
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string ProjectId { get; set; }
    public required string UserId { get; set; }
    public required bool IsActiveCollaborator { get; set; }

    public virtual User? User { get; set; }
}