namespace AccountService.Features.Accounts.Dto;

public class AccountResponseFullDto
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public AccountType Type { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public decimal? Percent { get; set; }
    public long CreatedAt { get; set; }
    public long? ClosedAt { get; set; }
}