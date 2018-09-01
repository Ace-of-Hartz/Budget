using System;
using System.Collections.Generic;
using System.Text;

namespace Budget.model
{
    public class AccountLedger : dto.AccountLedger
    {
        public Account Account { get; set; }
    }
}
