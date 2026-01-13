using Microsoft.Extensions.DependencyInjection;
using NosEmu.Core;
using NosEmu.Shared;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace NosEmu.Host;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var configPath = args.Length > 0 ? args[0] : Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "config", "appsettings.yaml");
        var config = LoadConfig(configPath);

        var services = new ServiceCollection();
        services.AddSingleton(config);
        services.AddSingleton(config.Database);
        services.AddSingleton(config.Scripting);
        services.AddSingleton<IEventBus, EventBus>();
        services.AddSingleton(RuntimeInfo.ProductName);

        using var provider = services.BuildServiceProvider();
        var eventBus = provider.GetRequiredService<IEventBus>();

        var pluginDirectory = Path.GetFullPath(config.PluginsDirectory, AppContext.BaseDirectory);
        var pluginContext = new PluginContext(eventBus, provider, pluginDirectory);
        await using var manager = new PluginManager(pluginDirectory, pluginContext);

        using var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, eventArgs) =>
        {
            eventArgs.Cancel = true;
            cts.Cancel();
        };

        Console.WriteLine($"{RuntimeInfo.ProductName} starting with plugins from {pluginDirectory}...");
        await manager.LoadAllAsync(cts.Token).ConfigureAwait(false);

        Console.WriteLine("Press Ctrl+C to stop.");
        try
        {
            await Task.Delay(Timeout.Infinite, cts.Token).ConfigureAwait(false);
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("Shutdown requested.");
        }
    }

    private static EmulatorConfig LoadConfig(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Configuration file not found: {path}");
        }

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();

        var yaml = File.ReadAllText(path);
        return deserializer.Deserialize<EmulatorConfig>(yaml) ?? new EmulatorConfig();
    }
}
