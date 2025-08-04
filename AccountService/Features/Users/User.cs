namespace AccountService.Features.Users;

public class User
{
    public Guid Id { get; set; }
    // ReSharper disable once UnusedMember.Global model KeyCloak for just for show
    public string Username { get; set; } = string.Empty;

    // ReSharper disable once UnusedMember.Global model KeyCloak for just for show
    public string FirstName { get; set; } = string.Empty;

    // ReSharper disable once UnusedMember.Global model KeyCloak for just for show
    public string LastName { get; set; } = string.Empty;

    // ReSharper disable once UnusedMember.Global model KeyCloak for just for show
    public string Email { get; set; } = string.Empty;

    // ReSharper disable once UnusedMember.Global model KeyCloak for just for show
    public bool EmailVerified { get; set; }

    // ReSharper disable once UnusedMember.Global model KeyCloak for just for show
    public bool Enabled { get; set; }

    // ReSharper disable once UnusedMember.Global model KeyCloak for just for show
    public long CreatedTimestamp { get; set; }
}