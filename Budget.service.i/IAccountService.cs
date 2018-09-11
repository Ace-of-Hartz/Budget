using Budget.model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.service.i
{
    public interface IAccountService
    {
        Task<Account> GetAccountAsync(int id, int numEntries);
        Task<IEnumerable<Account>> GetAccountsAsync();

        Task<Account> CreateAccountAsync(string name, string description);
        Task RenameAccountAsync(int id, string name);
        Task UpdateDescriptionAsync(int id, string description);
        Task DeleteAccountAsync(int id);

        Task AddTagAsync(int id, string tag, string description);
        Task RemoveTagAsync(int id);

        Task<Account> DepositeMoneyAsync(int id, decimal money, string description);
        Task<Account> WithdrawMoneyAsync(int id, decimal money, string description);
    }
}
