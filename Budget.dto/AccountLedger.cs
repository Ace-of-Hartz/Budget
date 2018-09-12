using System;
using System.Collections.Generic;
using System.Text;

namespace Budget.dto
{
    public class AccountLedger
    {
        public long Id { get; set; }
        public decimal Transaction { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string Description { get; set; }

        public AccountLedger() { }
        public AccountLedger(AccountLedger accountLedger)
        {
            this.Id = accountLedger.Id;
            this.Transaction = accountLedger.Transaction;
            this.TimeStamp = accountLedger.TimeStamp;
            this.Description = accountLedger.Description;
        }
    }
}
