using System.Net.Http.Headers;
using Microsoft.Extensions.Caching.Memory;

namespace AccountService.Features.Users.Utils;

public class KeyCloakClient : IKeyCloakClient
{
    private readonly IMemoryCache _cache;
    private const string KeyToken = "access_token";
    private readonly string _endpointUserFind;
    private readonly string _endpointToken;
    private readonly string _clientSecret;
    private readonly string _clientId;
    private readonly string _grantType;

    public KeyCloakClient(IMemoryCache cache, IConfiguration configuration)
    {
        _cache = cache;
        var host = configuration["KeyCloak:Host"] ?? throw new InvalidOperationException("configuration host is empty");
        var realm = configuration["KeyCloak:Realm"] ?? throw new InvalidOperationException("configuration realm is empty");
        _clientSecret = configuration["KeyCloak:ClientSecret"] ?? throw new InvalidOperationException("configuration client secret is empty");
        _clientId = configuration["KeyCloak:ClientId"] ?? throw new InvalidOperationException("configuration client id is empty");
        _grantType = configuration["KeyCloak:GrantType"] ?? throw new InvalidOperationException("configuration grant type is empty");
        _endpointUserFind = $"{host}/admin/realms/{realm}/users";
        _endpointToken = $"{host}/realms/{realm}/protocol/openid-connect/token";
    }

    public async Task<List<User>?> GetAllUsers()
    {
        var token = await GetTokenAndValidate();
        using var client = GetHttpClient(token.TokenType, token.AccessToken);
        var users = await client.GetFromJsonAsync<List<User>>(_endpointUserFind);
        return users;
    }

    public async Task<User?> FindUser(Guid id)
    {
        var token = await GetTokenAndValidate();
        using var client = GetHttpClient(token.TokenType, token.AccessToken);
        var user = await client.GetFromJsonAsync<User>($"{_endpointUserFind}/{id}");
        return user;
    }

    private static HttpClient GetHttpClient(string scheme, string accessToken)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, accessToken);
        return client;
    }

    private async Task<KeyCloakToken> GetTokenAndValidate()
    {
        var token = await GetTokenFromCacheAsync();
        if (token == null)
            throw new InvalidOperationException("Authorization service is error requesting...");
        return token;
    }

    private async Task<KeyCloakToken?> GetTokenFromCacheAsync()
    {
        var token = await _cache.GetOrCreateAsync(KeyToken, async _ => await SendRequestToKeyCloak());
        var expiredDate = token!.CreatedAt.AddSeconds(token.ExpiresIn);

        // ReSharper disable once InvertIf The code looks less confusing.
        if (DateTime.UtcNow >= expiredDate)
        {
            token = await SendRequestToKeyCloak();
            _cache.Set(KeyToken, token);
        } 

        return token;
    }

    private async Task<KeyCloakToken?> SendRequestToKeyCloak()
    {
        var requestContent = new List<KeyValuePair<string, string>>
        {
            new("grant_type", _grantType),
            new("client_id", _clientId),
            new("client_secret", _clientSecret),
        };
        using var client = new HttpClient();
        using var req = new HttpRequestMessage(HttpMethod.Post, _endpointToken);
        req.Content = new FormUrlEncodedContent(requestContent);
        using var res = await client.SendAsync(req);
        var responseContent = await res.Content.ReadFromJsonAsync<KeyCloakToken>();
        responseContent!.CreatedAt = DateTime.UtcNow;
        return responseContent;
    }

}