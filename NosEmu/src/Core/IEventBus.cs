namespace NosEmu.Core;

public interface IEventBus
{
    IDisposable Subscribe<TEvent>(Func<TEvent, CancellationToken, Task> handler);
    Task PublishAsync<TEvent>(TEvent evt, CancellationToken cancellationToken);
}
