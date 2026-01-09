namespace RESTfulBankAPI.Models.Records;

public record TransferRequest()
{
    public decimal Amount { get; init; }
    public Guid SenderId { get; init; }
    public Guid ReceiverId { get; init; }
}