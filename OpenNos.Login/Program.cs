/*
 * This file is part of the OpenNos Emulator Project. See AUTHORS file for Copyright information
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 */

using OpenNos.Core;
using OpenNos.Core.Bootstrapping;
using OpenNos.DAL.EF.Helpers;
using OpenNos.GameObject.Networking;
using OpenNos.Handler;
using OpenNos.Handler.Packets.LoginPackets;
using OpenNos.Master.Library.Client;
using System;
using System.Configuration;
using System.Diagnostics;
using NosTale.Packets.Packets.ClientPackets;

namespace OpenNos.Login
{
    public static class Program
    {
        #region Members

        private static int _port;

        #endregion

        #region Methods

        public static void Main(string[] args)
        {
            try
            {
                ConsoleStartup.InitializeCulture("en-US");
                ConsoleStartupArguments startupArgs = ConsoleStartup.ParseArguments(args);
                ConsoleStartup.SetConsoleTitle("NosTale NosMonsterV3 - Login Server [Port: 4002 - Language: FR]");
                ConsoleStartup.InitializeLogger(typeof(Program));

                int port = Convert.ToInt32(ConfigurationManager.AppSettings["LoginPort"]);
                if (startupArgs.PortOverride.HasValue)
                {
                    port = startupArgs.PortOverride.Value;
                    Console.WriteLine("Port override: " + port);
                }
                _port = port;

                if (!startupArgs.IgnoreStartupMessages)
                {
                    ConsoleStartup.WriteBanner("- NosMonsterV3 -");
                }

                if (CommunicationServiceClient.Instance.Authenticate(ConfigurationManager.AppSettings["MasterAuthKey"]))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[Authentication] The Master Server connection has been established");
                }

                if (!DataAccessHelper.Initialize())
                {
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("[Load] Config has been loaded");

                try
                {
                    AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
                }
                catch (Exception ex)
                {
                    Logger.Error("General Error", ex);
                }

                try
                {
                    PacketFactory.Initialize<WalkPacket>();

                    NetworkManager<LoginCryptography> networkManager = new NetworkManager<LoginCryptography>(
                        ConfigurationManager.AppSettings["IPAddress"],
                        port,
                        typeof(NoS0575PacketHandler),
                        typeof(LoginCryptography),
                        false);
                }
                catch (Exception ex)
                {
                    Logger.LogEventError("INITIALIZATION_EXCEPTION", "General Error Server", ex);
                }
            }
            catch (Exception ex)
            {
                Logger.LogEventError("INITIALIZATION_EXCEPTION", "General Error", ex);
                Console.ReadKey();
            }
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Error((Exception)e.ExceptionObject);
            try
            {
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            Logger.Debug("Login Server crashed! Rebooting gracefully...");
            Process.Start("OpenNos.Login.exe", $"--nomsg --port {_port}");
            Environment.Exit(1);
        }

        #endregion
    }
}
