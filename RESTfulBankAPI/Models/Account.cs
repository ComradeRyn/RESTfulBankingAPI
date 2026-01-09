namespace RESTfulBankAPI.Models;

public class Account
{
    public Guid Id { get; init; } = Guid.NewGuid();
    // Should I restrict the string length?
    public required string HolderName{ get; init; }
    public decimal Balance { get; set; }
}