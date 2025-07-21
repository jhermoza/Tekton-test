namespace ProductApi.Application.Interfaces
{
    public interface IDiscountService
    {
        Task<decimal> GetDiscountPercentageAsync(int productId);
    }
}
