# NosEmu (.NET 8)

This directory contains a clean, modular emulator foundation designed for NosTale.

## Highlights
- Core host with hot-reloadable plugin loader.
- YAML configuration.
- SQL Server-ready database plugin (EF Core).
- Lua scripting plugin (MoonSharp).

## Running
1. Build the solution with .NET 8.
2. Copy plugin DLLs into the `plugins` folder (relative to the host executable).
3. Adjust `config/appsettings.yaml` as needed.

## Plugins
The host loads every DLL in the `plugins` directory, then watches for changes to reload them without restarting.

## Structure
```
NosEmu/
  config/
    appsettings.yaml
  src/
    Core/
    Host/
    Shared/
    Plugins/
```
