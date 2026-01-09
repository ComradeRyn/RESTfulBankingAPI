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

    public Account GetAccount(string id)
    {
        var queriedAccount = _context.Accounts.Find(id);

        if (queriedAccount == null)
        {
            throw new AccountNotFoundException("No user found with given id", nameof(id));
        }
        
        return queriedAccount;
    }

    public Account CreateAccount(CreationRequest request)
    {
        if (!ValidateName(request.Name))
        {
            throw new ArgumentException("Name must be at most 50 characters and " +
                                        "in the format of <first name> <second name> <...> <last name>", 
                nameof(request));
        }

        var newAccount = new Account(){HolderName = request.Name};
        _context.Accounts.Add(newAccount);
        _context.SaveChanges();
        
        return newAccount;
    }

    public decimal Deposit(string id, ChangeBalanceRequest request)
    {
        var queriedAccount = GetAccount(id);
        PreformDeposit(queriedAccount, request.Amount);
        
        return queriedAccount.Balance;
    }

    public decimal Withdraw(string id, ChangeBalanceRequest request)
    {
        var queriedAccount = GetAccount(id);
        PreformWithdraw(queriedAccount, request.Amount);
        
        return queriedAccount.Balance;
    }

    public decimal Transfer(TransferRequest request)
    {
        var receiver = GetAccount(request.ReceiverId);
        var sender = GetAccount(request.SenderId);

        PreformWithdraw(sender, request.Amount);
        PreformDeposit(receiver, request.Amount);

        return receiver.Balance;
    }
    
    private bool ValidateName(string accountName)
    {
        var nameTokens = accountName.Split(" ");
        
        return accountName.Length <= 50 && nameTokens.All(name => name.All(char.IsLetter) && name != "");
    }

    private void PreformDeposit(Account account, decimal amount)
    {
        if (amount <= 0)
        {
            throw new NegativeAmountException("Requested amount must be positive", nameof(amount));
        }
        
        account.Balance += amount;
        _context.SaveChanges();
    }

    private void PreformWithdraw(Account account, decimal amount)
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
        _context.SaveChanges();
    }
}