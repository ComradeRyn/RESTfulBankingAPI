namespace RESTfullBankAPI.Models.Records;

public record ChangeBalanceRequest()
{
    public required decimal Amount { get; init; }
}