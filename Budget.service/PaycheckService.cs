using Budget.dal.i;
using Budget.model;
using Budget.service.i;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.service
{
    public class PaycheckService : IPaycheckService
    {
        private IRepositoryService _repositoryService;

        public PaycheckService(IRepositoryService repositoryService)
        {
            this._repositoryService = repositoryService;
        }

        public async Task<Paycheck> CreatePaycheckAsync(decimal money, DateTime paydate)
        {
            using (ITransactionHelper transactionHelper = this._repositoryService.GetTransactionHelper())
            {
                var paycheckRepository = this._repositoryService.GetPaycheckRepository(transactionHelper);
                await paycheckRepository.CreatePaycheckAsync(money, paydate);
                Paycheck paycheck = new Paycheck(await paycheckRepository.GetLastPaycheck());
                transactionHelper.CommitTransaction();
                return paycheck;
            }
        }

        public async Task DeletePaycheckAsync(int id)
        {
            using (ITransactionHelper transactionHelper = this._repositoryService.GetTransactionHelper())
            {
                var paycheckRepository = this._repositoryService.GetPaycheckRepository(transactionHelper);
                await paycheckRepository.DeletePaycheckAsync(id);
                transactionHelper.CommitTransaction();
            }
        }

        public async Task<IEnumerable<Paycheck>> GetLastXPaychecksAsync(int numPaychecks)
        {
            using (ITransactionHelper transactionHelper = this._repositoryService.GetTransactionHelper())
            {
                var paycheckRepository = this._repositoryService.GetPaycheckRepository(transactionHelper);
                return (await paycheckRepository.GetLastXPaychecksAsync(numPaychecks)).Select(p => new Paycheck(p)).ToList();
            }
        }

        public async Task<Paycheck> GetPaycheckAsync(int id)
        {
            using (ITransactionHelper transactionHelper = this._repositoryService.GetTransactionHelper())
            {
                var paycheckRepository = this._repositoryService.GetPaycheckRepository(transactionHelper);
                return new Paycheck(await paycheckRepository.GetPaycheckAsync(id));
            }
        }

        public async Task<IEnumerable<Paycheck>> GetPaychecksBetweenAsync(DateTime startDate, DateTime endDate)
        {
            using (ITransactionHelper transactionHelper = this._repositoryService.GetTransactionHelper())
            {
                var paycheckRepository = this._repositoryService.GetPaycheckRepository(transactionHelper);
                return (await paycheckRepository.GetPaychecksBetweenAsync(startDate, endDate)).Select(p => new Paycheck(p)).ToList();
            }
        }

        public async Task UpdatePaycheckAmountAsync(int id, decimal money)
        {
            using (ITransactionHelper transactionHelper = this._repositoryService.GetTransactionHelper())
            {
                var paycheckRepository = this._repositoryService.GetPaycheckRepository(transactionHelper);
                await paycheckRepository.UpdatePaycheckAmountAsync(id, money);
                transactionHelper.CommitTransaction();
            }
        }

        public async Task UpdatePaycheckDateAsync(int id, DateTime paydate)
        {
            using (ITransactionHelper transactionHelper = this._repositoryService.GetTransactionHelper())
            {
                var paycheckRepository = this._repositoryService.GetPaycheckRepository(transactionHelper);
                await paycheckRepository.UpdatePaycheckDateAsync(id, paydate);
                transactionHelper.CommitTransaction();
            }
        }

        public async Task UpdatePaycheckUnallocatedAsync(int id, decimal unallocatedMoney)
        {
            using (ITransactionHelper transactionHelper = this._repositoryService.GetTransactionHelper())
            {
                var paycheckRepository = this._repositoryService.GetPaycheckRepository(transactionHelper);
                await paycheckRepository.UpdatePaycheckUnallocatedAsync(id, unallocatedMoney);
                transactionHelper.CommitTransaction();
            }
        }
    }
}
