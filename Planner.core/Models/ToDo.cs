using System.ComponentModel.DataAnnotations;

namespace Planner.core.Models;

public class ToDo
{
    [Key]
    public required string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Name { get; set; }
    public required string Description { get; set; }
    public DateTimeOffset StartDate { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset? EndDate { get; set;}
    public string ProjectId { get; set; }

    /// <summary>
    /// serialized list of this todo tasks
    /// </summary>
    public string? ToDoList { get; set; }
    public string? CollaboratorId { get; set; }

    public virtual Collaborator? Collaborator { get; set; }
    public virtual Project? Project { get; set; }
}