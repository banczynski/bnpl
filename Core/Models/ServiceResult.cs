namespace Core.Models
{
    public sealed record ServiceResult<T>(T Data, string[]? Messages = null);
}
