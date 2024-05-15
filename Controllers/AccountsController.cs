using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountApi.Models;

namespace AccountApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AccountContext _context;

        public AccountsController(AccountContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            return await _context.Accounts.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        [HttpGet("owner/{name}")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccountsByOwner(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Owner name cannot be empty.");
            }

            var accounts = await _context.Accounts
                                         .Where(a => a.OwnerName == name)
                                         .ToListAsync();

            if (accounts == null || accounts.Count == 0)
            {
                return NotFound();
            }

            return accounts;
        }

        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccount", new { id = account.Id }, account);
        }

        [HttpPost("transfer")]
        public async Task<ActionResult> TransferFunds([FromQuery] int fromAccountId, [FromQuery] int toAccountId, [FromQuery] float amount)
        {
            if (amount <= 0)
                return BadRequest("Invalid amount");

            var fromAccount = await _context.Accounts.FindAsync(fromAccountId);
            var toAccount = await _context.Accounts.FindAsync(toAccountId);

            if (fromAccount == null || toAccount == null)
                return NotFound("One or both accounts not found.");

            if (fromAccount.Balance < amount)
                return BadRequest("Insufficient funds in the source account.");

            fromAccount.Balance -= amount;
            toAccount.Balance += amount;

            var transaction = new Transaction
            {
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId,
                Amount = amount,
                TransactionDate = DateTime.Now
            };

            try
            {
                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error ocurred while processing the transaction.");
            }

            return Ok("Transaction successful.");
        }
    }
}
