using System;
using System.Collections.Generic;
using System.Text;

namespace Taylor.Infrastructure.Entity
{
    public class Member
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public MemberType Type { get; set; }
        public Team Team { get; set; }
    }
}
