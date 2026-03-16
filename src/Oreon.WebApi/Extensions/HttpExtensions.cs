using System.Text.Json;
using Oreon.WebApi.Helpers;

namespace Oreon.WebApi.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, PaginationHeader header)
        {
            var jsonOtions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            response.Headers.Add("Pagination", JsonSerializer.Serialize(header, jsonOtions));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
