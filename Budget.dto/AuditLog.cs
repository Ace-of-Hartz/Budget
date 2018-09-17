using System;
using System.Collections.Generic;
using System.Text;

namespace Budget.dto
{
    public class AuditLog
    {
        public int Id { get; set; }
        public int? PaycheckId { get; set; }
        public int LedgerId { get; set; }
        public short Action { get; set; }
        public DateTime Timestamp { get; set; }
        public Decimal Transaction { get; set; }
    }
}
