using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class ShopResponse
    {
        [JsonPropertyName("shopId")]
        public required int Id { get; set; }

        [JsonPropertyName("address")]
        public required string Address { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("postAreaId")]
        public required int PostAreaId { get; set; }

        [JsonPropertyName("phoneNo")]
        public required string PhoneNo { get; set; }

        [JsonPropertyName("openingHours")]
        public required string OpeningHours { get; set; }
    }
}
