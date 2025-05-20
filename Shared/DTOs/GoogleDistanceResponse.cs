using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class GoogleDistanceResponse
    {
        public List<Row>? rows { get; set; }
        public string? status { get; set; }
    }

    public class Row
    {
        public List<Element>? elements { get; set; }
    }

    public class Element
    {
        public Info? distance { get; set; }
        public Info? duration { get; set; }
        public Info? status { get; set; }
    }

    public class Info
    {
        public string? text { get; set; }
        public int value { get; set; }
    }
}
