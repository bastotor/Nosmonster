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
using OpenNos.Master.Library.Interface;
using OpenNos.SCS.Communication.Scs.Communication.EndPoints.Tcp;
using OpenNos.SCS.Communication.ScsServices.Service;
using System;
using System.Configuration;

namespace OpenNos.Master.Server
{
    internal static class Program
    {
        #region Methods

        public static void Main(string[] args)
        {
            try
            {
                ConsoleStartup.InitializeCulture("en-US");
                ConsoleStartupArguments startupArgs = ConsoleStartup.ParseArguments(args);
                ConsoleStartup.SetConsoleTitle("NosTale NosMonsterV3 - Master Server [Port: 4545 - Language: EN]");
                ConsoleStartup.InitializeLogger(typeof(Program));

                int port = Convert.ToInt32(ConfigurationManager.AppSettings["MasterPort"]);
                if (startupArgs.PortOverride.HasValue)
                {
                    port = startupArgs.PortOverride.Value;
                    Console.WriteLine("Port override: " + port);
                }

                if (!startupArgs.IgnoreStartupMessages)
                {
                    ConsoleStartup.WriteBanner("- NosMonsterV3 -");
                }

                if (!DataAccessHelper.Initialize())
                {
                    Console.ReadLine();
                    return;
                }

                Console.WriteLine("[Load] Config has been loaded");

                try
                {
                    string ipAddress = ConfigurationManager.AppSettings["MasterIP"];
                    IScsServiceApplication server = ScsServiceBuilder.CreateService(new ScsTcpEndPoint(ipAddress, port));

                    server.AddService<ICommunicationService, CommunicationService>(new CommunicationService());
                    server.AddService<IConfigurationService, ConfigurationService>(new ConfigurationService());
                    server.AddService<IMailService, MailService>(new MailService());
                    server.AddService<IMallService, MallService>(new MallService());
                    server.AddService<IAuthentificationService, AuthentificationService>(new AuthentificationService());
                    server.ClientConnected += OnClientConnected;
                    server.ClientDisconnected += OnClientDisconnected;

                    server.Start();
                    Console.WriteLine("[Start] Master Server has been started successfully");
                    Console.WriteLine($"[Info] Started at: {DateTime.Now}");
                    if (!startupArgs.IgnoreTelemetry)
                    {
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("General Error Server", ex);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("General Error", ex);
                Console.ReadKey();
            }
        }

        private static void OnClientConnected(object sender, ServiceClientEventArgs e)
        {
            if (e.Client.ClientId == 1)
            {
                Console.WriteLine("[Connect] World Server has been connected");
            }

            if (e.Client.ClientId == 2)
            {
                Console.WriteLine("[Connect] Login Server has been connected");
            }
        }

        private static void OnClientDisconnected(object sender, ServiceClientEventArgs e)
        {
        }

        #endregion
    }
}
