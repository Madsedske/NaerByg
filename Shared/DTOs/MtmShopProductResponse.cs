using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class MtmShopProductResponse
    {
        [JsonPropertyName("shopProductId")]
        public required int Id { get; set; }

        [JsonPropertyName("productId")]
        public required int ProductId { get; set; }

        [JsonPropertyName("shopId")]
        public required int ShopId { get; set; }

        [JsonPropertyName("quantity")]
        public required double Stock { get; set; }

        [JsonPropertyName("price")]
        public required double Price { get; set; }
    }
}
