namespace RESTfulBankAPI.Models;

public class Account
{
    
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public required string HolderName{ get; init; }
    public decimal Balance { get; set; }
}