using System;
using System.Collections.Generic;
using System.Text;

namespace Budget.model
{
    public enum AuditActions: short
    {
        Inserted = 1,
        Updated = 2,
        Deleted = 3,
    }
}
