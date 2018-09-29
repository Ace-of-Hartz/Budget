using Budget.dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Budget.model
{
    public class Account : dto.Account
    {
        public IEnumerable<Tags> Tags { get; set; } = new Tags[0];
        public IEnumerable<AccountLedger> LedgerEntries { get; set; } = new AccountLedger[0];

        public Account() : base() { }
        public Account(dto.Account baseAccount)
            : base(baseAccount) { }
    }
}
