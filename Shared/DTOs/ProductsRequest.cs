using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class ProductsRequest
    {
        public required string SearchTerm { get; set; }
    }
}
