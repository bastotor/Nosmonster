using Microsoft.EntityFrameworkCore;

namespace NosEmu.Plugin.Database;

public sealed class NosEmuDbContext : DbContext
{
    public NosEmuDbContext(DbContextOptions<NosEmuDbContext> options) : base(options)
    {
    }
}
