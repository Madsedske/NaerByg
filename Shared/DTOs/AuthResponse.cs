using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class AuthResponse
    {
        public required string Token { get; set; }

        /*public required DateTime ExpiryTime { get; set; }*/
    }
}
