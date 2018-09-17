using System;
using System.Collections.Generic;
using System.Text;

namespace Budget.dto
{
    public class Paycheck
    {
        public int Id { get; set; }
        public decimal Money { get; set; }
        public decimal UnallocatedMoney { get; set; }
        public DateTime PayDate { get; set; }

        public Paycheck() { }
        public Paycheck(Paycheck paycheck)
        {
            this.Id = paycheck.Id;
            this.Money = paycheck.Money;
            this.UnallocatedMoney = paycheck.UnallocatedMoney;
            this.PayDate = paycheck.PayDate;
        }
    }
}
