using Microsoft.EntityFrameworkCore;

namespace RESTfullBankAPI.Models;

public class AccountContext(DbContextOptions<AccountContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; init; }
}