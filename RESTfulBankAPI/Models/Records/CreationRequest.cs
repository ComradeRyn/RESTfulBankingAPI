namespace RESTfulBankAPI.Models.Records;

public record CreationRequest
{
    public required string Name { get; init; }
}