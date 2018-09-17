using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Budget.Api.Dependencies;
using Budget.Api.Models.AccountModels;
using Budget.Api.Models.PaycheckModels;
using Budget.model;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/paychecks")]
    public class PaycheckController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Paycheck>>> GetPaychecksAsync(int id)
        {
            DateTime dateTime = new DateTime(DateTime.Now.Year, 1, 1);
            return Ok(await RepositoryServiceProvider.PaycheckService.GetPaychecksBetweenAsync(dateTime, DateTime.Now));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Paycheck>> GetPaycheckAsync(int id)
        {
            return Ok(await RepositoryServiceProvider.PaycheckService.GetPaycheckAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<Paycheck>> CreatePaycheckAsync([FromBody] PaycheckRequest paycheckRequest)
        {
            return Ok(await RepositoryServiceProvider.PaycheckService.CreatePaycheckAsync(paycheckRequest.Money, paycheckRequest.PayDate));
        }

        [HttpPut("{id}/paydate")]
        public async Task<ActionResult> UpdatePaycheckPaydateAsync(int id, [FromBody]PaycheckRequest paycheckRequest)
        {
            await RepositoryServiceProvider.PaycheckService.UpdatePaycheckDateAsync(id, paycheckRequest.PayDate);
            return Ok();
        }

        [HttpPut("{id}/money")]
        public async Task<ActionResult> UpdatePaycheckAmountAsync(int id, [FromBody]PaycheckRequest paycheckRequest)
        {
            await RepositoryServiceProvider.PaycheckService.UpdatePaycheckAmountAsync(id, paycheckRequest.Money);
            return Ok();
        }

        [HttpPut("{id}/unallocated")]
        public async Task<ActionResult> UpdatePaycheckUnallocatedAsync(int id, [FromBody]PaycheckRequest paycheckRequest)
        {
            await RepositoryServiceProvider.PaycheckService.UpdatePaycheckUnallocatedAsync(id, paycheckRequest.Money);
            return Ok();
        }
    }
}