using System.ComponentModel.DataAnnotations;

namespace Planner.core.Models;
#nullable disable
public class WorkSpace
{
    public WorkSpace()
    {
        Projects= new HashSet<Project>();
    }

    [Key]
    public required string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Name { get; set; }
    public required string Description { get; set; }
    public DateTimeOffset? CreateAt { get; set; } = DateTimeOffset.Now;

    /// <summary>
    /// The owner/ creator of the workspace(current)
    /// </summary>
    public required string UserId { get; set; }


    public virtual User User { get; set; }
    public virtual ICollection<Project> Projects { get; set; }
}
