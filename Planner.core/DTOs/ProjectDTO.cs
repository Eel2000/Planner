using System.ComponentModel.DataAnnotations;

namespace Planner.core.DTOs;

public class ProjectDTO
{
    [Required(ErrorMessage ="Please specify the project name")]
    public string? ProjectName { get; set; }
    [Required(ErrorMessage ="Please specify the project description")]
    public string? ProjectDescription { get; set; }
    public string? WorkSpaceId { get; set; }
    public bool IsArchived { get; set; }
    public string? TagId { get; set; }
}
