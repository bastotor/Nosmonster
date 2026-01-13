namespace NosEmu.Core;

public sealed record PluginContext(IEventBus EventBus, IServiceProvider Services, string PluginDirectory);
