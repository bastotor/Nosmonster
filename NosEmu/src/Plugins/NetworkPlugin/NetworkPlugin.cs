using NosEmu.Core;

namespace NosEmu.Plugin.Network;

public sealed class NetworkPlugin : IPlugin
{
    public string Name => "Network";
    public Version Version => new(0, 1, 0);

    public Task StartAsync(PluginContext context, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[{Name}] Network subsystem placeholder started.");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine($"[{Name}] Stopped.");
        return Task.CompletedTask;
    }
}
