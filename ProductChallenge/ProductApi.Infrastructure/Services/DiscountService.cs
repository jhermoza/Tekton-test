using Microsoft.Extensions.Options;
using ProductApi.Application.Interfaces;
using ProductApi.Infrastructure.Options;
using System.Net.Http.Json;

namespace ProductApi.Infrastructure.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly HttpClient _httpClient;
        private readonly DiscountApiOptions _options;
        public DiscountService(HttpClient httpClient, IOptions<DiscountApiOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<decimal> GetDiscountPercentageAsync(int productId)
        {
            var url = $"{_options.BaseUrl}/productId/{productId}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return 0;
            var result = await response.Content.ReadFromJsonAsync<DiscountResponse>();
            return result?.Discount ?? 0;
        }

        private class DiscountResponse
        {
            public decimal Discount { get; set; }
        }
    }
}
