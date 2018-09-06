using Budget.dal.i;
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

        public async Task<IEnumerable<model.Account>> GetAccountsAsync()
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

        public async Task<model.Account> GetAccountAsync(int id, int numEntries = 1000)
        {
            using (this._transactionHelper.CreateSqlConnection())
            {
                model.Account account = new model.Account(await this._accountRepository.GetAccountAsync(id));
                account.Tags = await this._tagsRepository.GetTagsForAccountAsync(id);
                account.LedgerEntries = (await this._accountLedgerRepository.GetLastNumEntriesAsync(numEntries)).Select(e => new model.AccountLedger(e) { Account = account });
                return account;
            }
        }
    }
}
