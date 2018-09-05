using Budget.dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Budget.dal.i
{
    public interface IAccountLedgerRepository
    {
        Task<IEnumerable<AccountLedger>> GetLastNumEntriesAsync(int lastX);
        Task<IEnumerable<AccountLedger>> GetEntriesBetweenDatesAsync(DateTimeOffset startDate, DateTimeOffset endDate);

        Task AddLedgerEntryAsync(decimal transaction, string description);
        Task UpdateLedgerEntryAsync(long id, decimal transaction, string description);
        Task DeleteLedgerEntryAsync(long id);

        Task UpdateDescriptionAsync(long id, string description);
    }
}
