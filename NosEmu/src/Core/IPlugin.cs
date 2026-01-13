namespace NosEmu.Core;

public interface IPlugin
{
    string Name { get; }
    Version Version { get; }
    Task StartAsync(PluginContext context, CancellationToken cancellationToken);
    Task StopAsync(CancellationToken cancellationToken);
}
