﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class ProductRequest
    {
        public string? SearchTerm { get; set; }
        public int? Category { get; set; }
    }
}
