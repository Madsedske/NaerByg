using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class PostAreaResponse
    {
        [JsonPropertyName("postAreaId")]
        public required int Id { get; set; }

        [JsonPropertyName("code")]
        public required int PostArea { get; set; }
    }
}
