using Budget.dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Budget.dal.i
{
    public interface IAccountLedgerRepository
    {
        Task<IEnumerable<AccountLedger>> GetLastNumEntriesAsync(int accountId, int lastX);
        Task<IEnumerable<AccountLedger>> GetEntriesBetweenDatesAsync(int accountId, DateTimeOffset startDate, DateTimeOffset endDate);

        Task AddLedgerEntryAsync(int accountId, decimal transaction, string description);
        Task UpdateLedgerEntryAsync(int accountId, long id, decimal transaction);
        Task DeleteLedgerEntryAsync(int accountId, long id);

        Task UpdateDescriptionAsync(int accountId, long id, string description);
    }
}
