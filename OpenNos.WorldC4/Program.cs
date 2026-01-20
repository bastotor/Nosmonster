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
                ServerName = "World Server C4",
                RestartExecutable = "OpenNos.WorldC4.exe",
                PacketHandlerType = typeof(CommandPacketHandler),
                FallbackCryptographyType = typeof(LoginCryptography)
            };

            WorldServerBootstrap<WorldCryptography>.Run(args, options);
        }
    }
}
