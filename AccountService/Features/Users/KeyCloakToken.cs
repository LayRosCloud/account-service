using System.Text.Json.Serialization;

namespace AccountService.Features.Users;

public class KeyCloakToken
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;

    [JsonPropertyName("expires_in")]
    public long ExpiresIn { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}