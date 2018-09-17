using Budget.dal.i;
using Budget.dal.sqlserver;
using Budget.model;
using Budget.service.i;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.service
{
    public class AccountService : IAccountService
    {
        private IRepositoryService _repositoryService;

        public AccountService(IRepositoryService repositoryService)
        {
            this._repositoryService = repositoryService;
        }

        public async Task<IEnumerable<Account>> GetAccountsAsync()
        {
            IEnumerable<dto.Tags> tags = new dto.Tags[0];
            using (var transactionHelper = this._repositoryService.GetTransactionHelper())
            {
                var tagsRepository = this._repositoryService.GetTagsRepository(transactionHelper);
                tags = (await tagsRepository.GetTagsAsync()).ToList();
            }
            using (var transactionHelper = this._repositoryService.GetTransactionHelper())
            {
                var accountRepository = this._repositoryService.GetAccountRepository(transactionHelper);
                IEnumerable<Account> accounts = (await accountRepository.GetAccountsAsync()).Select(a => new Account(a)).ToList();
                foreach (var account in accounts)
                {
                    account.Tags = tags.Where(t => t.AccountId == account.Id).ToList();
                }
                return accounts;
            }
        }

        public async Task<Account> GetAccountAsync(int id, int numEntries = 1000)
        {
            Account account;
            IEnumerable<dto.Tags> tags = new dto.Tags[0];
            IEnumerable<AccountLedger> accountLedgers = new AccountLedger[0];

            using (var transactionHelper = this._repositoryService.GetTransactionHelper())
            {
                var tagsRepository = this._repositoryService.GetTagsRepository(transactionHelper);
                tags = (await tagsRepository.GetTagsForAccountAsync(id)).ToList();
            }
            using (var transactionHelper = this._repositoryService.GetTransactionHelper())
            {
                var accountRepository = this._repositoryService.GetAccountRepository(transactionHelper);
                account = new Account(await accountRepository.GetAccountAsync(id));
            }
            if (numEntries > 0)
            {
                using (var transactionHelper = this._repositoryService.GetTransactionHelper())
                {
                    var accountLedgerRepository = this._repositoryService.GetAccountLedgerRepository(transactionHelper);
                    accountLedgers = (await accountLedgerRepository.GetLastNumEntriesAsync(id, numEntries)).Select(e => new AccountLedger(e, id)).ToList();
                }
            }
            account.Tags = tags;
            account.LedgerEntries = accountLedgers;
            return account;
        }

        public async Task<Account> GetAccountAsync(int id, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            Account account;
            IEnumerable<dto.Tags> tags = new dto.Tags[0];
            IEnumerable<AccountLedger> accountLedgers = new AccountLedger[0];

            using (var transactionHelper = this._repositoryService.GetTransactionHelper())
            {
                var tagsRepository = this._repositoryService.GetTagsRepository(transactionHelper);
                tags = (await tagsRepository.GetTagsForAccountAsync(id)).ToList();
            }
            using (var transactionHelper = this._repositoryService.GetTransactionHelper())
            {
                var accountRepository = this._repositoryService.GetAccountRepository(transactionHelper);
                account = new model.Account(await accountRepository.GetAccountAsync(id));
            }
            using (var transactionHelper = this._repositoryService.GetTransactionHelper())
            {
                var accountLedgerRepository = this._repositoryService.GetAccountLedgerRepository(transactionHelper);
                accountLedgers = (await accountLedgerRepository.GetEntriesBetweenDatesAsync(id, startDate, endDate)).Select(e => new AccountLedger(e, id)).ToList();
            }
            account.Tags = tags;
            account.LedgerEntries = accountLedgers;
            return account;
        }

        public async Task<IEnumerable<AccountLedger>> GetAccountLedgerEntriesAsync(int accountId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            using (var transactionHelper = this._repositoryService.GetTransactionHelper())
            {
                var accountLedgerRepository = this._repositoryService.GetAccountLedgerRepository(transactionHelper);
                return (await accountLedgerRepository.GetEntriesBetweenDatesAsync(accountId, startDate, endDate)).Select(e => new AccountLedger(e, accountId)).ToList();
            }
        }

        public async Task<Account> CreateAccountAsync(string name, string description)
        {
            using (var transactionHelper = this._repositoryService.GetTransactionHelper())
            {
                var accountRepository = this._repositoryService.GetAccountRepository(transactionHelper);
                await accountRepository.CreateAccountAsync(name, description);
                Account newAccount = new Account(await accountRepository.GetLastAccountAsync());
                transactionHelper.CommitTransaction();
                return newAccount;
            }
        }

        public async Task RenameAccountAsync(int id, string name)
        {
            using (var transactionHelper = this._repositoryService.GetTransactionHelper())
            {
                var accountRepository = this._repositoryService.GetAccountRepository(transactionHelper);
                await accountRepository.RenameAccountAsync(id, name);
                transactionHelper.CommitTransaction();
            }
        }

        public async Task UpdateDescriptionAsync(int id, string description)
        {
            using (var transactionHelper = this._repositoryService.GetTransactionHelper())
            {
                var accountRepository = this._repositoryService.GetAccountRepository(transactionHelper);
                await accountRepository.UpdateDescriptionAsync(id, description);
                transactionHelper.CommitTransaction();
            }
        }

        public async Task DeleteAccountAsync(int id)
        {
            using (var transactionHelper = this._repositoryService.GetTransactionHelper())
            {
                var accountRepository = this._repositoryService.GetAccountRepository(transactionHelper);
                await accountRepository.DeleteAccountAsync(id);
                transactionHelper.CommitTransaction();
            }
        }

        public async Task AddTagAsync(int id, string tag, string description)
        {
            using (var transactionHelper = this._repositoryService.GetTransactionHelper())
            {
                var tagsRepository = this._repositoryService.GetTagsRepository(transactionHelper);
                await tagsRepository.AddTagAsync(id, tag, description);
                transactionHelper.CommitTransaction();
            }
        }

        public async Task RemoveTagAsync(int tagId)
        {
            using (var transactionHelper = this._repositoryService.GetTransactionHelper())
            {
                var tagsRepository = this._repositoryService.GetTagsRepository(transactionHelper);
                await tagsRepository.RemoveTagAsync(tagId);
                transactionHelper.CommitTransaction();
            }
        }

        public async Task<Account> AddTransactionAsync(int id, int? paycheckId, decimal transaction, string description, TransactionType transactionType)
        {
            if (transactionType == TransactionType.Deposite && transaction < 0) { transaction *= -1; }
            else if (transactionType == TransactionType.Withdraw && transaction > 0) { transaction *= -1; }

            using (var transactionHelper = this._repositoryService.GetTransactionHelper())
            {
                var accountLedgerRepository = this._repositoryService.GetAccountLedgerRepository(transactionHelper);
                await accountLedgerRepository.AddLedgerEntryAsync(id, paycheckId, transaction, description);
                transactionHelper.CommitTransaction();
            }
            return await this.GetAccountAsync(id, 100);
        }

        public async Task<Account> WithdrawMoneyAsync(int id, int? paycheckId, decimal transaction, string description)
        {
            if (transaction > 0) { transaction *= -1; }
            using (var transactionHelper = this._repositoryService.GetTransactionHelper())
            {
                var accountLedgerRepository = this._repositoryService.GetAccountLedgerRepository(transactionHelper);
                await accountLedgerRepository.AddLedgerEntryAsync(id, paycheckId, transaction, description);
                transactionHelper.CommitTransaction();
            }
            return await this.GetAccountAsync(id, 100);
        }

        public async Task UpdateTransactionAmountAsync(int accountId, long transactionId, decimal transaction)
        {
            using (var transactionHelper = this._repositoryService.GetTransactionHelper())
            {
                var accountLedgerRepository = this._repositoryService.GetAccountLedgerRepository(transactionHelper);
                await accountLedgerRepository.UpdateLedgerEntryAsync(accountId, transactionId, transaction);
                transactionHelper.CommitTransaction();
            }
        }

        public async Task UpdateTransactionDescriptionAsync(int accountId, long transactionId, string description)
        {
            using (var transactionHelper = this._repositoryService.GetTransactionHelper())
            {
                var accountLedgerRepository = this._repositoryService.GetAccountLedgerRepository(transactionHelper);
                await accountLedgerRepository.UpdateDescriptionAsync(accountId, transactionId, description);
                transactionHelper.CommitTransaction();
            }
        }

        public async Task DeleteTransactionAsync(int accountId, long tranactionId)
        {
            using (var transactionHelper = this._repositoryService.GetTransactionHelper())
            {
                var accountLedgerRepository = this._repositoryService.GetAccountLedgerRepository(transactionHelper);
                await accountLedgerRepository.DeleteLedgerEntryAsync(accountId, tranactionId);
                transactionHelper.CommitTransaction();
            }
        }

        public async Task<IEnumerable<AccountLedger>> GetAccountLedgerEntriesAsync(int id, int? paycheckId)
        {
            using (var transactionHelper = this._repositoryService.GetTransactionHelper())
            {
                var accountLedgerRepository = this._repositoryService.GetAccountLedgerRepository(transactionHelper);
                return (await accountLedgerRepository.GetPaycheckEntriesAsync(id, paycheckId)).Select(p => new AccountLedger(p, id)).ToList();
            }
        }
    }
}
