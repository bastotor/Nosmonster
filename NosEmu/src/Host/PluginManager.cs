using System.Reflection;
using NosEmu.Core;

namespace NosEmu.Host;

public sealed class PluginManager : IAsyncDisposable
{
    private readonly string _pluginDirectory;
    private readonly PluginContext _context;
    private readonly Dictionary<string, PluginHandle> _handles = new(StringComparer.OrdinalIgnoreCase);
    private readonly FileSystemWatcher _watcher;
    private readonly SemaphoreSlim _reloadGate = new(1, 1);

    public PluginManager(string pluginDirectory, PluginContext context)
    {
        _pluginDirectory = pluginDirectory;
        _context = context;
        Directory.CreateDirectory(_pluginDirectory);

        _watcher = new FileSystemWatcher(_pluginDirectory, "*.dll")
        {
            IncludeSubdirectories = false,
            EnableRaisingEvents = true,
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
        };

        _watcher.Changed += OnPluginChanged;
        _watcher.Created += OnPluginChanged;
        _watcher.Renamed += OnPluginChanged;
    }

    public async Task LoadAllAsync(CancellationToken cancellationToken)
    {
        foreach (var file in Directory.GetFiles(_pluginDirectory, "*.dll"))
        {
            await LoadPluginAsync(file, cancellationToken).ConfigureAwait(false);
        }
    }

    private async void OnPluginChanged(object sender, FileSystemEventArgs e)
    {
        await ReloadAsync(e.FullPath).ConfigureAwait(false);
    }

    private async Task ReloadAsync(string path)
    {
        await _reloadGate.WaitAsync().ConfigureAwait(false);
        try
        {
            await ReloadPluginAsync(path, CancellationToken.None).ConfigureAwait(false);
        }
        finally
        {
            _reloadGate.Release();
        }
    }

    public async Task ReloadPluginAsync(string path, CancellationToken cancellationToken)
    {
        if (_handles.TryGetValue(path, out var existing))
        {
            await existing.Plugin.StopAsync(cancellationToken).ConfigureAwait(false);
            existing.Context.Unload();
            _handles.Remove(path);
        }

        await LoadPluginAsync(path, cancellationToken).ConfigureAwait(false);
    }

    private async Task LoadPluginAsync(string path, CancellationToken cancellationToken)
    {
        if (!File.Exists(path))
        {
            return;
        }

        var shadowDirectory = Path.Combine(_pluginDirectory, ".shadow");
        Directory.CreateDirectory(shadowDirectory);

        var shadowPath = Path.Combine(shadowDirectory, $"{Path.GetFileNameWithoutExtension(path)}-{DateTimeOffset.UtcNow:yyyyMMddHHmmssfff}.dll");
        File.Copy(path, shadowPath, overwrite: true);

        var loadContext = new PluginLoadContext(shadowPath);
        var assembly = loadContext.LoadFromAssemblyPath(shadowPath);
        var pluginType = assembly.GetTypes().FirstOrDefault(type => typeof(IPlugin).IsAssignableFrom(type) && !type.IsAbstract);

        if (pluginType is null)
        {
            loadContext.Unload();
            return;
        }

        if (Activator.CreateInstance(pluginType) is not IPlugin plugin)
        {
            loadContext.Unload();
            return;
        }

        await plugin.StartAsync(_context, cancellationToken).ConfigureAwait(false);
        _handles[path] = new PluginHandle(path, plugin, loadContext, assembly);
    }

    public async ValueTask DisposeAsync()
    {
        _watcher.EnableRaisingEvents = false;
        _watcher.Dispose();

        foreach (var handle in _handles.Values)
        {
            await handle.Plugin.StopAsync(CancellationToken.None).ConfigureAwait(false);
            handle.Context.Unload();
        }

        _handles.Clear();
        _reloadGate.Dispose();
    }

    private sealed record PluginHandle(string Path, IPlugin Plugin, PluginLoadContext Context, Assembly Assembly);
}
