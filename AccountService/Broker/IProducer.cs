using Broker.Entity;

namespace AccountService.Broker;

public interface IProducer<in T> where T:EventWrapper
{
    Task ProduceAsync(T param);
}