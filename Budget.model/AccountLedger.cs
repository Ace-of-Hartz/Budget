using System;
using System.Collections.Generic;
using System.Text;

namespace Budget.model
{
    public class AccountLedger : dto.AccountLedger
    {
        public int AccountId { get; set; }
        public AccountLedger(dto.AccountLedger baseAccountLedger, int accountId)
            : base(baseAccountLedger)
        {
            this.AccountId = accountId;
        }
    }
}
