using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class ProviderRequest
    {
        public required int ChainId { get; set; }
        public required DateTime LastSynced { get; set; }
    }
}
