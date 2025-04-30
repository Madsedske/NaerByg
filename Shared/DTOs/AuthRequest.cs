using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class AuthRequest
    {
        public required string APIKey { get; set; }
    }
}
