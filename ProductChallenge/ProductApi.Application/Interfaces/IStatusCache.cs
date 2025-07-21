namespace ProductApi.Application.Interfaces
{
    public interface IStatusCache
    {
        Task<string> GetStatusNameAsync(int status);
    }
}
