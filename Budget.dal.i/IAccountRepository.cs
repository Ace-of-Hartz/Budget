using Budget.dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Budget.dal.i
{
    public interface IAccountRepository
    {
        Task<Account> GetLastAccountAsync();
        Task<Account> GetAccountAsync(int id);
        Task<IEnumerable<Account>> GetAccountsAsync();
        Task<IEnumerable<Account>> GetAccountsByTagAsync(string tag);
        Task<IEnumerable<Account>> GetAccountsByNameAsync(string name);

        Task CreateAccountAsync(string name, decimal money, string description);
        Task UpdateAccountAsync(Account account);
        Task RenameAccountAsync(int id, string name);
        Task UpdateDescriptionAsync(int id, string description);
        Task DeleteAccountAsync(int id);
    }
}
