using Microsoft.EntityFrameworkCore;

namespace RESTfulBankAPI.Models;

public class AccountContext(DbContextOptions<AccountContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; init; }
}