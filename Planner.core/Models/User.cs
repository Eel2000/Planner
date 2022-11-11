using System.ComponentModel.DataAnnotations;

namespace Planner.core.Models;

public class User 
{
    public User()
    {
        WorkSpaces = new HashSet<WorkSpace>();
    }

    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public DateTimeOffset RegistredAt { get; set; } =  DateTimeOffset.Now;

    public virtual ICollection<WorkSpace>? WorkSpaces { get; set; }
}