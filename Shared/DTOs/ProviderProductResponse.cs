using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class ProviderProductResponse
    {
        [JsonPropertyName("productId")]
        public required int Id { get; set; }

        [JsonPropertyName("sku")]
        public required string Sku { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("categoryId")]
        public required string CategoryId { get; set; }

        [JsonPropertyName("brandId")]
        public required string BrandId { get; set; }

        [JsonPropertyName("imageURL")]
        public required string ImageURL { get; set; }
    }
}
