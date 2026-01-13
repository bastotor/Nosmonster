using MoonSharp.Interpreter;
using NosEmu.Core;
using NosEmu.Shared;

namespace NosEmu.Plugin.Scripting;

public sealed class ScriptingPlugin : IPlugin
{
    private Script? _scriptRuntime;
    private string _scriptsDirectory = string.Empty;

    public string Name => "Scripting";
    public Version Version => new(0, 1, 0);

    public Task StartAsync(PluginContext context, CancellationToken cancellationToken)
    {
        var config = context.Services.GetService(typeof(ScriptingConfig)) as ScriptingConfig;
        if (config is null)
        {
            throw new InvalidOperationException("ScriptingConfig is not registered.");
        }

        _scriptsDirectory = Path.GetFullPath(config.ScriptsDirectory, AppContext.BaseDirectory);
        Directory.CreateDirectory(_scriptsDirectory);

        _scriptRuntime = new Script();
        _scriptRuntime.Globals["log"] = (Action<string>)(message => Console.WriteLine($"[Lua] {message}"));

        Console.WriteLine($"[{Name}] Lua runtime ready. Scripts directory: {_scriptsDirectory}");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine($"[{Name}] Stopped.");
        _scriptRuntime = null;
        return Task.CompletedTask;
    }

    public DynValue Execute(string scriptName)
    {
        if (_scriptRuntime is null)
        {
            throw new InvalidOperationException("Scripting plugin not started.");
        }

        var scriptPath = Path.Combine(_scriptsDirectory, scriptName);
        if (!File.Exists(scriptPath))
        {
            throw new FileNotFoundException("Script not found.", scriptPath);
        }

        var script = File.ReadAllText(scriptPath);
        return _scriptRuntime.DoString(script);
    }
}
