using System;
using System.Collections.Generic;
using System.Text;

namespace Budget.model
{
    public class AuditLog : dto.AuditLog
    {
        public AuditActions AuditAction { get; set; }
        public IEnumerable<AccountLedger> LedgerEntries { get; set; }
    }
}
