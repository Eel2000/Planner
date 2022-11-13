using System.ComponentModel.DataAnnotations;

namespace Planner.core.Models;

public class Project
{
    public Project()
    {
        ToDos = new HashSet<ToDo>();
        Collaborators= new HashSet<Collaborator>();
    }

    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string ProjectName { get; set; }
    public string? ProjectDescription { get; set; }
    public DateTimeOffset CreationDate { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset LastUpdate { get; set; } = DateTimeOffset.Now;
    public required string WorkSpaceId { get; set; }
    public required bool IsArchived { get; set; }
    public required string TagId { get; set; }


    public virtual Tag? Tag { get; set; }
    public virtual WorkSpace? WorkSpace { get; set; }
    public virtual ICollection<ToDo>? ToDos { get; set; }
    public virtual ICollection<Collaborator>? Collaborators { get; set; }
}