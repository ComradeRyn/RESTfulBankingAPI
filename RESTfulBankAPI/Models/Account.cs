namespace RESTfullBankAPI.Models;

public class Account()
{
    public Guid Id { get; init; } = Guid.NewGuid();
    // Should I restrict the string length?
    public string HolderName { get; init; }
    public decimal Balance { get; set; } = 0;
}