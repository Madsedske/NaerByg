using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class ProviderProductsResponse
    {
        public required int Id { get; set; }
        public required string Sku { get; set; }
        public required string Name { get; set; }
        public required string CategoryId { get; set; }
        public required string BrandId { get; set; }
        public required string ImageURL { get; set; }
    }
}
