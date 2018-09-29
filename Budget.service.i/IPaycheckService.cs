using Budget.model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Budget.service.i
{
    public interface IPaycheckService
    {
        Task<Paycheck> GetPaycheckAsync(int id);
        Task<IEnumerable<Paycheck>> GetLastXPaychecksAsync(int numPaychecks);
        Task<IEnumerable<Paycheck>> GetPaychecksBetweenAsync(DateTime startDate, DateTime endDate);

        Task<Paycheck> CreatePaycheckAsync(decimal money, DateTime paydate);
        Task UpdatePaycheckAsync(Paycheck paycheck);
        Task UpdatePaycheckAmountAsync(int id, decimal money);
        Task UpdatePaycheckUnallocatedAsync(int id, decimal unallocatedMoney);
        Task UpdatePaycheckDateAsync(int id, DateTime paydate);
        Task DeletePaycheckAsync(int id);
    }
}