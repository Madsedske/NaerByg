using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class ShopsResponse
    {
        public required int Id { get; set; }
        public required string Address { get; set; }
        public required string Name { get; set; }
        public required int PostAreaId { get; set; }
        public required string PhoneNo { get; set; }
    }
}
