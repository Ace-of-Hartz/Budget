using Budget.dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Budget.model
{
    public class Account : dto.Account
    {
        public Account(dto.Account baseAccount)
            : base(baseAccount) { }

        public IEnumerable<Tags> Tags { get; set; }
        public IEnumerable<AccountLedger> LedgerEntries { get; set; }
    }
}
