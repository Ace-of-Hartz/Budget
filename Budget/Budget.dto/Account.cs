using System;
using System.Collections.Generic;
using System.Text;

namespace Budget.dto
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Decimal Money { get; set; }
        public string Description { get; set; }
    }
}
