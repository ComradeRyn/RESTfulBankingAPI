namespace RESTfullBankAPI.Models;

public class Account(string name)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string HolderName { get; init; } = name;
    public decimal Balance { get; set; } = 0;
}