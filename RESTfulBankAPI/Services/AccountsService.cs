using System.Text.RegularExpressions;
using RESTfulBankAPI.Models;
using RESTfulBankAPI.Models.Records;
using RESTfulBankAPI.Exceptions;

namespace RESTfulBankAPI.Services;

public class AccountsService
{
    private readonly AccountContext _context;

    public AccountsService(AccountContext context)
    {
        _context = context;
    }

    public async Task<Account> GetAccount(string id)
    {
        var queriedAccount = await _context.Accounts.FindAsync(id);

        if (queriedAccount == null)
        {
            throw new AccountNotFoundException("No user found with given id", nameof(id));
        }
        
        return queriedAccount;
    }

    public async Task<Account> CreateAccount(CreationRequest request)
    {
        if (!ValidateName(request.Name))
        {
            throw new ArgumentException("in the format of <first name> <second name> <...> <last name>", 
                                        nameof(request));
        }

        var newAccount = new Account(){HolderName = request.Name};
        
        await _context.Accounts.AddAsync(newAccount);
        await _context.SaveChangesAsync();
        
        return newAccount;
    }

    public async Task<decimal> Deposit(string id, ChangeBalanceRequest request)
    {
        var queriedAccount = await GetAccount(id);
        
        await PreformDeposit(queriedAccount, request.Amount);
        
        return queriedAccount.Balance;
    }

    public async Task<decimal> Withdraw(string id, ChangeBalanceRequest request)
    {
        var queriedAccount = await GetAccount(id);
        
        await PreformWithdraw(queriedAccount, request.Amount);
        
        return queriedAccount.Balance;
    }

    public async Task<decimal> Transfer(TransferRequest request)
    {
        var receiver = await GetAccount(request.ReceiverId);
        var sender = await GetAccount(request.SenderId);

        await PreformWithdraw(sender, request.Amount);
        await PreformDeposit(receiver, request.Amount);

        return receiver.Balance;
    }
    
    private bool ValidateName(string accountName)
    {
        var nameRegex = new Regex(@"^([^\d\s]+\s?)+$");
        
        return nameRegex.IsMatch(accountName);
    }

    private async Task PreformDeposit(Account account, decimal amount)
    {
        if (amount <= 0)
        {
            throw new NegativeAmountException("Requested amount must be positive", nameof(amount));
        }
        
        account.Balance += amount;
        
        await _context.SaveChangesAsync();
    }

    private async Task PreformWithdraw(Account account, decimal amount)
    {
        if (amount <= 0)
        {
            throw new NegativeAmountException("Requested amount must be positive", nameof(amount));
        }

        if (amount > account.Balance)
        {
            throw new InsufficientFundsException("Requested amount must be less than or equal to current balance",
                nameof(amount));
        }

        account.Balance -= amount;
        
        await _context.SaveChangesAsync();
    }
}