using System.ComponentModel.DataAnnotations;

namespace Planner.core.DTOs;

public class WorkspaceDTO
{
    [Required(ErrorMessage = "must specify the workspace name please")]
    public string Name { get; set; }

    [Required(ErrorMessage = "the description is required"),
        StringLength(250, ErrorMessage = "the description must at least have 10 charaters and no more than 250",
        MinimumLength = 10)]
    public string Description { get; set; }

    public string UserId { get; set; }


}
