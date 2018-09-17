using System;
using System.Collections.Generic;
using System.Text;

namespace Budget.model
{
    public class Paycheck : dto.Paycheck
    {
        public Paycheck(dto.Paycheck paycheck)
            : base(paycheck) { }
    }
}
