using Microsoft.AspNetCore.Mvc;
using RESTfulBankAPI.Models;
using RESTfulBankAPI.Models.Records;
using RESTfulBankAPI.Services;

namespace RESTfulBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController(AccountsService services) : ControllerBase
    {
        private readonly AccountsService _services = services;

        [HttpPost]
        [Produces("application/json")]
        public ActionResult<Account> PostAccount(CreationRequest request)
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

        // Should I include the parameter constraint?
        [HttpGet("{id}")]
        [Produces("application/json")]
        public ActionResult<Account> GetAccount(Guid id)
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
        [Produces("application/json")]
        public IActionResult PostDeposit(Guid id, ChangeBalanceRequest request)
        {
            try
            {
                // What type of success should I return?
                return Ok($"new balance of {_services.Deposit(id, request)}");
            }
            
            // Question: how could I split up the exceptions
            // Currently two: Not found and bad request
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("{id}/withdraws")]
        [Produces("application/json")]
        public IActionResult PostWithdraw(Guid id, ChangeBalanceRequest request)
        {
            try
            {
                return Ok($"new balance of {_services.Withdraw(id, request)}");
            }

            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("transfers")]
        [Produces("application/json")]
        public IActionResult PostTransfer(TransferRequest request)
        {
            try
            {
                return Ok($"new balance of {_services.Transfer(request)}");
            }

            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}