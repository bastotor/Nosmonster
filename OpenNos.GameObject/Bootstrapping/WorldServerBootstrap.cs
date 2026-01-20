using NosTale.Packets.Packets.ClientPackets;
using OpenNos.Core;
using OpenNos.Core.Bootstrapping;
using OpenNos.DAL.EF.Helpers;
using OpenNos.Data;
using OpenNos.GameObject.Helpers;
using OpenNos.GameObject.Networking;
using OpenNos.Master.Library.Client;
using OpenNos.Master.Library.Data;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace OpenNos.GameObject.Bootstrapping
{
    public sealed class WorldServerOptions
    {
        public string ServerName { get; set; } = "World Server";

        public string Language { get; set; } = "EN";

        public string PortSettingKey { get; set; } = "WorldPort";

        public string AuthKeySettingKey { get; set; } = "MasterAuthKey";

        public string IpSettingKey { get; set; } = "IPAddress";

        public string PublicIpSettingKey { get; set; } = "PublicIP";

        public string ServerGroupSettingKey { get; set; } = "ServerGroup";

        public string AutoRebootSettingKey { get; set; } = "AutoReboot";

        public string RestartExecutable { get; set; } = "OpenNos.World.exe";

        public Type PacketHandlerType { get; set; }

        public Type FallbackCryptographyType { get; set; }

        public bool IsWorldServer { get; set; } = true;
    }

    public sealed class WorldServerBootstrap<TEncryptor> where TEncryptor : CryptographyBase
    {
        private const int SessionLimit = 100;
        private const string CrashLogPath = "C:\\WORLD_CRASHLOG.txt";

        private readonly WorldServerOptions _options;
        private EventHandler _exitHandler;
        private bool _ignoreTelemetry;
        private int _port;

        public WorldServerBootstrap(WorldServerOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public static void Run(string[] args, WorldServerOptions options)
        {
            new WorldServerBootstrap<TEncryptor>(options).Run(args);
        }

        private void Run(string[] args)
        {
#if DEBUG
            Thread.Sleep(1000);
#endif
            ConsoleStartup.InitializeCulture("en-US");

            ConsoleStartupArguments startupArgs = ConsoleStartup.ParseArguments(args);
            bool ignoreStartupMessages = startupArgs.IgnoreStartupMessages;
            _port = GetConfiguredPort();
            if (startupArgs.PortOverride.HasValue)
            {
                _port = startupArgs.PortOverride.Value;
                Console.WriteLine("Port override: " + _port);
            }
            _ignoreTelemetry = startupArgs.IgnoreTelemetry;

            SetConsoleTitle();
            InitializeLogger();
            if (!ignoreStartupMessages)
            {
                WriteStartupBanner();
            }

            AuthenticateMasterServer();
            InitializeDataAccess();
            PacketFactory.Initialize<WalkPacket>();

            bool autoreboot = ShouldAutoReboot();
            RegisterShutdownHandlers(autoreboot);

            StartNetwork();
            RegisterWorldServer();
        }

        private int GetConfiguredPort() => Convert.ToInt32(ConfigurationManager.AppSettings[_options.PortSettingKey]);

        private void SetConsoleTitle()
        {
            Console.Title = $"NosTale NosMonsterV3 - {_options.ServerName} [Port: {_port} - Language: {_options.Language}]";
        }

        private void InitializeLogger()
        {
            ConsoleStartup.InitializeLogger(typeof(WorldServerBootstrap<TEncryptor>));
        }

        private void WriteStartupBanner()
        {
            ConsoleStartup.WriteBanner("- NosMonsterV3 - ");
        }

        private void AuthenticateMasterServer()
        {
            string authKey = ConfigurationManager.AppSettings[_options.AuthKeySettingKey];
            if (CommunicationServiceClient.Instance.Authenticate(authKey))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Authentication] The Master Server communication has been established");
            }
        }

        private void InitializeDataAccess()
        {
            if (DataAccessHelper.Initialize())
            {
                ServerManager.Instance.Initialize();
                return;
            }

            Console.ReadKey();
            throw new InvalidOperationException("Failed to initialize data access.");
        }

        private bool ShouldAutoReboot()
        {
            if (bool.TryParse(ConfigurationManager.AppSettings[_options.AutoRebootSettingKey], out bool autoreboot))
            {
                autoreboot = false;
            }

            return autoreboot;
        }

        private void RegisterShutdownHandlers(bool autoreboot)
        {
            try
            {
                _exitHandler += ExitHandler;
                if (autoreboot)
                {
                    AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
                }

                NativeMethods.SetConsoleCtrlHandler(_exitHandler, true);
            }
            catch (Exception ex)
            {
                Logger.Error("General Error", ex);
            }
        }

        private void StartNetwork()
        {
            string ipAddress = ConfigurationManager.AppSettings[_options.IpSettingKey];

        portloop:
            try
            {
                if (_options.PacketHandlerType == null)
                {
                    throw new InvalidOperationException("PacketHandlerType is not configured.");
                }

                NetworkManager<TEncryptor> networkManager = new NetworkManager<TEncryptor>(
                    ipAddress,
                    _port,
                    _options.PacketHandlerType,
                    _options.FallbackCryptographyType,
                    _options.IsWorldServer);
            }
            catch (SocketException ex)
            {
                if (ex.ErrorCode == 10048)
                {
                    _port++;
                    Logger.Info("Port already in use! Incrementing...");
                    goto portloop;
                }

                Logger.Error("General Error", ex);
                Environment.Exit(ex.ErrorCode);
            }
        }

        private void RegisterWorldServer()
        {
            string authKey = ConfigurationManager.AppSettings[_options.AuthKeySettingKey];
            string publicIp = ConfigurationManager.AppSettings[_options.PublicIpSettingKey];

            ServerManager.Instance.ServerGroup = ConfigurationManager.AppSettings[_options.ServerGroupSettingKey];

            int? newChannelId = CommunicationServiceClient.Instance.RegisterWorldServer(
                new SerializableWorldServer(ServerManager.Instance.WorldId, publicIp, _port, SessionLimit, ServerManager.Instance.ServerGroup));

            if (newChannelId.HasValue)
            {
                ServerManager.Instance.ChannelId = newChannelId.Value;
                MailServiceClient.Instance.Authenticate(authKey, ServerManager.Instance.WorldId);
                ConfigurationServiceClient.Instance.Authenticate(authKey, ServerManager.Instance.WorldId);
                ServerManager.Instance.Configuration = ConfigurationServiceClient.Instance.GetConfigurationObject();
                ServerManager.Instance.MallApi = new MallAPIHelper(ServerManager.Instance.Configuration.MallBaseURL);
                ServerManager.Instance.SynchronizeSheduling();
                return;
            }

            Logger.Error("Could not retrieve ChannelId from Web API.");
            Console.ReadKey();
        }

        private bool ExitHandler(CtrlType sig)
        {
            CommunicationServiceClient.Instance.UnregisterWorldServer(ServerManager.Instance.WorldId);
            ServerManager.Shout(string.Format(Language.Instance.GetMessageFromKey("SHUTDOWN_SEC"), 5));
            foreach (ClientSession sess in ServerManager.Instance.Sessions)
            {
                sess.Character?.Dispose();
            }

            ServerManager.Instance.SaveAll();
            Thread.Sleep(5000);
            return false;
        }

        private void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            if (e == null)
            {
                return;
            }

            ServerManager.Instance.InShutdown = true;
            Logger.Error((Exception)e.ExceptionObject);

            File.AppendAllText(CrashLogPath, e.ExceptionObject + "\n");

            Logger.Debug("Server crashed! Rebooting gracefully...");
            CommunicationServiceClient.Instance.UnregisterWorldServer(ServerManager.Instance.WorldId);
            ServerManager.Shout(string.Format(Language.Instance.GetMessageFromKey("SHUTDOWN_SEC"), 5));
            ServerManager.Instance.SaveAll();
            foreach (ClientSession sess in ServerManager.Instance.Sessions)
            {
                sess.Character?.Dispose();
            }

            Process.Start(_options.RestartExecutable, $"--nomsg --port {_port}");
            Environment.Exit(1);
        }

        public delegate bool EventHandler(CtrlType sig);

        public enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        public static class NativeMethods
        {
            [DllImport("Kernel32")]
            internal static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);
        }
    }
}
