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
            try
            {
                var message = $"new balance of {_services.Deposit(id, request)}";
                
                // What type of success should I return?
                return Ok(message);
            }
            
            // Question: how could I split up the exceptions
            // Currently two: Not found and bad request
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("{id}/withdraws")]
        public async Task<IActionResult> PostWithdraw(Guid id, ChangeBalanceRequest request)
        {
            try
            {
                var message = $"new balance of {_services.Withdraw(id, request)}";

                return Ok(message);
            }

            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("transfers")]
        public async Task<IActionResult> PostTransfer(TransferRequest request)
        {
            try
            {
                var message = $"new balance of {_services.Transfer(request)}";

                return Ok(message);
            }

            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
