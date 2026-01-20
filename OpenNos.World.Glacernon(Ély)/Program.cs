using OpenNos.Core;
using OpenNos.GameObject.Bootstrapping;
using OpenNos.Handler;

namespace OpenNos.World
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            WorldServerOptions options = new WorldServerOptions
            {
                ServerName = "Glacernon World Server (Ely)",
                RestartExecutable = "OpenNos.World.Glacernon.exe",
                PacketHandlerType = typeof(CommandPacketHandler),
                FallbackCryptographyType = typeof(LoginCryptography)
            };

            WorldServerBootstrap<WorldCryptography>.Run(args, options);
        }
    }
}
