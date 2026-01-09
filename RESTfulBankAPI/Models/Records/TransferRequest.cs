namespace RESTfulBankAPI.Models.Records;

public record TransferRequest
{
    public decimal Amount { get; init; }
    public required string SenderId { get; init; }
    public required string ReceiverId { get; init; }
}