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

        if (queriedAccount is null)
        {
            throw new AccountNotFoundException("No user found with given id", nameof(id));
        }
        
        return queriedAccount;
    }

    public async Task<Account> CreateAccount(CreationRequest request)
    {
        ValidateName(request.Name);

        var newAccount = new Account(){HolderName = request.Name};
        
        await _context.Accounts.AddAsync(newAccount);
        await _context.SaveChangesAsync();
        
        return newAccount;
    }

    public async Task<decimal> Deposit(string id, ChangeBalanceRequest request)
    {
        var queriedAccount = await GetAccount(id);
        
        ValidatePositiveAmount(request.Amount);
        
        queriedAccount.Balance += request.Amount;
        
        await _context.SaveChangesAsync();
        
        return queriedAccount.Balance;
    }

    public async Task<decimal> Withdraw(string id, ChangeBalanceRequest request)
    {
        var queriedAccount = await GetAccount(id);
        
        ValidatePositiveAmount(request.Amount);
        ValidateSufficientBalance(queriedAccount.Balance, request.Amount);
        
        queriedAccount.Balance -= request.Amount;
        
        await _context.SaveChangesAsync();
        
        return queriedAccount.Balance;
    }
    
    public async Task<decimal> Transfer(TransferRequest request)
    {
        var receiver = await GetAccount(request.ReceiverId);
        var sender = await GetAccount(request.SenderId);

        ValidatePositiveAmount(request.Amount);
        ValidateSufficientBalance(sender.Balance, request.Amount);

        sender.Balance -= request.Amount;
        receiver.Balance += request.Amount;
        
        await _context.SaveChangesAsync();

        return receiver.Balance;
    }
    
    private void ValidateName(string accountName)
    {
        var nameRegex = new Regex(@"^([^\d\s]+\s?)+$");

        if (!nameRegex.IsMatch(accountName))
        {
            throw new ArgumentException("Name must be in the format of <first name> <second name> <...> <last name>", 
                nameof(accountName));
        }
    }

    private void ValidatePositiveAmount(decimal amount)
    {
        if (amount <= 0)
        {
            throw new NegativeAmountException("Requested amount must be positive", nameof(amount));
        }
    }

    private void ValidateSufficientBalance(decimal balance, decimal amount)
    {
        if (amount > balance)
        {
            throw new InsufficientFundsException("Requested amount must be less than or equal to current balance",
                nameof(amount));
        }
    }
}