namespace AccountService.Features.Users.Utils;

public interface IKeyCloakClient
{
    Task<List<User>?> GetAllUsers();
    Task<User?> FindUser(Guid id);
    Task<KeyCloakToken?> GetTokenFromCacheAsync();
}