using System;
using System.Collections.Generic;
using System.Text;

namespace Budget.dto
{
    public class AccountLedger
    {
        public int Id { get; set; }
        public decimal Transaction { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string Description { get; set; }
    }
}
