using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTfullBankAPI.Models;
using RESTfullBankAPI.Models.Records;
using RESTfullBankAPI.Services;

namespace RESTfullBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AccountsService _services;

        public AccountsController(AccountsService services)
        {
            _services = services;
        }

        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount(CreationRequest request)
        {
            return null;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(Guid id)
        {
            return null;
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
