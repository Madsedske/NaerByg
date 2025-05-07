using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class BrandResponse
    {
        [JsonPropertyName("brandId")]
        public required int BrandId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}
