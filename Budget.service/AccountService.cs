using Budget.dal.i;
using Budget.model;
using Budget.service.i;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.service
{
    public class AccountService : IAccountService
    {
        private ITransactionHelper _transactionHelper;
        private IAccountRepository _accountRepository;
        private IAccountLedgerRepository _accountLedgerRepository;
        private ITagsRepository _tagsRepository;

        public AccountService(
            ITransactionHelper transactionHelper,
            IAccountRepository accountRepository,
            IAccountLedgerRepository accountLedgerRepository,
            ITagsRepository tagsRepository)
        {
            this._transactionHelper = transactionHelper;
            this._accountRepository = accountRepository;
            this._accountLedgerRepository = accountLedgerRepository;
            this._tagsRepository = tagsRepository;
        }

        public async Task<IEnumerable<Account>> GetAccountsAsync()
        {
            using (this._transactionHelper.CreateSqlConnection())
            {
                IEnumerable<dto.Tags> tags = await this._tagsRepository.GetTagsAsync();
                IEnumerable<model.Account> accounts = (await this._accountRepository.GetAccountsAsync()).Select(a => new model.Account(a));
                foreach (var account in accounts)
                {
                    account.Tags = tags.Where(t => t.AccountId == account.Id);
                }
                return accounts;
            }
        }

        public async Task<Account> GetAccountAsync(int id, int numEntries = 1000)
        {
            using (this._transactionHelper.CreateSqlConnection())
            {
                model.Account account = new model.Account(await this._accountRepository.GetAccountAsync(id));
                account.Tags = await this._tagsRepository.GetTagsForAccountAsync(id);
                account.LedgerEntries = (await this._accountLedgerRepository.GetLastNumEntriesAsync(id, numEntries)).Select(e => new model.AccountLedger(e) { Account = account });
                return account;
            }
        }

        public async Task<Account> CreateAccountAsync(string name, string description)
        {
            await this._accountRepository.CreateAccountAsync(name, description);
            return new Account(await this._accountRepository.GetLastAccountAsync());
        }

        public async Task RenameAccountAsync(int id, string name)
        {
            await this._accountRepository.RenameAccountAsync(id, name);
        }

        public async Task UpdateDescriptionAsync(int id, string description)
        {
            await this._accountRepository.UpdateDescriptionAsync(id, description);
        }

        public async Task DeleteAccountAsync(int id)
        {
            await this._accountRepository.DeleteAccountAsync(id);
        }

        public async Task AddTagAsync(int id, string tag, string description)
        {
            await this._tagsRepository.AddTagAsync(id, tag, description);
        }

        public async Task RemoveTagAsync(int tagId)
        {
            await this._tagsRepository.RemoveTagAsync(tagId);
        }

        public async Task<Account> DepositeMoneyAsync(int id, decimal transaction, string description)
        {
            await this._accountLedgerRepository.AddLedgerEntryAsync(id, transaction, description);
            return new Account(await this._accountRepository.GetAccountAsync(id));
        }

        public async Task<Account> WithdrawMoneyAsync(int id, decimal transaction, string description)
        {
            await this._accountLedgerRepository.AddLedgerEntryAsync(id, transaction, description);
            return new Account(await this._accountRepository.GetAccountAsync(id));
        }

        public async Task<Account> UpdateTransactionAsync(int accountId, long transactionId, decimal transaction)
        {
            await this._accountLedgerRepository.UpdateLedgerEntryAsync(accountId, transactionId, transaction);
            return new Account(await this._accountRepository.GetAccountAsync(accountId));
        }
    }
}
