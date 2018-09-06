using Budget.model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Budget.service.i
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAccountsAsync();
    }
}
