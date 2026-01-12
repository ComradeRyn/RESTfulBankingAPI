using Microsoft.AspNetCore.Mvc;
using RESTfulBankAPI.Models;
using RESTfulBankAPI.Models.Records;
using RESTfulBankAPI.Services;
using RESTfulBankAPI.Exceptions;

namespace RESTfulBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AccountsService _service;

        public AccountsController(AccountsService service)
        {
            _service = service;
        }

        /// <summary>
        /// Creates a new Account with the provided name given the name follows the required naming convention
        /// </summary>
        /// <param name="request">A record which contains a string Name for the new account</param>
        /// <returns>The information of the generated account</returns>
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<Account>> PostAccount(CreationRequest request)
        {
            try
            {
                var createdAccount = await _service.CreateAccount(request);
                
                return CreatedAtAction(nameof(GetAccount), new{id = createdAccount.Id}, createdAccount);
            }
            catch(ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
        
        /// <summary>
        /// Retrieves an account based off a given ID
        /// </summary>
        /// <param name="id">The unique identification for the requested account</param>
        /// <returns>The account information corresponding to the id</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<Account>> GetAccount(string id)
        {
            try
            {
                return Ok(await _service.GetAccount(id));
            }
            catch (AccountNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
        
        /// <summary>
        /// Adds an entered money amount to a requested account
        /// </summary>
        /// <param name="id">The unique identification for the requested account</param>
        /// <param name="request">A record which contains a decimal Amount that will be deposited</param>
        /// <returns>A message stating the new account balance</returns>
        [HttpPost("{id}/deposits")]
        [Produces("application/json")]
        public async Task<IActionResult> PostDeposit(string id, ChangeBalanceRequest request)
        {
            try
            {
                return Ok($"new balance of ${await _service.Deposit(id, request)}");
            }
            catch (AccountNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (NegativeAmountException e)
            {
                return BadRequest(e.Message);
            }
        }
        
        /// <summary>
        /// Subtracts an entered money amount to a requested account
        /// </summary>
        /// <param name="id">The unique identification for the requested account</param>
        /// <param name="request">A record which contains a decimal Amount that will be withdrawn</param>
        /// <returns>A message stating the new account balance</returns>
        [HttpPost("{id}/withdraws")]
        [Produces("application/json")]
        public async Task<IActionResult> PostWithdraw(string id, ChangeBalanceRequest request)
        {
            try
            {
                return Ok($"new balance of ${await _service.Withdraw(id, request)}");
            }
            catch (AccountNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e) when (e is InsufficientFundsException or NegativeAmountException)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Takes money from one account and moves it to another
        /// </summary>
        /// <param name="request">A record that contains an id for the sending account, an id for the receiving
        /// account, along with the decimal amount that will be transferred</param>
        /// <returns>A message saying the receiver's new account balance</returns>
        [HttpPost("transfers")]
        [Produces("application/json")]
        public async Task<IActionResult> PostTransfer(TransferRequest request)
        {
            try
            {
                return Ok($"new receiver balance of ${await _service.Transfer(request)}");
            }
            catch (AccountNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e) when (e is InsufficientFundsException or NegativeAmountException)
            {
                return BadRequest(e.Message);
            }
        }
    }
}