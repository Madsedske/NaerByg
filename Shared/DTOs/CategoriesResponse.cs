using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class CategoriesResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required int ParentId { get; set; }
    }
}
