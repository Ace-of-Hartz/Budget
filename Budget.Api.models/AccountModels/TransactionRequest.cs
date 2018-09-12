using System;
using System.Collections.Generic;
using System.Text;

namespace Budget.Api.Models.AccountModels
{
    public class TransactionRequest
    {
        public decimal Money { get; set; }
        public string Description { get; set; }
    }
}
