using NosEmu.Core;

namespace NosEmu.Plugin.GameLogic;

public sealed class GameLogicPlugin : IPlugin
{
    public string Name => "GameLogic";
    public Version Version => new(0, 1, 0);

    public Task StartAsync(PluginContext context, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[{Name}] Game logic placeholder started.");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine($"[{Name}] Stopped.");
        return Task.CompletedTask;
    }
}
