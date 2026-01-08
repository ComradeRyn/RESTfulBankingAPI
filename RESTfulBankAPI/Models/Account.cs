namespace RESTfullBankAPI.Models;

public class Account
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string HolderName { get; init; }
    public decimal Balance { get; set; } = 0;
}