using RESTfullBankAPI.Models;
using RESTfullBankAPI.Models.Records;

namespace RESTfullBankAPI.Services;

public class AccountsService(AccountContext context)
{
    private readonly AccountContext _context = context;

    public Account GetAccount(Guid id)
    {
        return null;
    }

    public Account CreateAccount(CreationRequest request)
    {
        return null;
    }

    public decimal Deposit(Guid id, ChangeBalanceRequest request)
    {
        return 0;
    }

    public decimal Withdraw(Guid id, ChangeBalanceRequest request)
    {
        return 0;
    }

    public decimal Transfer(TransferRequest request)
    {
        return 0;
    }
}