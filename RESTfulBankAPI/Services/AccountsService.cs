using RESTfullBankAPI.Models;
using RESTfullBankAPI.Models.Records;

namespace RESTfullBankAPI.Services;

public class AccountsService(AccountContext context)
{
    /* Db Notes:
     * _context.Accounts.Add(item) adds and item
     * await _context.SaveChangesAsync() saves the changes
     * await _context.Accounts.FindAsync(id) finds the Account with an id
     */
    
    // Should I follow what rider is suggesting?
    private readonly AccountContext _context = context;

    public Account GetAccount(Guid id)
    {
        var queriedAccount = _context.Accounts.Find(id);

        if (queriedAccount == null)
        {
            throw new ArgumentException("No user found with given id", nameof(id));
        }
        
        return queriedAccount;
    }

    public Account CreateAccount(CreationRequest request)
    {
        if (!ValidateName(request.Name))
        {
            throw new ArgumentException("Name must be in the format of <first name> <second name> <...> <last name>", 
                nameof(request));
        }

        var newAccount = new Account(){HolderName = request.Name};
        _context.Accounts.Add(newAccount);
        _context.SaveChanges();
        
        return newAccount;
    }

    public decimal Deposit(Guid id, ChangeBalanceRequest request)
    {
        var queriedAccount = GetAccount(id);
        Deposit(queriedAccount, request.Amount);
        
        return queriedAccount.Balance;
    }

    public decimal Withdraw(Guid id, ChangeBalanceRequest request)
    {
        var queriedAccount = GetAccount(id);
        Withdraw(queriedAccount, request.Amount);
        
        return queriedAccount.Balance;
    }

    public decimal Transfer(TransferRequest request)
    {
        var receiver = GetAccount(request.ReceiverId);
        var sender = GetAccount(request.SenderId);

        Withdraw(receiver, request.Amount);
        Deposit(sender, request.Amount);

        return receiver.Balance;
    }
    
    private bool ValidateName(string accountName)
    {
        var nameTokens = accountName.Split(" ");
        
        return nameTokens.All(name => name.All(char.IsLetter) && name != "");
    }

    private void Deposit(Account account, decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Requested amount must be positive", nameof(amount));
        }
        
        account.Balance += amount;
        _context.SaveChanges();
    }

    private void Withdraw(Account account, decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Requested amount must be positive", nameof(amount));
        }

        if (amount > account.Balance)
        {
            throw new ArgumentException("Requested amount must be less than or equal to current balance", 
                nameof(amount));
        }

        account.Balance -= amount;
        _context.SaveChanges();
    }
}