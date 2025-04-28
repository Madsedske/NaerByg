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
        public required List<Product> Products { get; set; }
    }
}
