using System;
using System.Collections.Generic;
using System.Text;

namespace Budget.Api.Models.AccountModels
{
    public class AccountRequest
    {
        public Decimal Money { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
