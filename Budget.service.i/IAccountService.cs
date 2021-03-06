﻿using Budget.Api.Models.AccountModels;
using Budget.model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.service.i
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAccountsAsync();
        Task<Account> GetAccountAsync(int id, int numEntries);
        Task<Account> GetAccountAsync(int id, DateTimeOffset startDate, DateTimeOffset endDate);

        Task<IEnumerable<AccountLedger>> GetAccountLedgerEntriesAsync(int id, DateTimeOffset startDate, DateTimeOffset endDate);
        Task<IEnumerable<AccountLedger>> GetAccountLedgerEntriesAsync(int id, int? paycheckId);

        Task<Account> CreateAccountAsync(string name, decimal initialFunds, string description);
        Task UpdateAccountAsync(Account account);
        Task RenameAccountAsync(int id, string name);
        Task UpdateDescriptionAsync(int id, string description);
        Task DeleteAccountAsync(int id);

        Task AddTagAsync(int accountId, string tag, string description);
        Task RemoveTagAsync(int id);

        Task<Account> AddTransactionAsync(int accountId, int? paycheckId, decimal money, string description, TransactionType transactionType);
        Task UpdateTransactionAmountAsync(int accountId, long transactionId, decimal money);
        Task UpdateTransactionDescriptionAsync(int accountId, long transactionId, string description);
        Task DeleteTransactionAsync(int accountId, long transactionId);
    }
}
