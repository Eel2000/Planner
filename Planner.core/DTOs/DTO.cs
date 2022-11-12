namespace Planner.core.DTOs
{
    public record Response<T>(Status Status, string? message = null, T? data = null) where T : class;


    public enum Status
    {
        Success, Failure, Error
    }
}
