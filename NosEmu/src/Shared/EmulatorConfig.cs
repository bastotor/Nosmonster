namespace NosEmu.Shared;

public sealed class EmulatorConfig
{
    public string PluginsDirectory { get; set; } = "plugins";
    public DatabaseConfig Database { get; set; } = new();
    public ScriptingConfig Scripting { get; set; } = new();
}

public sealed class DatabaseConfig
{
    public string ConnectionString { get; set; } = "Server=localhost;Database=NosEmu;Trusted_Connection=True;TrustServerCertificate=True";
}

public sealed class ScriptingConfig
{
    public string ScriptsDirectory { get; set; } = "scripts";
}
