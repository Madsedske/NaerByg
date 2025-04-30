using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class MTMShopsProductsResponse
    {
        public required int id { get; set; }
        public required int ProductId { get; set; }
        public required int ShopId { get; set; }
        public required double Price { get; set; }
    }
}
