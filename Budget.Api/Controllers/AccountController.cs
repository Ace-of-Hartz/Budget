using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Budget.Api.Dependencies;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Account>))]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            return Ok(await RepositoryServiceProvider.AccountService.GetAccountsAsync());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Account))]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            return Ok(await RepositoryServiceProvider.AccountService.GetAccountAsync(id, 1000));
        }
    }
}