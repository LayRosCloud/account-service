namespace Broker.Entity;

public class Metadata
{
    public Metadata(string version, string source, Guid correlationId, Guid causationId)
    {
        Version = version;
        Source = source;
        CorrelationId = correlationId;
        CausationId = causationId;
    }

    public string Version { get; }
    public string Source { get; }
    public Guid CorrelationId { get; }
    public Guid CausationId { get; }
}