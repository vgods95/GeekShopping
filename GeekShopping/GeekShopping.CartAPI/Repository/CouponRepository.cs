using GeekShopping.CartAPI.Data.ValueObjects;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

namespace GeekShopping.CartAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly HttpClient _httpClient;
        public CouponRepository(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<CouponVO> GetCouponByCouponCode(string couponCode, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"api/v1/Coupon/{couponCode}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK) return new CouponVO();

            return JsonConvert.DeserializeObject<CouponVO>(content);
        }
    }
}
