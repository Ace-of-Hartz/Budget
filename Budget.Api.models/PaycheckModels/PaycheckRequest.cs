using System;
using System.Collections.Generic;
using System.Text;

namespace Budget.Api.Models.PaycheckModels
{
    public class PaycheckRequest
    {
        public decimal Money { get; set; }
        public long PayDate { get; set; }
    }
}
