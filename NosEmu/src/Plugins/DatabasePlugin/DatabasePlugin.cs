using Microsoft.EntityFrameworkCore;
using NosEmu.Core;
using NosEmu.Shared;

namespace NosEmu.Plugin.Database;

public sealed class DatabasePlugin : IPlugin
{
    private DbContextOptions<NosEmuDbContext>? _options;

    public string Name => "Database";
    public Version Version => new(0, 1, 0);

    public Task StartAsync(PluginContext context, CancellationToken cancellationToken)
    {
        var config = context.Services.GetService(typeof(DatabaseConfig)) as DatabaseConfig;
        if (config is null)
        {
            throw new InvalidOperationException("DatabaseConfig is not registered.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<NosEmuDbContext>();
        optionsBuilder.UseSqlServer(config.ConnectionString);
        _options = optionsBuilder.Options;

        Console.WriteLine($"[{Name}] Configured SQL Server connection.");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine($"[{Name}] Stopped.");
        _options = null;
        return Task.CompletedTask;
    }

    public NosEmuDbContext CreateDbContext()
    {
        if (_options is null)
        {
            throw new InvalidOperationException("Database plugin not started.");
        }

        return new NosEmuDbContext(_options);
    }
}
