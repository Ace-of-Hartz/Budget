using System;
using System.Collections.Generic;
using System.Text;

namespace Budget.model
{
    public class Paycheck : dto.Paycheck
    {
        public new long PayDate { get; set; }

        public Paycheck(dto.Paycheck paycheck)
            : base(paycheck) {
            this.PayDate = base.PayDate.ToEpoch();
        }
    }
}
