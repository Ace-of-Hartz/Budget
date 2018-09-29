using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Budget.Api.Dependencies;
using Budget.Api.Models.AccountModels;
using Budget.dal.i;
using Budget.dal.sqlserver;
using Budget.model;
using Budget.service;
using Budget.service.i;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/accounts")]
    public class AccountController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccountsAsync()
        {
            return Ok(await RepositoryServiceProvider.AccountService.GetAccountsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccountAsync(int id, [FromQuery] DateTimeOffset? startDate, [FromQuery] DateTimeOffset? endDate)
        {
            startDate = startDate ?? DateTimeOffset.Now.AddDays(-90);
            endDate = endDate ?? DateTimeOffset.Now;
            // TODO add -90 to config values
            return Ok(await RepositoryServiceProvider.AccountService.GetAccountAsync(id, startDate.Value, endDate.Value));
        }

        [HttpGet("{id}/entries")]
        public async Task<ActionResult<IEnumerable<AccountLedger>>> GetAccountLedgersAsync(int id, [FromQuery] DateTimeOffset? startDate, [FromQuery] DateTimeOffset? endDate)
        {
            startDate = startDate ?? DateTimeOffset.Now.AddDays(-90);
            endDate = endDate ?? DateTimeOffset.Now;
            // TODO add -90 to config values
            return Ok(await RepositoryServiceProvider.AccountService.GetAccountLedgerEntriesAsync(id, startDate.Value, endDate.Value));
        }

        [HttpPost]
        public async Task<ActionResult<Account>> CreateAccountAsync([FromBody] AccountRequest account)
        {
            return Ok(await RepositoryServiceProvider.AccountService.CreateAccountAsync(account.Name, account.Money, account.Description));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAccountAsync(int id, [FromBody] Account account)
        {
            account.Id = id;
            await RepositoryServiceProvider.AccountService.UpdateAccountAsync(account);
            return Ok();
        }

        [HttpPut("{id}/name")]
        public async Task<ActionResult<Account>> RenameAccountAsync(int id, [FromBody] AccountRequest account)
        {
            IAccountService accountService = RepositoryServiceProvider.AccountService;
            await accountService.RenameAccountAsync(id, account.Name);
            return Ok(accountService.GetAccountAsync(id, 0));
        }

        [HttpPut("{id}/name")]
        public async Task<ActionResult<Account>> UpdateDescriptionAccountAsync(int id, [FromBody] AccountRequest account)
        {
            IAccountService accountService = RepositoryServiceProvider.AccountService;
            await accountService.UpdateDescriptionAsync(id, account.Description);
            return Ok(await accountService.GetAccountAsync(id, 0));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAccountAsync(int id)
        {
            await RepositoryServiceProvider.AccountService.DeleteAccountAsync(id);
            return Ok();
        }

        [HttpPost("{id}/tags")]
        public async Task<ActionResult<Account>> AddTagAsync(int id, [FromBody] TagRequest tag)
        {
            IAccountService accountService = RepositoryServiceProvider.AccountService;
            await accountService.AddTagAsync(id, tag.Tag, tag.Description);
            return Ok(await accountService.GetAccountAsync(id, 0));
        }

        [HttpDelete("{id}/tags/{tagId}")]
        public async Task<ActionResult<Account>> DeleteTagAsync(int id, int tagId)
        {
            IAccountService accountService = RepositoryServiceProvider.AccountService;
            await accountService.RemoveTagAsync(tagId);
            return Ok(await accountService.GetAccountAsync(id, 0));
        }

        [HttpPost("{id}/transactions")]
        public async Task<ActionResult<Account>> WithdrawMoneyAsync(int id, [FromBody] TransactionRequest transaction)
        {           
            return Ok(await RepositoryServiceProvider.AccountService.AddTransactionAsync(id, transaction.PaycheckId, transaction.Money, transaction.Description, transaction.TransactionType));
        }

        [HttpPut("{id}/transactions/{transactionId}/amount")]
        public async Task<ActionResult> UpdateTransactionAmountAsync(int id, long transactionId, TransactionRequest transaction)
        {
            await RepositoryServiceProvider.AccountService.UpdateTransactionAmountAsync(id, transactionId, transaction.Money);
            return Ok();
        }

        [HttpPut("{id}/transactions/{transactionId}/description")]
        public async Task<ActionResult> UpdateTransactionDescriptionAsync(int id, long transactionId, TransactionRequest transaction)
        {
            await RepositoryServiceProvider.AccountService.UpdateTransactionDescriptionAsync(id, transactionId, transaction.Description);
            return Ok();
        }

        [HttpDelete("{id}/transactions/{transactionId}")]
        public async Task<ActionResult> DeleteTransactionAsync(int id, long transactionId)
        {
            await RepositoryServiceProvider.AccountService.DeleteTransactionAsync(id, transactionId);
            return Ok();
        }
    }
}