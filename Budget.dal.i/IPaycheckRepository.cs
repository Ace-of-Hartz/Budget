using Budget.dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Budget.dal.i
{
    public interface IPaycheckRepository
    {
        Task<Paycheck> GetLastPaycheck();
        Task<Paycheck> GetPaycheckAsync(int id);
        Task<IEnumerable<Paycheck>> GetLastXPaychecksAsync(int numPaychecks);
        Task<IEnumerable<Paycheck>> GetPaychecksBetweenAsync(DateTimeOffset startDate, DateTimeOffset endDate);
        
        Task CreatePaycheckAsync(decimal money, DateTimeOffset paydate);
        Task UpdatePaycheckAmountAsync(int id, decimal money);
        Task UpdatePaycheckUnallocatedAsync(int id, decimal unallocatedMoney);
        Task UpdatePaycheckDateAsync(int id, DateTimeOffset paydate);
        Task DeletePaycheckAsync(int id);
    }
}
