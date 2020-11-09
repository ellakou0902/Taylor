using System;
using System.Collections.Generic;
using System.Text;

namespace Taylor.Infrastructure.Entity.DmsResponse
{
    public class SearchParameter
    {
        public string Field { get; set; }
        public string Op { get; set; }
        public string Value { get; set; }
    }
}
