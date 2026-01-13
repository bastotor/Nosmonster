using System.Collections.Concurrent;

namespace NosEmu.Core;

public sealed class EventBus : IEventBus
{
    private readonly ConcurrentDictionary<Type, List<Func<object, CancellationToken, Task>>> _handlers = new();

    public IDisposable Subscribe<TEvent>(Func<TEvent, CancellationToken, Task> handler)
    {
        var handlers = _handlers.GetOrAdd(typeof(TEvent), _ => new List<Func<object, CancellationToken, Task>>());
        var wrapper = new Func<object, CancellationToken, Task>((evt, token) => handler((TEvent)evt, token));

        lock (handlers)
        {
            handlers.Add(wrapper);
        }

        return new Subscription(() =>
        {
            lock (handlers)
            {
                handlers.Remove(wrapper);
            }
        });
    }

    public async Task PublishAsync<TEvent>(TEvent evt, CancellationToken cancellationToken)
    {
        if (!_handlers.TryGetValue(typeof(TEvent), out var handlers))
        {
            return;
        }

        List<Func<object, CancellationToken, Task>> snapshot;
        lock (handlers)
        {
            snapshot = handlers.ToList();
        }

        foreach (var handler in snapshot)
        {
            await handler(evt!, cancellationToken).ConfigureAwait(false);
        }
    }

    private sealed class Subscription : IDisposable
    {
        private readonly Action _dispose;
        private int _disposed;

        public Subscription(Action dispose)
        {
            _dispose = dispose;
        }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposed, 1) == 1)
            {
                return;
            }

            _dispose();
        }
    }
}
