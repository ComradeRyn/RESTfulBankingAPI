using System.ComponentModel.DataAnnotations;

namespace RESTfulBankAPI.Models;

public class Account
{
    
    public string Id { get; init; } = Guid.NewGuid().ToString();
    [MaxLength(50)]
    public required string HolderName{ get; init; }
    public decimal Balance { get; set; }
}