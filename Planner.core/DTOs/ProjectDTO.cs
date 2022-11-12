namespace Planner.core.DTOs
{
    public class ProjectDTO
    {
        public required string ProjectName { get; set; }
        public string? ProjectDescription { get; set; }
        public required string WorkSpaceId { get; set; }
        public required bool IsArchived { get; set; }
        public required string TagId { get; set; }
    }
}
