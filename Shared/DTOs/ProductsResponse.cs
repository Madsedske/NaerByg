using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Models;

namespace Shared.DTOs
{
    public class ProductsResponse
    {
        public int ProductId { get; set; }
        public required string ProductSku { get; set; }
        public required string ProductName { get; set; }
        public required string Brand { get; set; }
        public required double Price { get; set; }
        public required int Stock { get; set; }
        public required string PictureURL { get; set; }
        public required string ChainLogoURL { get; set; }
        public required string ShopName { get; set; }
        public required string ShopAddress { get; set; }
        public required string ShopPostArea { get; set; }
        public required string ShopCity { get; set; }
        public required string ShopPhoneNo { get; set; }
        public required string ShopOpeningHours { get; set; }
    }
}
