using Microsoft.AspNetCore.Mvc;
using RESTfullBankAPI.Models;
using RESTfullBankAPI.Models.Records;
using RESTfullBankAPI.Services;

namespace RESTfullBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController(AccountsService services) : ControllerBase
    {
        private readonly AccountsService _services = services;

        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount(CreationRequest request)
        {
            try
            {
                var createdAccount = _services.CreateAccount(request);
                
                return CreatedAtAction(nameof(GetAccount), new{id = createdAccount.Id}, createdAccount);
            }
            
            catch(ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(Guid id)
        {
            try
            {
                return _services.GetAccount(id);
            }

            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost("{id}/deposits")]
        public async Task<IActionResult> PostDeposit(Guid id, ChangeBalanceRequest request)
        {
            return null;
        }

        [HttpPost("{id}/withdraws")]
        public async Task<IActionResult> PostWithdraw(Guid id, ChangeBalanceRequest request)
        {
            return null;
        }

        [HttpPost("transfers")]
        public async Task<IActionResult> PostTransfer(TransferRequest request)
        {
            return null;
        }
    }
}
