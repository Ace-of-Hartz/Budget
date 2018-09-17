using Budget.model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Budget.Api.Models.AccountModels
{
    public class TransactionRequest
    {
        public int? PaycheckId { get; set; }
        public decimal Money { get; set; }
        public string Description { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
