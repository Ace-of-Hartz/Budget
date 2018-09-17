using Budget.dal.i;
using Budget.dto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.dal.sqlserver
{
    public class PaycheckRepository : IPaycheckRepository
    {
        private ITransactionHelper _transactionHelper;

        public PaycheckRepository(ITransactionHelper transactionHelper)
        {
            this._transactionHelper = transactionHelper;
        }

        public async Task CreatePaycheckAsync(decimal money, DateTimeOffset paydate)
        {
            string sql = @"
INSERT INTO [Paycheck] ( [Money], [UnallocatedMoney], [Paydate] )
VALUES ( @money, @money, @paydate );
";
            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction))
            {
                command.Parameters.Add(new SqlParameter("@money", money));
                command.Parameters.Add(new SqlParameter("@paydate", paydate));
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeletePaycheckAsync(int id)
        {
            string sql = @"
DELETE FROM [Paycheck]
WHERE [Id] = @id;
";
            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction))
            {
                command.Parameters.Add(new SqlParameter("@id", id));
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<Paycheck> GetLastPaycheck()
        {
            string sql = @"
SELECT TOP 1 [Id], [Money], [UnallocatedMoney], [Paydate]
FROM [Paycheck]
ORDER BY [Id] DESC;
";
            _transactionHelper.SqlCommand = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction);
            _transactionHelper.SqlDataReader = await _transactionHelper.SqlCommand.ExecuteReaderAsync();
            return _transactionHelper.SqlDataReader.GetDtos<Paycheck>().FirstOrDefault();
        }

        public async Task<IEnumerable<Paycheck>> GetLastXPaychecksAsync(int numPaychecks)
        {
            string sql = @"
SELECT TOP (@numPaychecks) [Id], [Money], [UnallocatedMoney], [Paydate]
FROM [Paycheck]
ORDER BY [Paydate] DESC
";
            _transactionHelper.SqlCommand = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction);
            _transactionHelper.SqlCommand.Parameters.Add(new SqlParameter("@numPaychecks", numPaychecks));
            _transactionHelper.SqlDataReader = await _transactionHelper.SqlCommand.ExecuteReaderAsync();
            return _transactionHelper.SqlDataReader.GetDtos<Paycheck>();
        }

        public async Task<Paycheck> GetPaycheckAsync(int id)
        {
            string sql = @"
SELECT TOP 1 [Id], [Money], [UnallocatedMoney], [Paydate]
FROM [Paycheck]
WHERE [Id] = @id;
";
            _transactionHelper.SqlCommand = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction);
            _transactionHelper.SqlCommand.Parameters.Add(new SqlParameter("@id", id));
            _transactionHelper.SqlDataReader = await _transactionHelper.SqlCommand.ExecuteReaderAsync();
            return _transactionHelper.SqlDataReader.GetDtos<Paycheck>().FirstOrDefault();
        }

        public async Task<IEnumerable<Paycheck>> GetPaychecksBetweenAsync(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            string sql = @"
SELECT [Id], [Money], [UnallocatedMoney], [Paydate]
FROM [Paycheck]
WHERE [Paydate] >= @startDate
AND [Paydate] <= @endDate
ORDER BY [Paydate] DESC
";
            _transactionHelper.SqlCommand = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction);
            _transactionHelper.SqlCommand.Parameters.Add(new SqlParameter("@startDate", startDate));
            _transactionHelper.SqlCommand.Parameters.Add(new SqlParameter("@endDate", endDate));
            _transactionHelper.SqlDataReader = await _transactionHelper.SqlCommand.ExecuteReaderAsync();
            return _transactionHelper.SqlDataReader.GetDtos<Paycheck>();
        }

        public async Task UpdatePaycheckAmountAsync(int id, decimal money)
        {
            string sql = @"
UPDATE [Paycheck]
SET [Money] = @money
WHERE [Id] = @id;
";
            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction))
            {
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Parameters.Add(new SqlParameter("@money", money));
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdatePaycheckDateAsync(int id, DateTimeOffset paydate)
        {
            string sql = @"
UPDATE [Paycheck]
SET [Paydate] = @paydate
WHERE [Id] = @id;
";
            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction))
            {
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Parameters.Add(new SqlParameter("@paydate", paydate));
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdatePaycheckUnallocatedAsync(int id, decimal unallocatedMoney)
        {
            string sql = @"
UPDATE [Paycheck]
SET [UnallocatedMoney] = @unallocatedMoney
WHERE [Id] = @id;
";
            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction))
            {
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Parameters.Add(new SqlParameter("@unallocatedMoney", unallocatedMoney));
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
