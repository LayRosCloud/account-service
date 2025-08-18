using Broker.Entity;

namespace AccountService.Utils.Broker;

public static class MetaCreator
{
    public static Metadata Create(Guid correlationId, Guid causationId, string version = "v1")
    {
        return new Metadata(version, "account-service", correlationId, causationId);
    }

    public static readonly Guid AccountCreate = Guid.Parse("107429c2-edb6-4bef-81f8-64875afe1961");
}