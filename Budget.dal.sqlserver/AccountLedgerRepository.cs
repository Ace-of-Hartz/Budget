﻿using Budget.dal.i;
using Budget.dto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Budget.dal.sqlserver
{
    public class AccountLedgerRepository : IAccountLedgerRepository
    {
        private ITransactionHelper _transactionHelper;

        public AccountLedgerRepository(ITransactionHelper transactionHelper)
        {
            this._transactionHelper = transactionHelper;
        }

        public async Task AddLedgerEntryAsync(int accountId, int? paycheckId, decimal transaction, string description = null)
        {
            string sql = $@"
INSERT INTO [{this.GenerateTableName(accountId)}] ([PaycheckId], [Transaction], [Description])
VALUES (@paycheckId, @transaction, @description);
";
            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction))
            {
                command.Parameters.Add(new SqlParameter("@paycheckId", paycheckId));
                command.Parameters.Add(new SqlParameter("@transaction", transaction));
                command.Parameters.Add(new SqlParameter("@description", description));
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteLedgerEntryAsync(int accountId, long id)
        {
            string sql = $@"
DELETE FROM [{this.GenerateTableName(accountId)}]
WHERE [Id] = @id;
";
            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction))
            {
                command.Parameters.Add(new SqlParameter("@id", id));
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<AccountLedger>> GetEntriesBetweenDatesAsync(int accountId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            string sql = $@"
SELECT [Id], [PaycheckId], [Transaction], [Timestamp], [Description]
FROM [{this.GenerateTableName(accountId)}]
WHERE [Timestamp] >= @startDate
AND [Timestamp] <= @endDate
ORDER BY [Id];
";
            this._transactionHelper.SqlCommand = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction);
            this._transactionHelper.SqlCommand.Parameters.Add(new SqlParameter("@startDate", startDate));
            this._transactionHelper.SqlCommand.Parameters.Add(new SqlParameter("@endDate", endDate));
            this._transactionHelper.SqlDataReader = await this._transactionHelper.SqlCommand.ExecuteReaderAsync();
            return this._transactionHelper.SqlDataReader.GetDtos<AccountLedger>();
        }

        public async Task<IEnumerable<AccountLedger>> GetLastNumEntriesAsync(int accountId, int lastX)
        {
            string sql = $@"
SELECT TOP (@numRows) [Id], [PaycheckId], [Transaction], [Timestamp], [Description]
FROM [{this.GenerateTableName(accountId)}]
ORDER BY [Id];
";
            this._transactionHelper.SqlCommand = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction);
            this._transactionHelper.SqlCommand.Parameters.Add(new SqlParameter("@numRows", lastX));
            this._transactionHelper.SqlDataReader = await this._transactionHelper.SqlCommand.ExecuteReaderAsync();
            return this._transactionHelper.SqlDataReader.GetDtos<AccountLedger>();
        }

        public async Task<IEnumerable<AccountLedger>> GetPaycheckEntriesAsync(int accountId, int? paycheckId)
        {
            string sql = $@"
SELECT [Id], [PaycheckId], [Transaction], [Timestamp], [Description]
FROM [{this.GenerateTableName(accountId)}]
WHERE [PaycheckId] = @paycheckId
ORDER BY [Id];
";
            this._transactionHelper.SqlCommand = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction);
            this._transactionHelper.SqlCommand.Parameters.Add(new SqlParameter("@paycheckId", paycheckId));
            this._transactionHelper.SqlDataReader = await this._transactionHelper.SqlCommand.ExecuteReaderAsync();
            return this._transactionHelper.SqlDataReader.GetDtos<AccountLedger>();
        }

        public async Task UpdateDescriptionAsync(int accountId, long id, string description)
        {
            string sql = $@"
UPDATE [{this.GenerateTableName(accountId)}]
SET [Description] = @description
WHERE [Id] = @id;
";
            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction))
            {
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Parameters.Add(new SqlParameter("@description", description));
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateLedgerEntryAsync(int accountId, long id, decimal transaction)
        {
            string sql = $@"
UPDATE [{this.GenerateTableName(accountId)}]
SET [Transaction] = @transaction
WHERE [Id] = @id;
";
            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction))
            {
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Parameters.Add(new SqlParameter("@transaction", transaction));
                await command.ExecuteNonQueryAsync();
            }
        }

        private string GenerateTableName(int accountId)
        {
            return $"AccountLedger_{accountId}";
        }
    }
}
